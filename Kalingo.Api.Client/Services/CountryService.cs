using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kalingo.Api.Client.Client;
using Kalingo.Games.Contract.Entity;

namespace Kalingo.Api.Client.Services
{
    public class CountryService
    {
        private readonly KalingoApiClient _apiClient;

        private static IEnumerable<CountryResponse> _countries;

        public CountryService()
        {
            _apiClient = new KalingoApiClient();
        }

        public async Task<IEnumerable<CountryResponse>> GetCountries()
        {
            try
            {
                return _countries  ?? (_countries = await _apiClient.GetCountries());
            }
            catch (System.Exception)
            {
                return new List<CountryResponse>();
            }
        }

        public static int GetCountryId(string name)
        {
            return _countries.First(x => x.Name == name).Id;
        }
    }
}