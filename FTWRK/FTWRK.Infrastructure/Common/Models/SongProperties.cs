namespace FTWRK.Infrastructure.Common.Models
{
    public class SongProperties
    {
        public int SampleRate { get; set; }
        public SampleSize SampleSize { get; set; } = SampleSize.SixteenBit;
        public int Bitrate { get; set; }
        public int Channels { get; set; }
        public string Format { get; set; } = String.Empty;
    }

    public enum SampleSize : int
    {
        EightBit = 1,
        SixteenBit = 2
    }
}
