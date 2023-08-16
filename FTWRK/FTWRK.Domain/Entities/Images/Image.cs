using FTWRK.Domain.Attributes;

namespace FTWRK.Domain.Entities.Images
{
    [BsonCollection("Images")]
    public class Image
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ImageBlob ImageBlob { get; set; }
    }
}
