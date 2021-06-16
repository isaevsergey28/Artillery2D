using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWeaponSpawner : WeaponSpawner
{
    [SerializeField] private GameObject _currentColdWeapon;
   

    private void Start()
    {
        WeaponsChoice.onColdWeaponSelected += RememberChosenWeapon;
    }
    private void OnDisable()
    {
        WeaponsChoice.onColdWeaponSelected -= RememberChosenWeapon;
    }
    private void RememberChosenWeapon(GameObject weapon)
    {
        _selectedWeapon = weapon;
        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        if (_currentColdWeapon == null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition = new Vector3(transform.position.x + 0.5f, transform.position.y, 0f);
            _currentColdWeapon = Instantiate(_selectedWeapon, spawnPosition, _selectedWeapon.transform.rotation, null);
        }
    }
}
