using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    private GameObject _activeWeapon;

    public void Attack()
    {
        _activeWeapon = GameObject.FindObjectOfType<ColdWeapon>().gameObject;
        if(_activeWeapon)
        {
            if(_activeWeapon.TryGetComponent<ColdWeapon>(out ColdWeapon coldWeapon))
            {
                coldWeapon.Attack();
                gameObject.SetActive(false);
            }
        }
    }
}
