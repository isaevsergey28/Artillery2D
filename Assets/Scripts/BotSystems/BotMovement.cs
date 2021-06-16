using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : Movement
{
    [SerializeField] private float _meleeRange;
    [SerializeField] private float _stopDistance;

    public delegate void OnCanAttack();
    public static event OnCanAttack onCanAttackWitchColdWeapon;
    public static event OnCanAttack onCanAttackWitchExplosiveWeapon;

    private BotBehaviour _botBehaviour;
    private GameObject _target;
    private bool _isCanMove = true;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _botBehaviour = GetComponent<BotBehaviour>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        onMyTurn += SetMove;
    }
    private void OnDisable()
    {
        onMyTurn -= SetMove;
    }
    private void Update()
    {
        if(!_isMyTurn)
        {
            return;
        }
        _target = _botBehaviour.GetClosestEnemy();
        if (_target && _isCanMove)
        {
            float distance = Vector2.Distance(gameObject.transform.position, _target.transform.position);
            if(distance < _stopDistance)
            {
                _isCanMove = false;
                onCanAttackWitchColdWeapon?.Invoke();
            }
            else if (distance < _meleeRange)
            {
                transform.position = Vector2.Lerp(transform.position, _target.transform.position, Time.deltaTime * _speed);
                float deltaX = _target.transform.position.x - transform.position.x;
                _direction = deltaX > 0 ? 1 : -1;
                CheckForJump();
                FlipSprite();
            }
            else
            {
                _isCanMove = false;
                onCanAttackWitchExplosiveWeapon?.Invoke();
            }
        }
    }
    private void CheckForJump()
    {
        RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, Vector2.right * _direction, 0.6f);
        
        for (int i = 0; i < allHits.Length; i++)
        {
            if(allHits[i].transform.name == "Tilemap")
            {
                Jump();
            }
        }
    }
    private void SetMove()
    {
        _isCanMove = true;
    }
}
