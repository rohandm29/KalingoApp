using System;
using Android.Content;
using Android.Gms.Ads.Reward;

namespace Kalingo.AdMob
{
    public class RewardedAdListener : IRewardedVideoAdListener
    {
        private readonly Context _context;
        public IntPtr Handle { get; }


        public RewardedAdListener(Context context)
        {
            _context = context;
        }

        public void Dispose()
        {            
        }

        public void OnRewarded(IRewardItem reward)
        {
            var amount = reward.Amount; // 29

            var rewardType = reward.Type; // Kalingo6637403349Rewarded
        }
            
        public void OnRewardedVideoAdClosed()
        {
        }

        public void OnRewardedVideoAdFailedToLoad(int errorCode)
        {
        }

        public void OnRewardedVideoAdLeftApplication()
        {
        }

        public void OnRewardedVideoAdLoaded()
        {
        }

        public void OnRewardedVideoAdOpened()
        {
        }

        public void OnRewardedVideoStarted()
        {
        }
    }
}