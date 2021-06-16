using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponsChoice : MonoBehaviour
{
    public delegate void OnWeaponSelected(GameObject weapon);
    public static event OnWeaponSelected onExplosiveWeaponSelected;
    public static event OnWeaponSelected onColdWeaponSelected;

    private bool _isWeaponSelected = false;
    private GameObject _selectedWeapon;
    
    public void ChooseExplosiveWeapon(GameObject weapon)
    {
        RememberChosenWeapon(weapon);
        onExplosiveWeaponSelected?.Invoke(_selectedWeapon);
    }
    public void ChooseColdWeapon(GameObject weapon)
    {
        RememberChosenWeapon(weapon);
        onColdWeaponSelected?.Invoke(_selectedWeapon);
    }
    private void RememberChosenWeapon(GameObject weapon)
    {
        _isWeaponSelected = true;
        _selectedWeapon = weapon;
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        selectedButton.transform.parent.gameObject.SetActive(false);
    }
}
