namespace FTWRK.Infrastructure.Common.Models
{
    public class FileAbstraction : TagLib.File.IFileAbstraction
    {
        public FileAbstraction(string name, byte[] songBytes)
        {
            Name = name;
            var stream = new MemoryStream(songBytes);
            ReadStream = stream;
            WriteStream = stream;
        }

        public void CloseStream(Stream stream)
        {
            stream.Dispose();
        }

        public Stream ReadStream { get; set; }

        public Stream WriteStream { get; set; }

        public string Name { get; set; }
    }
}
