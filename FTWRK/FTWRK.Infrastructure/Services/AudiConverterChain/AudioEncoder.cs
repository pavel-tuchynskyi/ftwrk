using FTWRK.Infrastructure.Common.Models;
using Serilog;

namespace FTWRK.Infrastructure.Services.AudiConverterChain
{
    public class AudioEncoder
    {
        public bool Mp3Convert { get; private set; }
        public bool WavConvert { get; private set; }
        public byte[] Data { get; set; }
        public SongProperties SongInfo { get; private set; }
        public AudioEncoder(byte[] songBytes, SongProperties songInfo)
        {
            if(!FormatValidator(songInfo.Format, out AllowedFormats audioFormat))
            {
                Log.Error("Format is not allowed: {format}", songInfo.Format);
                throw new InvalidOperationException("Format is not allowed");
            }

            if(audioFormat == AllowedFormats.mp3)
            {
                Mp3Convert = true;
            }

            WavConvert = true;
            SongInfo = songInfo;
            Data = songBytes;
        }
        
        private bool FormatValidator(string format, out AllowedFormats audioFormat)
        {
            var parseResult = Enum.TryParse(format, true, out audioFormat);

            if (!parseResult)
            {
                Log.Error("Format is not allowed: {format}", format);
                throw new InvalidOperationException("Format is not allowed");
            }

            return true;
        }
    }

    public enum AllowedFormats
    {
        mp3,
        wav
    }
}
