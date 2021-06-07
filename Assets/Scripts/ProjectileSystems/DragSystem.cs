using UnityEngine;

public class DragSystem : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private ProjectileTrajectory _projectileTrajectory;
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
        ProjectileSpawner.onProjectileSpawn += SetProjectile;
    }

    private void Update()
    {
        if(_projectile)
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
        _projectile.DesactivateRb();
        _startPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        _projectileTrajectory.Show();
    }

    private void OnDrag()
    {
        _endPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _distance = Vector2.Distance(_startPoint, _endPoint);
        _direction = (_startPoint - _endPoint).normalized;
        _force = _direction * (_distance * _pushForce);

        _projectileTrajectory.UpdateDots(_projectile.Pos, _force);
    }

    private void OnDragEnd()
    {
        _projectile.ActivateRb();

        _projectile.Push(_force);

        _projectileTrajectory.Hide();
    }

    private void SetProjectile(Projectile projectile)
    {
        _projectile = projectile;
        _isProjectileSpawned = true;
        _projectile.DesactivateRb();
    }
}