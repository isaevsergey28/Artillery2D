using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveWeaponSpawner : WeaponSpawner
{
    [SerializeField] private float _longClickTimeLimit = 2f;
    [SerializeField] private GameObject _currentExplosiveWeapon;

    public delegate void OnExplosiveWeaponSpawn(ExplosiveWeapon explosiveWeapon);
    static public OnExplosiveWeaponSpawn onExplosiveWeaponSpawn;
    
    private Camera _mainCamera;
    private bool _isPausedGame = false;
    

    private void Start()
    {
        _mainCamera = Camera.main;
        GameRound.onChangeState += IsRoundChangeState;
        WeaponsChoice.onExplosiveWeaponSelected += RememberChosenWeapon;
    }
    private void OnDisable()
    {
        GameRound.onChangeState -= IsRoundChangeState;
        WeaponsChoice.onExplosiveWeaponSelected -= RememberChosenWeapon;
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
        if (Input.GetMouseButton(0) && _selectedWeapon || Input.touchCount == 1 && _selectedWeapon)
        {
            //Vector2 _clickPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _clickPosition = _mainCamera.ScreenToWorldPoint(Input.touches[0].position);
            float deltaX = _clickPosition.x - transform.position.x;
            if (Mathf.Abs(deltaX) < 0.5f)
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
            if (Input.touches[0].phase == TouchPhase.Ended)//Input.GetMouseButtonUp(0) || 
            {
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
        if (_currentExplosiveWeapon == null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, 0f);
            _currentExplosiveWeapon = Instantiate(_selectedWeapon, spawnPosition, _selectedWeapon.transform.rotation, null);
            onExplosiveWeaponSpawn.Invoke(_currentExplosiveWeapon.GetComponent<ExplosiveWeapon>());
        }
    }
    private void RememberChosenWeapon(GameObject weapon)
    {
        _selectedWeapon = weapon;
    }
    private void IsRoundChangeState(bool flag)
    {
        _isPausedGame = flag;
    }
}
