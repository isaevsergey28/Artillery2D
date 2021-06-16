using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartRoundTextController : MonoBehaviour
{
    [SerializeField] private GameObject _startRoundObject;

    private Text _startRoundText;
    private AllParticipants _allParticipants;
    private string _activePlayername;

    [Inject]
    private void Construct(AllParticipants allParticipants)
    {
        _allParticipants = allParticipants;
    }
    private void Start()
    {
        GameRound.onRoundStart += ShowText;
        _startRoundText = _startRoundObject.GetComponent<Text>();
       
    }
    private void OnDisable()
    {
        GameRound.onRoundStart -= ShowText;
    }
    private void ShowText(int roundNumber)
    {
        foreach(var participant in _allParticipants.GetParticipants())
        {
            if(participant.GetWalkNumber() == roundNumber)
            {
                _activePlayername = participant.gameObject.GetComponentInChildren<Text>().text;
                break;
            }
        }
        _startRoundText.text = "Сейчас ходит : " + _activePlayername;
         StartCoroutine(ShowStartRoundText());
    }

    private IEnumerator ShowStartRoundText()
    {
        _startRoundObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        _startRoundObject.SetActive(false);
    }
}
