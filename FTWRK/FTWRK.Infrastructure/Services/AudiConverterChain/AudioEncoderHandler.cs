namespace FTWRK.Infrastructure.Services.AudiConverterChain
{
    public abstract class AudioEncoderHandler
    {
        public AudioEncoderHandler Successor { get; set; }
        public abstract Task<byte[]> Handle(AudioEncoder encoder);
    }
}
