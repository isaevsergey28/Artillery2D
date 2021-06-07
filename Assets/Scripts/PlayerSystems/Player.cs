using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    public void GiveDamage(int damage)
    {
        _health -= damage;
        CheckAlive();
    }
    private void CheckAlive()
    {
        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
