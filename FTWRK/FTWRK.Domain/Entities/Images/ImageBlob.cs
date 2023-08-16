namespace FTWRK.Domain.Entities.Images
{
    public class ImageBlob
    {
        public string ImageType { get; set; }
        public byte[] ImageBytes { get; set; }

        public ImageBlob(string imageType, byte[] imageBytes)
        {
            ImageType = imageType;
            ImageBytes = imageBytes;
        }

        public ImageBlob()
        {
        }
    }
}
