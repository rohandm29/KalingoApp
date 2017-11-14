using System.Collections.Generic;
using System.Threading.Tasks;
using Kalingo.Api.Client.Client;
using Kalingo.Core;
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

        public async Task<IEnumerable<Voucher>> GetVouchers()
        {
            var vouchers = await _apiClient.GetVouchers(2);

            return vouchers;
        }

        public async Task<VoucherClaimResponse> ClaimVoucher(int voucherId)
        {
            var claimRequest = new VoucherClaim(voucherId, App.UserId);

            var claimResponse = await _apiClient.SubmitClaim(claimRequest);

            return claimResponse;
        }
    }
}