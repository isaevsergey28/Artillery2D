using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;

    private void Start()
    {
        Player.onPlayerWalks += ShowOrHideInventory;
    }
    private void OnDisable()
    {
        Player.onPlayerWalks -= ShowOrHideInventory;
    }
    private void ShowOrHideInventory(bool flag)
    {
        _inventory.SetActive(flag);
    }
}
