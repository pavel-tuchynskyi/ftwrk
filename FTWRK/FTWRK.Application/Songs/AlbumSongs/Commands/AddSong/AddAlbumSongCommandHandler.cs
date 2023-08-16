using AutoMapper;
using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Application.Hubs;
using FTWRK.Domain.Entities.Songs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.AddSong
{
    public class AddAlbumSongCommandHandler : IRequestHandler<AddAlbumSongCommand, Unit>
    {
        private readonly ISongServiceFactory _serviceFactory;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAudioService _audioService;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private const string FunctionUrlKey = "ConvertFunctionUrl";
        private readonly HttpClient _httpClient;
        private readonly Guid _userId;

        public AddAlbumSongCommandHandler(ISongServiceFactory serviceFactory, IHttpClientFactory httpClientFactory, IMapper mapper, 
            IConfiguration configuration, IAudioService audioService, IUserContextService userContext, IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            _serviceFactory = serviceFactory;
            _httpClient = httpClientFactory.CreateClient();
            _mapper = mapper;
            _configuration = configuration;
            _audioService = audioService;
            _hubContext = hubContext;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(AddAlbumSongCommand request, CancellationToken cancellationToken)
        {
            var albumSongBuilder = new AlbumSongBuilder();

            await _hubContext.Clients.Client(request.ConnectionId).ReportProgress(ProgressState.Converting);

            AlbumSong song = albumSongBuilder
                .SongInfo 
                    .MapFromSource(_mapper, request)
                    .SetDuration(await _audioService.GetSongDuration(request.SongBlob))
                .SongBlob
                    .Create(await CallConvertFunction(request.SongBlob));

            var songService = _serviceFactory.GetSongService(SongType.Album);

            await _hubContext.Clients.Client(request.ConnectionId).ReportProgress(ProgressState.Saving);

            var result = await songService.Add(request.AlbumId, _userId, song);

            if (result)
            {
                await _hubContext.Clients.Client(request.ConnectionId).ReportProgress(ProgressState.Succeeded);
            }
            else
            {
                await _hubContext.Clients.Client(request.ConnectionId).ReportProgress(ProgressState.Failed, "Failed to save this song");
            }

            return Unit.Value;
        }

        private async Task<byte[]> CallConvertFunction(IFormFile song)
        {
            var functionUrl = _configuration.GetValue<string>(FunctionUrlKey);
            var req = new HttpRequestMessage(HttpMethod.Post, functionUrl);
            var content = new MultipartFormDataContent();

            using (var songStream = song.OpenReadStream())
            {
                var buffer = new byte[song.Length];
                await songStream.ReadAsync(buffer, 0, (int)song.Length);

                content.Add(new StreamContent(new MemoryStream(buffer)), song.Name, song.FileName);
            }

            req.Content = content;

            var res = await _httpClient.SendAsync(req);
            var responseBody = await res.Content.ReadAsStringAsync();
            var convertedSongBytes = JsonConvert.DeserializeObject<byte[]>(responseBody);

            content.Dispose();

            return convertedSongBytes;
        }
    }
}
