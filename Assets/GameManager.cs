using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IUnityAdsListener
{
    bool showInterAdMob = false;
    bool showRewAdMob = true;

    string gameId = "4042887";
    public string bannerSurfacingId = "Banner_Android";
    public string rewardSurfacingId = "Rewarded_Android";
    bool testMode = false;
    public Button rewardAdButton;


    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private string appId = "ca-app-pub-2369381059070458~7046096759";

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        StartCoroutine(ShowBannerWhenInitialized());
        

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        RequestBanner();
        CreateAndLoadRewardedAd();

        Advertisement.AddListener(this);

        LoadInterstitial();
    }

    // Update is called once per frame
    void Update()
    {

        if (showRewAdMob)
            rewardAdButton.interactable = rewardedAd.IsLoaded();
        else
            rewardAdButton.interactable = Advertisement.IsReady(rewardSurfacingId);

    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady("Interstitial_Android"))
        {
            Advertisement.Show("Interstitial_Android");
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(bannerSurfacingId);
    }

    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(rewardSurfacingId))
        {
            Advertisement.Show(rewardSurfacingId);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }
    public void RequestBanner()
    {
        //production id
        //string adUnitId = "ca-app-pub-2369381059070458/9726518801";

        //test id
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        // Create a 320x50 banner at the top of the screen.              
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    public void LoadInterstitial()
    {
        //production
        //string adUnitId = "ca-app-pub-2369381059070458/2937749172";

        //test
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    public void RequestInterstitial()
    {
        
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }

        if (interstitial != null)
        {
            interstitial.Destroy();
        }

        LoadInterstitial();
    }

    public void UserChoseToWatchAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            MonoBehaviour.print(
            "Ad not loaded");
        }
    }



    public void HandleUserEarnedReward(object sender, Reward args)
    {
        LogicManager.AddGame();

        string type = args.Type;
        double amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void CreateAndLoadRewardedAd()
    {
        //production
        //string adUnitId = "ca-app-pub-2369381059070458/5372340825";

        //test
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        
        rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
    }

    public void ShowAnInterstitial()
    {
        if(showInterAdMob)
        {
            
            RequestInterstitial();
            showInterAdMob = false;
        }
        else
        {
            ShowInterstitialAd();
            showInterAdMob = true;
        }
    }

    public void ShowAReward()
    {
        if (showRewAdMob)
        {
            UserChoseToWatchAd();
            showRewAdMob = false;
        }
        else
        {
            ShowRewardedVideo();
            showRewAdMob = true;
        }
        
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId.Equals(rewardSurfacingId))
        {
            LogicManager.AddGame();
        }
    }
}
