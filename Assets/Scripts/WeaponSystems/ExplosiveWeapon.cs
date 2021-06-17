using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public abstract class ExplosiveWeapon : Weapon
{
    [SerializeField] protected GameObject _explosionPrefab;
    [SerializeField] protected int _power;
    [SerializeField] protected int _doneDistanceDamage;
    [SerializeField] protected int _damageDistance;

    public delegate void OnExplosiveWeaponActive(bool isActive);
    public static OnExplosiveWeaponActive onExplosiveWeaponActive;
    public delegate void OnExplosiveWeaponDestroy();
    public static OnExplosiveWeaponDestroy onExplosiveWeaponDestroy;

    public Vector3 Pos { get { return transform.position; } }

    protected Rigidbody2D _rigidbody;
    protected Camera _mainCamera;
    protected AllParticipants _allParticipants;
    protected Tilemap _tilemap;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _mainCamera = Camera.main;
        onExplosiveWeaponActive.Invoke(true);
        _allParticipants = GameObject.FindObjectOfType<AllParticipants>();
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }
    private void Update()
    {
        CalculateAngle();
    }
    private void CalculateAngle()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 dir = transform.position - mousePos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        UpdateAngle(angle);
    }

    private void UpdateAngle(float angle)
    {
        float currentAngle = angle;
        gameObject.transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
    }

    public void Push(Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        _rigidbody.isKinematic = false;
    }

    public void DesactivateRb()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.isKinematic = true;
    }

    private void CheckDistanceToPlayers(Collider2D other)
    {
        foreach (var participant in _allParticipants.GetParticipants().ToList())
        {
            float distance = Vector2.Distance(transform.position, participant.gameObject.transform.position);
            distance *= _damageDistance;
            if (distance < _doneDistanceDamage)
            {
                participant.GiveDamage(_damage /(int)(distance + 1));
                ThrowPlayer(participant.gameObject, distance);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            onExplosiveWeaponActive.Invoke(false);
            CheckDistanceToPlayers(other);
            Explode();
            CalculateExplosiveFunnel();
        }
        else if (other.CompareTag("Walls"))
        {
            Explode();
        }
    }
    protected abstract void CalculateExplosiveFunnel();
   
    private void ThrowPlayer(GameObject player, float distance)
    {
        if (player.TryGetComponent<Movement>(out Movement movement))
        {
            Vector2 explosionPos = transform.position;
            movement.ThrowAway(explosionPos, _power / (int)(distance + 1));
        }
    }
    private void Explode()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity, null);
        onExplosiveWeaponDestroy?.Invoke();
        Destroy(explosion, 1f);
        Destroy(this.gameObject);
    }
}
