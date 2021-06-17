using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameRound : MonoBehaviour
{
    [SerializeField] private GameObject _roundTextObject;
    [SerializeField] private int _roundTime;
    [SerializeField] private GameObject _startPanel;
    
    public delegate void OnChangeState(bool flag);
    public static event OnChangeState onChangeState;

    public delegate void OnRoundStart(int roundNumber);
    public static event OnRoundStart onRoundStart;
    
    private Text _roundText;
    private RoundState _roundState;
    private float _seconds;
    private int _roundNumber = 0;
    private AllParticipants _allParticipants;

    private void Start()
    {
        _roundText = _roundTextObject.GetComponent<Text>();
        _roundState = RoundState.Pause;
        ExplosiveWeapon.onExplosiveWeaponDestroy += StopRound;
        ColdWeapon.onColdWeaponDestroy += StopRound;
        Participant.onDeath += NullifyRounds;
        _allParticipants = GameObject.FindObjectOfType<AllParticipants>();
    }
    private void OnDisable()
    {
        ExplosiveWeapon.onExplosiveWeaponDestroy -= StopRound;
        ColdWeapon.onColdWeaponDestroy -= StopRound;
    }
    public void StartRound()
    {
        _startPanel.SetActive(false);
        _roundState = RoundState.Start;
        _roundText.text = _roundTime.ToString();
        onChangeState?.Invoke(false);
        if (_roundNumber == _allParticipants.GetParticipants().Count)
        {
            _roundNumber = 0;
        }
        _roundNumber++;
        onRoundStart.Invoke(_roundNumber);
    }
    public int GetRoundNumber()
    {
        return _roundNumber;
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
    private void NullifyRounds(Participant participant)
    {
        _roundNumber = 0;
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