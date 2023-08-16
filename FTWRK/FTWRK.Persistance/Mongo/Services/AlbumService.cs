using FTWRK.Application.Common.DTO.Albums;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Infrastructure.Extensions;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Persistance.Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog;

namespace FTWRK.Persistance.Cosmos.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IMongoCollection<Album> _collection;
        private readonly IMongoContext _dbContext;

        public AlbumService(IMongoContext dbContext)
        {
            _collection = dbContext.GetCollection<Album>();
            _dbContext = dbContext;
        }

        public async Task<PagedList<AlbumDto>> GetAll(QueryParameters parameters)
        {
            Log.Debug("{method} is started in {service}", nameof(GetAll), nameof(AlbumService));

            var albums = await _collection
                .AsQueryable()
                .Where(x => x.IsArchived == false)
                .Filter(parameters.Filter)
                .Sort(parameters.OrderBy)
                .Select(x => new AlbumDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Year = x.Year,
                    AlbumType = x.AlbumType,
                    Poster = x.Poster
                })
                .ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

            Log.Debug("{method} is finished successfully in {service}", nameof(GetAll), nameof(AlbumService));

            return albums;
        }

        public async Task<AlbumDetailsDto> GetById(Guid id)
        {
            Log.Debug("{method} is started in {service}", nameof(GetById), nameof(AlbumService));

            var userCollectionName = _dbContext.GetCollectionName<ApplicationUser>();

            var result = await _collection.Aggregate()
                .Match(x => x.Id == id && x.IsArchived == false)
                .Lookup(userCollectionName, nameof(Album.CreatorId), "_id", nameof(ApplicationUser))
                .Unwind(nameof(ApplicationUser))
                .Project(new BsonDocument 
                {
                    { nameof(AlbumDetailsDto.CreatorId), $"${nameof(ApplicationUser)}._id" },
                    { nameof(AlbumDetailsDto.CreatorName), $"${nameof(ApplicationUser)}.{nameof(ApplicationUser.FullName)}"},
                    { nameof(AlbumDetailsDto.Title), $"${nameof(Album.Title)}" },
                    { nameof(AlbumDetailsDto.Year), $"${nameof(Album.Year)}" },
                    { nameof(AlbumDetailsDto.Genres), $"${nameof(Album.Genres)}" },
                    { nameof(AlbumDetailsDto.AlbumType), $"${nameof(Album.AlbumType)}" },
                    { nameof(AlbumDetailsDto.Poster), $"${nameof(Album.Poster)}" }
                })
                .As<AlbumDetailsDto>()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                Log.Error("Can't find this album with id: {id}", id);
                throw new NotFoundException("Can't find this album");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(GetById), nameof(AlbumService));

            return result;
        }

        public async Task<bool> Add(Album album)
        {
            Log.Debug("{method} is started in {service}", nameof(Add), nameof(AlbumService));
            await _collection.InsertOneAsync(album);

            Log.Debug("{method} is finished successfully in {service}", nameof(Add), nameof(AlbumService));

            return true;
        }

        public async Task<bool> Update(Guid id, Album album)
        {
            Log.Debug("{method} is started in {service}", nameof(Update), nameof(AlbumService));

            var updateDef = Builders<Album>.Update.CreateUpdateDefinition(album);

            var result = await _collection.UpdateOneAsync(x => x.Id == id && x.CreatorId == album.CreatorId, updateDef);
            
            if(result.ModifiedCount == 0)
            {
                Log.Error("Can't update album with id: {id}", id);
                throw new ApplicationException("Can't update this album");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(Update), nameof(AlbumService));
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            Log.Debug("{method} is started in {service}", nameof(Delete), nameof(AlbumService));

            var filter = Builders<Album>.Filter.Eq(x => x.Id, id);
            var update = Builders<Album>.Update.Set(x => x.IsArchived, true);

            var result = await _collection.UpdateOneAsync(filter, update);

            if(result.ModifiedCount == 0)
            {
                Log.Error("Can't delete album with id: {id}", id);
                throw new ApplicationException("Can't delete this album");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(Delete), nameof(AlbumService));

            return true;
        }
    }
}
