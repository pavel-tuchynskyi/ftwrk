using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.EmailTemplates;
using FTWRK.Persistance.Common.Interfaces;
using MongoDB.Driver;
using Serilog;

namespace FTWRK.Persistance.Mongo.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IMongoCollection<EmailTemplate> _collection;
        public TemplateService(IMongoContext dbContext)
        {
            _collection = dbContext.GetCollection<EmailTemplate>();
        }

        public async Task<EmailTemplate> GetEmailTemplateAsync(string key)
        {
            Log.Debug("{method} is started in {service}", nameof(GetEmailTemplateAsync), nameof(TemplateService));
            var template = await _collection.Find(x => x.Key == key).SingleOrDefaultAsync();

            if (template == null)
            {
                Log.Error("Can't find template: {key}", key);
                throw new NotFoundException("Can't find thid template");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(GetEmailTemplateAsync), nameof(TemplateService));
            return template;
        }
    }
}
