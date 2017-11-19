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
            try
            {
                var gameId = await _apiClient.CreateMinesBoom(userId);

                SaveSessionGameId(gameId);

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<MinesboomSelectionResponse> Submit(int optionSelected)
        {
            try
            {
                var mbArgs = new MinesboomSelectionRequest(App.GameId, App.UserId, optionSelected);

                var result = await _apiClient.SubmitMinesBoom(mbArgs);

                return result;
            }
            catch (System.Exception)
            {
                return new MinesboomSelectionResponse(0, false, "");
            }
        }

        public async Task Terminate()
        {
            try
            {
                var gameArgs = new GameArgs(App.GameId, App.UserId, App.MinesBoomId);

                await _apiClient.TerminateMinesBoom(gameArgs);
            }
            catch (System.Exception)
            {
                // ignored
            }
        }

        private static void SaveSessionGameId(int gameId)
        {
            App.GameId = gameId;
        }
    }
}