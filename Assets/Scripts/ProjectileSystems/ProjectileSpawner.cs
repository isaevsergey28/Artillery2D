using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private float _longClickTimeLimit = 2f;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _currentProjectile;

    public delegate void OnProjectileSpawn(Projectile projectile);
    static public OnProjectileSpawn onProjectileSpawn;
    
    private Camera _mainCamera;
    private bool _isPausedGame = false;
    private void Start()
    {
        _mainCamera = Camera.main;
        GameRound.onChangeState += IsRoundChangeState;
    }

    private void Update()
    {
        if (!_isPausedGame)
        {
            CheckClick();
        }
    }

    private void CheckClick()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 _clickPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            float deltaX = _clickPosition.x - transform.position.x;
            if (Mathf.Abs(deltaX) < 0.2f)
            {
                StartCoroutine(CheckForLongClick());
            }
        }

    }

    private IEnumerator CheckForLongClick()
    {
        yield return new WaitForEndOfFrame();
        float count = 0f;
        while (count < _longClickTimeLimit)
        {
            if (Input.GetMouseButtonUp(0))
            {
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
        if (_currentProjectile == null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, 0f);
            _currentProjectile = Instantiate(_projectilePrefab, spawnPosition, _projectilePrefab.transform.rotation, null);
            onProjectileSpawn.Invoke(_currentProjectile.GetComponent<Projectile>());
        }
    }

    private void IsRoundChangeState(bool flag)
    {
        _isPausedGame = flag;
    }
}
