using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ads : MonoBehaviour
{
    private bool _isRewardAvailable;

    private void Start()
    {
        IronSource.Agent.init("fd816291", IronSourceAdUnits.REWARDED_VIDEO);
        Invoke("Init", 0);
       
    }
    private void Update()
    {
        _isRewardAvailable = IronSource.Agent.isRewardedVideoAvailable();
    }
    public void ShowReward()
    {
        if(_isRewardAvailable)
        {
            IronSource.Agent.showRewardedVideo();
            gameObject.SetActive(false);
        }
    }
    private void Init()
    {
        _isRewardAvailable = IronSource.Agent.isRewardedVideoAvailable();
    }
   
}
