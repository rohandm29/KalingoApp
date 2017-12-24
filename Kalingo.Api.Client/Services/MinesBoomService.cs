using System.Collections.Generic;
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

        public async Task<NewMinesboomResponse> CreateMinesBoom(int userId, bool playAgain)
        {
            try
            {
                var newMinesboomResponse = await _apiClient.CreateMinesBoom(userId, playAgain);

                SaveSessionGameId(newMinesboomResponse.GameId);

                return newMinesboomResponse;
            }
            catch (System.Exception)
            {
                return default(NewMinesboomResponse);
            }
        }

        public async Task<MinesboomSelectionResponse> Submit(int optionSelected, bool playAgain)
        {
            try
            {
                var mbArgs = new MinesboomSelectionRequest(App.GameId, App.UserId, optionSelected, playAgain);

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