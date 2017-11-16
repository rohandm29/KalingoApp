using System.Threading.Tasks;
using Kalingo.Api.Client.Client;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.MinesBoom;

namespace Kalingo.Api.Client.Services
{
    public class MinesBoomService
    {
        private readonly KalingoApiClient _apiClient;

        public MinesBoomService()
        {
            _apiClient = new KalingoApiClient();
        }

        public async Task<bool> CreateMinesBoom(int userId)
        {
            var gameId = await _apiClient.CreateMinesBoom(userId);

            SaveSessionGameId(gameId);

            return true;
        }

        public async Task<MinesboomSelectionResponse> Submit(int optionSelected)
        {
            var mbArgs = new MinesboomSelectionRequest(App.GameId, App.UserId, optionSelected);

            var result = await _apiClient.SubmitMinesBoom(mbArgs);

            return result;
        }

        public async Task Terminate()
        {
            var gameArgs = new GameArgs(App.GameId, App.UserId, App.MinesBoomId);

            await _apiClient.TerminateMinesBoom(gameArgs);
        }

        private static void SaveSessionGameId(int gameId)
        {
            App.GameId = gameId;
        }
    }
}