using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameRound : MonoBehaviour
{
    [SerializeField] private GameObject _roundTextObject;
    [SerializeField] private int _roundTime;
    [SerializeField] private GameObject _startPanel;
    
    public delegate void OnChangeState(bool flag);
    public static event OnChangeState onChangeState;
    
    private Text _roundText;
    private RoundState _roundState;
    private float _seconds;
    private void Start()
    {
        _roundText = _roundTextObject.GetComponent<Text>();
        _roundState = RoundState.Pause;
        Projectile.onProjectileDestroy += StopRound; 
    }
    public void StartRound()
    {
        _startPanel.SetActive(false);
        _roundState = RoundState.Start;
        _roundText.text = _roundTime.ToString();
        onChangeState?.Invoke(false);
    }

    private void Update()
    {
        if (_roundState == RoundState.Start)
        {
            _seconds += Time.deltaTime;
            _seconds = Mathf.Clamp(_seconds, 0, _roundTime);
            if ((int) _seconds == _roundTime)
            {
                StopRound();
            }
            _roundText.text = (_roundTime - ((int) _seconds)).ToString();
        }
    }

    private void StopRound()
    {
        _roundState = RoundState.End;
        StartCoroutine(SetPause());
        onChangeState?.Invoke(true);
    }

    private IEnumerator SetPause()
    {
        yield return new WaitForSeconds(3f);
        _seconds = 0f;
        _roundState = RoundState.Pause;
        _startPanel.SetActive(true);
    }
}
public enum RoundState
{
    Pause = 0,
    Start = 1,
    End = 2,
}