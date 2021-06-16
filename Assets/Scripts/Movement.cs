using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] protected int _speed;
    [SerializeField] protected LayerMask _groundCheckLayerMask;
    [SerializeField] protected Transform _groundCheckTransform;
    [SerializeField] protected float _jumpPower;
    [SerializeField] protected float _forceUp;
    [SerializeField] protected float _sideForce;

    public delegate void OnMyTurn();
    public event OnMyTurn onMyTurn;

    protected SpriteRenderer _spriteRenderer;
    protected int _direction;
    protected bool _isMyTurn = false;
    protected Rigidbody2D _rigidbody;
    protected bool _isInAir = false;

    public void ThrowAway(Vector2 explosionPos, int power)
    {
        Vector2 direction = (Vector2)transform.position - explosionPos;
        GetComponent<Rigidbody2D>().velocity = direction * power;
    }
    public bool IsMyTurn()
    {
        return _isMyTurn;
    }
    public void AllowToWalk()
    {
        _isMyTurn = true;
        onMyTurn?.Invoke();
    }
    public void ProhibitWalking()
    {
        _isMyTurn = false;
    }
    public int GetDirection()
    {
        return _direction;
    }
    public void FlipSprite()
    {
        if (_direction == 1)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_direction == -1)
        {
            _spriteRenderer.flipX = true;
        }
    }
    protected void Jump()
    {
        var vector = _jumpPower * new Vector3(_sideForce * _direction, _forceUp, 0);
        _rigidbody.velocity = vector;
        _isInAir = true;
    }
}
