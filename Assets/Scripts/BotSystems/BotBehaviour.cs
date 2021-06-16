using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] _allWeapons;
    [SerializeField] private float _pushForce;

    private GameObject _selectedWeapon;
    private AllParticipants _allParticipants;
    private GameObject _closestEnemy;
    private Vector2 _weaponSpawnPos;
    private Movement _movement;

    private void Start()
    {
        _allParticipants = GameObject.FindObjectOfType<AllParticipants>();
        Physics2D.IgnoreCollision(FindObjectOfType<Player>().GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
        _movement = GetComponent<Movement>();
        _movement.onMyTurn += FindClosestEnemy;
        BotMovement.onCanAttackWitchColdWeapon += AttackWithColdWeapon;
        BotMovement.onCanAttackWitchExplosiveWeapon += AttackWithExplosiveWeapon;
        Participant.onDeath += DestoyParticipant;
    }
    private void OnDisable()
    {
        BotMovement.onCanAttackWitchColdWeapon -= AttackWithColdWeapon;
        BotMovement.onCanAttackWitchExplosiveWeapon -= AttackWithExplosiveWeapon;
        Participant.onDeath -= DestoyParticipant;
    }
    public GameObject GetClosestEnemy()
    {
        return _closestEnemy;
    }
    private void FindClosestEnemy()
    {
        Participant[] activelParticipants = _allParticipants.GetParticipantsWithoutMe(gameObject).ToArray();
        if (activelParticipants.Length == 0)
        {
            return;
        }
        float distanceToClosetEnemy = Vector2.Distance(gameObject.transform.position, activelParticipants[0].gameObject.transform.position);
        _closestEnemy = activelParticipants[0].gameObject;
        foreach (var participant in activelParticipants)
        {
            float distance = Vector2.Distance(gameObject.transform.position, participant.gameObject.transform.position);
            if (distance < distanceToClosetEnemy)
            {
                distanceToClosetEnemy = distance;
                _closestEnemy = participant.gameObject;
            }
        }
    }
    private void AttackWithColdWeapon()
    {
        if(_movement.IsMyTurn())
        {
            _weaponSpawnPos = new Vector3(transform.position.x + 0.5f, transform.position.y, 0f);
            SelectWeapon<ColdWeapon>();  
        }
       
    }
    private void AttackWithExplosiveWeapon()
    {
        if (_movement.IsMyTurn())
        {
            _weaponSpawnPos = new Vector3(transform.position.x, transform.position.y + 0.5f, 0f);
            SelectWeapon<ExplosiveWeapon>();
        }
       
    }
    private void SelectWeapon<T>() where T : class
    {
        int num;
        while (true)
        {
            num = Random.Range(0, _allWeapons.Length);
            if (_allWeapons[num].TryGetComponent<T>(out T weaponType))
            {
                _selectedWeapon = _allWeapons[num];
                break;
            }
        }
        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        GameObject weapon = Instantiate(_selectedWeapon, _weaponSpawnPos, _selectedWeapon.transform.rotation, null);

        if(weapon.TryGetComponent<ExplosiveWeapon>(out ExplosiveWeapon explosiveWeapon))
        {
            float _distance = Vector2.Distance(transform.position, _closestEnemy.transform.position);
            Vector3 direction = (_closestEnemy.transform.position  - transform.position).normalized;
            Vector3 force = direction * (_distance * _pushForce);
            explosiveWeapon.Push(force);
        }
        else if(weapon.TryGetComponent<ColdWeapon>(out ColdWeapon coldWeapon))
        {
            coldWeapon.Attack();
        }
    }
    public void DestoyParticipant(Participant participant)
    {
        if (gameObject == participant.gameObject && participant.TryGetComponent<Bot>(out Bot bot))
        {
            _movement.onMyTurn -= FindClosestEnemy;
            BotMovement.onCanAttackWitchColdWeapon -= AttackWithColdWeapon;
            BotMovement.onCanAttackWitchExplosiveWeapon -= AttackWithExplosiveWeapon;
            Participant.onDeath -= DestoyParticipant;
            _allParticipants.DeleteParticipant(participant);
            _allParticipants.DecrementAllWalkNumbers(participant);
            Destroy(this.gameObject);

        }
        
    }
}
