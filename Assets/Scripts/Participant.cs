using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour
{
    [SerializeField] protected int _health;

    public delegate void OnDeath(Participant participant);
    public static event OnDeath onDeath;

    public delegate void OnHurt(int damage);
    public event OnHurt onHurt;

    protected Movement _movement;

    protected int _walkNumber;

    protected AllParticipants _allParticipants;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        GameRound.onRoundStart += CheckRoundNumber;
        _allParticipants = GameObject.FindObjectOfType<AllParticipants>();
    }
    private void OnDisable()
    {
        GameRound.onRoundStart -= CheckRoundNumber;
    }
    public void SetWalkNumber(int walkNumber)
    {
        _walkNumber = walkNumber;
    }
    public int GetWalkNumber()
    {
        return _walkNumber;
    }
    public void GiveDamage(int damage)
    {
        _health -= damage;
        onHurt?.Invoke(_health);
        CheckAlive();
    }
    public int GetHealth()
    {
        return _health;
    }
    public void DecrementWalkNumber()
    {
        _walkNumber--;
    }
    private void CheckAlive()
    {
        if (_health <= 0)
        {
            onDeath?.Invoke(this);
        }
    }
    private void CheckRoundNumber(int roundNumber)
    {
        if(_walkNumber == roundNumber)
        {
            _movement.AllowToWalk();
        }
        else
        {
            _movement.ProhibitWalking();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadLine"))
        {
            onDeath?.Invoke(this);
        }
    }
}
