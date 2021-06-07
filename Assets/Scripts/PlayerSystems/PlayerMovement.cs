using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int _speed;
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] protected float _jumpPower;
    [SerializeField] protected float _forceUp;
    [SerializeField] protected float _sideForce;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private int _direction;
    private float _doubleClickTimeLimit = 0.25f;
    private bool _grounded = true;
    private bool _isWantJump = false;
    private bool _isInAir = false;
    private bool _isProjectileActive = false;
    private Camera _mainCamera;
    private bool _isPausedGame = false;
    private PhotonView _photonView;
    private void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Projectile.onProjectileActive += SetProjectileActivity;
        GameRound.onChangeState += IsRoundChangeState;
        _photonView = GetComponent<PhotonView>();
    }
    private void FixedUpdate()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (_isProjectileActive == false && _isPausedGame == false)
            {
                CheckForAction();
                UpdateGroundedStatus();
            }

            FlipSprite();
        }
    }
    public void ThrowAway(Vector2 explosionPos, int power)
    {
        Vector2 direction = (Vector2)transform.position - explosionPos;
        GetComponent<Rigidbody2D>().velocity = direction * power;
    }
    private void CheckForAction()
    {
        StartCoroutine(CheckFirstClick());
        if (_isWantJump == true)
        {
            Jump();
        }
        else
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        if (Input.GetMouseButton(0) && _grounded)
        {
            _isInAir = false;
            Vector2 _clickPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            float deltaX = _clickPosition.x - transform.position.x;
            if (Mathf.Abs(deltaX) > 0.5f)
            {
                _direction = deltaX > 0 ? 1 : -1;
                
                _rigidbody.velocity = Vector2.right * (_direction * _speed);

            }
        }

    }
    private void Jump()
    {
        var vector = _jumpPower * new Vector3(_sideForce * _direction, _forceUp, 0);
        _rigidbody.velocity = vector;
        _isInAir = true;

    }

    private IEnumerator CheckFirstClick()
    {
        while (enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                yield return CheckForDoubleClick();
            }
            yield return null;
        }
    }
    private IEnumerator CheckForDoubleClick()
    {
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < _doubleClickTimeLimit)
        {
            if (Input.GetMouseButtonDown(0) && !_isInAir)
            {
                _isWantJump = true;
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
        _isWantJump = false;
    }


    private void UpdateGroundedStatus()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheckTransform.position, 0.3f, _groundCheckLayerMask);
    }

    private void FlipSprite()
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

    private void SetProjectileActivity(bool isProjectileActive)
    {
        _isProjectileActive = isProjectileActive;
    }
    private void IsRoundChangeState(bool flag)
    {
        _isPausedGame = flag;
    }
}
