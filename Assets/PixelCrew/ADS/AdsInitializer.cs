using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace PixelCrew.ADS
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField]  string androidGameID = "4776285";
        [SerializeField]  string iOSGameID = "4776284";
        [SerializeField]  bool testMode = true;
        private string gameID;
        void Awake ()
        {
            InitializeAds();
        }
    
        public void  InitializeAds()
        {
            gameID =(Application.platform == RuntimePlatform.IPhonePlayer) ? iOSGameID: androidGameID;
            Advertisement.Initialize(gameID, testMode, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads initialization Failed: {error.ToString()} - {message}");
        }
    }
    
    
}