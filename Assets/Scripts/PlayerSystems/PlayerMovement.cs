using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Movement
{
   

    private float _doubleClickTimeLimit = 0.25f;
    private bool _grounded = true;
    private bool _isWantJump = false;
    private bool _isProjectileActive = false;
    private Camera _mainCamera;
    private bool _isPausedGame = false;

    private void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ExplosiveWeapon.onExplosiveWeaponActive += SetProjectileActivity;
        GameRound.onChangeState += IsRoundChangeState;
    }
    private void OnDisable()
    {
        ExplosiveWeapon.onExplosiveWeaponActive -= SetProjectileActivity;
        GameRound.onChangeState -= IsRoundChangeState;
    }
    private void FixedUpdate()
    {
        if (!EventSystem.current.currentSelectedGameObject)
        {
            if (_isProjectileActive == false && _isPausedGame == false && _isMyTurn)
            {
                CheckForAction();
                UpdateGroundedStatus();
            }
            FlipSprite();
        }
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
        if (Input.touchCount == 1 && _grounded)//Input.GetMouseButton(0) && _grounded || 
        {
            _isInAir = false;
            //Vector2 _clickPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _clickPosition = _mainCamera.ScreenToWorldPoint(Input.touches[0].position);
            float deltaX = _clickPosition.x - transform.position.x;
            if (Mathf.Abs(deltaX) > 1f)
            {
                _direction = deltaX > 0 ? 1 : -1;
                
                _rigidbody.velocity = Vector2.right * (_direction * _speed);

            }
        }

    }
    
    private IEnumerator CheckFirstClick()
    {
        while (enabled)
        {
            if (Input.touchCount == 1)//Input.GetMouseButtonDown(0) ||
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
            foreach (var touch in Input.touches)
            {
                if (touch.tapCount == 2 && !_isInAir)
                {
                    _isWantJump = true;
                    yield break;
                }
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

   

    private void SetProjectileActivity(bool isProjectileActive)
    {
        _isProjectileActive = isProjectileActive;
    }
    private void IsRoundChangeState(bool flag)
    {
        _isPausedGame = flag;
    }
}
