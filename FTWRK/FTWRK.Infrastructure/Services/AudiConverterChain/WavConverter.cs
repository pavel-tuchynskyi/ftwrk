using FTWRK.Infrastructure.Common.Models;
using OggVorbisEncoder;
using Serilog;

namespace FTWRK.Infrastructure.Services.AudiConverterChain
{
    public class WavConverter : AudioEncoderHandler
    {
        private readonly int _writeBufferSize = 20000;

        public async override Task<byte[]> Handle(AudioEncoder encoder)
        {
            if (encoder.WavConvert)
            {
                encoder.Data = ConvertToOgg(44100, 2, encoder.Data, encoder.SongInfo);
            }
            
            if (Successor != null)
            {
                encoder.Data = await Successor.Handle(encoder);
            }

            return encoder.Data;
        }

        private byte[] ConvertToOgg(int outputSampleRate, int outputChannels, byte[] songBytes, SongProperties songInfo)
        {
            Log.Debug("Starting converting to ogg");
            int numWavSamples = (songBytes.Length / (int)songInfo.SampleSize / songInfo.Channels);
            float wavDuraton = numWavSamples / (float)songInfo.SampleRate;

            int numOutputSamples = (int)(wavDuraton * outputSampleRate);

            numOutputSamples = (numOutputSamples / _writeBufferSize) * _writeBufferSize;

            float[][] outSamples = new float[outputChannels][];

            for (int ch = 0; ch < outputChannels; ch++)
            {
                outSamples[ch] = new float[numOutputSamples];
            }

            for (int sampleNumber = 0; sampleNumber < numOutputSamples; sampleNumber++)
            {
                float rawSample = 0.0f;

                for (int ch = 0; ch < outputChannels; ch++)
                {
                    int sampleIndex = (sampleNumber * songInfo.Channels) * (int)songInfo.SampleSize;

                    if (ch < songInfo.Channels) sampleIndex += (ch * (int)songInfo.SampleSize);

                    switch (songInfo.SampleSize)
                    {
                        case SampleSize.EightBit:
                            rawSample = ByteToSample(songBytes[sampleIndex]);
                            break;
                        case SampleSize.SixteenBit:
                            rawSample = ShortToSample((short)(songBytes[sampleIndex + 1] << 8 | songBytes[sampleIndex]));
                            break;
                    }

                    outSamples[ch][sampleNumber] = rawSample;
                }
            }

            return GenerateFile(outSamples, outputSampleRate, outputChannels);
        }

        private byte[] GenerateFile(float[][] floatSamples, int sampleRate, int channels)
        {
            using MemoryStream outputData = new MemoryStream();

            var info = VorbisInfo.InitVariableBitRate(channels, sampleRate, 0.5f);

            var serial = new Random().Next();
            var oggStream = new OggStream(serial);

            var comments = new Comments();
            comments.AddTag("Name", Guid.NewGuid().ToString());

            var infoPacket = HeaderPacketBuilder.BuildInfoPacket(info);
            var commentsPacket = HeaderPacketBuilder.BuildCommentsPacket(comments);
            var booksPacket = HeaderPacketBuilder.BuildBooksPacket(info);

            oggStream.PacketIn(infoPacket);
            oggStream.PacketIn(commentsPacket);
            oggStream.PacketIn(booksPacket);

            FlushPages(oggStream, outputData, true);

            var processingState = ProcessingState.Create(info);

            for (int readIndex = 0; readIndex <= floatSamples[0].Length; readIndex += _writeBufferSize)
            {
                if (readIndex == floatSamples[0].Length)
                {
                    processingState.WriteEndOfStream();
                }
                else
                {
                    processingState.WriteData(floatSamples, _writeBufferSize, readIndex);
                }

                while (!oggStream.Finished && processingState.PacketOut(out OggPacket packet))
                {
                    oggStream.PacketIn(packet);

                    FlushPages(oggStream, outputData, false);
                }
            }

            FlushPages(oggStream, outputData, true);

            Log.Debug("Converting to ogg has finished");
            return outputData.ToArray();
        }

        private void FlushPages(OggStream oggStream, Stream output, bool force)
        {
            while (oggStream.PageOut(out OggPage page, force))
            {
                output.Write(page.Header, 0, page.Header.Length);
                output.Write(page.Body, 0, page.Body.Length);
            }
        }

        private static float ByteToSample(short value)
        {
            return value / 128f;
        }

        private static float ShortToSample(short value)
        {
            return value / 32768f;
        }
    }
}
