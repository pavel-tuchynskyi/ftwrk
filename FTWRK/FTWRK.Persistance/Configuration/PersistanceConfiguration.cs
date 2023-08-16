using FTWRK.Application.Common.Interfaces;
using FTWRK.Infrastructure;
using FTWRK.Persistance.BlobStorage;
using FTWRK.Persistance.BlobStorage.Services;
using FTWRK.Persistance.Common.Interfaces;
using FTWRK.Persistance.Cosmos.Services;
using FTWRK.Persistance.Mongo;
using FTWRK.Persistance.Mongo.Services;
using FTWRK.Persistance.Mongo.Services.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FTWRK.Persistance.Configuration
{
    public static class PersistanceConfiguration
    {
        public static void AddPersistace(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureMongoDb(services);
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.Configure<BlobStorageOptions>(configuration.GetSection(nameof(BlobStorageOptions)));
            services.AddScoped<IBlobStorageContext, BlobStorageContext>();
            services.AddScoped<ISongBlobService, SongBlobService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<ISongServiceFactory, SongServiceFactory>();
            services.AddScoped<AlbumSongService>()
                .AddScoped<ISongService, AlbumSongService>(s => s.GetService<AlbumSongService>());
            services.AddScoped<PlaylistSongService>()
                        .AddScoped<ISongService, PlaylistSongService>(s => s.GetService<PlaylistSongService>());
            services.AddScoped<IUserAnalyticsService, UserAnalyticsService>();
            services.AddScoped<IRecommendationService, RecommendationService>();
        }

        private static void ConfigureMongoDb(IServiceCollection services)
        {
            services.AddSingleton<IMongoClient, MongoClient>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<MongoOptions>>().Value;
                return new MongoClient(options.ConnectionString);
            });
        }
    }
}
