using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private int _damage;
    [SerializeField] private int _power;
    [SerializeField] private int _doneDistanceDamage;
    
    public delegate void OnProjectileActive(bool isActive);
    public static OnProjectileActive onProjectileActive;
    public delegate void OnProjectileDestroy();
    public static OnProjectileDestroy onProjectileDestroy;
    public Vector3 Pos { get { return transform.position; } }

    private Rigidbody2D _rigidbody;
    private Camera _mainCamera;
    private Player[] _allPlayers;
    private Tilemap _tilemap;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _mainCamera = Camera.main;
        onProjectileActive.Invoke(true);
        _allPlayers = GameObject.FindObjectsOfType<Player>();
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
        Vector3 dir =  transform.position - mousePos;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            onProjectileActive.Invoke(false);
            CheckDistanceToPlayers(other);
            Explode();
            CalculateExplosiveFunnel();
        }
        
    }

    private void CalculateExplosiveFunnel()
    {
        Vector3Int position = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y + 0.5f),
            Mathf.RoundToInt(transform.position.z)
        );
        _tilemap.SetTile(_tilemap.WorldToCell(position), null);
    }

    private void CheckDistanceToPlayers(Collider2D other)
    {
        foreach (var _player in _allPlayers)
        {
            float distance = Vector2.Distance(transform.position, _player.gameObject.transform.position);
            distance *= 5;
            if (distance < _doneDistanceDamage)
            {
                _player.GiveDamage(_damage / (int) distance);
                ThrowPlayer(_player.gameObject, distance);
            }
        }
    }

    private void ThrowPlayer(GameObject player, float distance)
    {
        if (player.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            Vector2 explosionPos = transform.position;
            playerMovement.ThrowAway(explosionPos, _power / (int)distance);
        }
    }

    private void Explode()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity, null);
        onProjectileDestroy?.Invoke();
        Destroy(explosion, 1f);
        Destroy(this.gameObject);
    }
}