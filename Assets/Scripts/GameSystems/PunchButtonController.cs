using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _puchButton;

    private void Start()
    {
        ColdWeapon.onColdWeaponSpawn += ShowButton;
    }
    private void OnDisable()
    {
        ColdWeapon.onColdWeaponSpawn -= ShowButton;
    }
    private void ShowButton()
    {
        _puchButton.SetActive(true);
    }
}
