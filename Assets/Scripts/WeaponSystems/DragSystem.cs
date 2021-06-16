using UnityEngine;

public class DragSystem : MonoBehaviour
{
    [SerializeField] private ExplosiveWeapon _explosiveWeapon;
    [SerializeField] private ExplosiveWeaponTrajectory _explosiveWeaponTrajectory;
    [SerializeField] private float _pushForce = 4f;

    private bool _isDragging = false;
    private bool _isProjectileSpawned = false;
    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private Vector2 _direction;
    private Vector2 _force;
    private float _distance;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        ExplosiveWeaponSpawner.onExplosiveWeaponSpawn += SetProjectile;
    }
    private void OnDisable()
    {
        ExplosiveWeaponSpawner.onExplosiveWeaponSpawn -= SetProjectile;
    }
    private void Update()
    {
        if(_explosiveWeapon)
        {
            CheckDragState();
        }
        else
        {
            _isProjectileSpawned = false;
        }
    }

    private void CheckDragState()
    {
        if (_isProjectileSpawned)
        {
            _isProjectileSpawned = false;
            _isDragging = true;
            OnDragStart();
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            OnDragEnd();
        }

        if (_isDragging)
        {
            OnDrag();
        }
    }

    private void OnDragStart()
    {
        _explosiveWeapon.DesactivateRb();
        _startPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        _explosiveWeaponTrajectory.Show();
    }

    private void OnDrag()
    {
        _endPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _distance = Vector2.Distance(_startPoint, _endPoint);
        _direction = (_startPoint - _endPoint).normalized;
        _force = _direction * (_distance * _pushForce);

        _explosiveWeaponTrajectory.UpdateDots(_explosiveWeapon.Pos, _force);
    }

    private void OnDragEnd()
    {
        _explosiveWeapon.ActivateRb();

        _explosiveWeapon.Push(_force);

        _explosiveWeaponTrajectory.Hide();
    }

    private void SetProjectile(ExplosiveWeapon projectile)
    {
        _explosiveWeapon = projectile;
        _isProjectileSpawned = true;
        _explosiveWeapon.DesactivateRb();
    }
}