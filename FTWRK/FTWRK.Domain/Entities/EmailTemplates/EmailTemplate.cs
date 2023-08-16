using FTWRK.Domain.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace FTWRK.Domain.Entities.EmailTemplates
{
    [BsonCollection("EmailTemplates")]
    public class EmailTemplate
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
