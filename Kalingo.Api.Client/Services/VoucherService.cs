using System.Collections.Generic;
using System.Threading.Tasks;
using Kalingo.Api.Client.Client;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.Voucher;

namespace Kalingo.Api.Client.Services
{
    public class VoucherService
    {
        private readonly KalingoApiClient _apiClient;

        public VoucherService()
        {
            _apiClient = new KalingoApiClient();
        }

        public async Task<IEnumerable<VoucherResponse>> GetVouchers()
        {
            try
            {
                var vouchers = await _apiClient.GetVouchers(App.CountryId);

                return vouchers;
            }
            catch (System.Exception)
            {
                return new List<VoucherResponse>();
            }
        }

        public async Task<VoucherClaimResponse> ClaimVoucher(int voucherId)
        {
            try
            {
                var claimRequest = new VoucherClaimRequest(voucherId, App.UserId);

                var claimResponse = await _apiClient.SubmitClaim(claimRequest);

                return claimResponse;
            }
            catch (System.Exception)
            {
                return new VoucherClaimResponse(VoucherCodes.NoVouchers);
            }
        }
    }
}