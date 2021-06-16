using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWeapon : Weapon
{
    [SerializeField] private float _strikeRange;

    public delegate void OnColdWeaponSpawn();
    public static OnColdWeaponSpawn onColdWeaponSpawn;
    public delegate void OnColdWeaponDestroy();
    public static OnColdWeaponDestroy onColdWeaponDestroy;
    
    private AllParticipants _allParticipants;
    private GameObject _activeParticipant;
    private Movement _movement;
    private float _distanceToPlayer = 0.4f;
    private int _direction;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    

    private void Awake()
    {
        _allParticipants = GameObject.FindObjectOfType<AllParticipants>();

        foreach (var participant in _allParticipants.GetParticipants())
        {
            Movement _tempMovement = participant.GetComponent<Movement>();
            if(_tempMovement.IsMyTurn())
            {
                _activeParticipant = participant.gameObject;
                _movement = _tempMovement;
            }
        }
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_activeParticipant.TryGetComponent<Player>(out Player player))
        {
            onColdWeaponSpawn?.Invoke();
        }
    }
    private void Update()
    {
        TranslateWeapon();
    }

    private void TranslateWeapon()
    {
        _direction = _movement.GetDirection();
        if (_direction == 1)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_direction == -1)
        {
            _spriteRenderer.flipX = false;
        }
        transform.position = new Vector3(_activeParticipant.transform.position.x + _distanceToPlayer * _direction, _activeParticipant.transform.position.y, 0f);
    }

    public void Attack()
    {
        if (_direction == 1)
        {
            _animator.SetBool("isFightRight", true);
        }
        else
        {
             _animator.SetBool("isFightLeft", true);
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(DestroyWeapon());
    }
    private IEnumerator DestroyWeapon()
    {
        onColdWeaponDestroy?.Invoke();
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 2);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Movement>(out Movement movement))
        {
            if(movement.IsMyTurn())
            {
                return;
            }
            movement.ThrowAway(transform.position, _damage);
        }
        if (other.TryGetComponent<Participant>(out Participant participant))
       {
            participant.GiveDamage(_damage);
       }
        
    }
}
