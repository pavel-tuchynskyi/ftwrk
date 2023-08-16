using NAudio.Wave;
using Serilog;

namespace FTWRK.Infrastructure.Services.AudiConverterChain
{
    public class Mp3Converter : AudioEncoderHandler
    {
        public async override Task<byte[]> Handle(AudioEncoder encoder)
        {
            if (encoder.Mp3Convert)
            {
                encoder.Data = await ConvertToWav(encoder.Data);
            }

            if (Successor != null)
            {
                encoder.Data = await Successor.Handle(encoder);
            }

            return encoder.Data;
        }

        private async Task<byte[]> ConvertToWav(byte[] songBytes)
        {
            Log.Debug("Starting converting to mp3");
            using (var inputStream = new MemoryStream(songBytes))
            using (var outputStream = new MemoryStream())
            using (var reader = new Mp3FileReader(inputStream))
            using (var writer = new WaveFileWriter(outputStream, reader.WaveFormat))
            {
                reader.CopyTo(writer);
                var buffer = new byte[outputStream.Length];
                outputStream.Position = 0;
                await outputStream.ReadAsync(buffer, 0, buffer.Length);

                Log.Debug("Converting to mp3 is finishing");
                return buffer;
            }
        }
    }
}
