using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsController : MonoBehaviour
{
    [SerializeField] private GameObject _adsPanel;

    private void Start()
    {
        Player.onPlayerDeath += ShowAdsPanel;
    }
    private void OnDisable()
    {
        Player.onPlayerDeath -= ShowAdsPanel;
    }
    private void ShowAdsPanel()
    {
        _adsPanel.SetActive(true);
    }
}
