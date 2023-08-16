using MongoDB.Bson.Serialization.Attributes;

namespace FTWRK.Domain.Common
{
    public abstract class Entity
    {
        [BsonId]
        public Guid Id { get; protected set; }
        protected Entity(Guid id) => Id = id;

        public Entity()
        {
        }
    }
}
