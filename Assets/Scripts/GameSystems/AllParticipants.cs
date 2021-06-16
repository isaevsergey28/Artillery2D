using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllParticipants : MonoBehaviour
{
    [SerializeField] private List<Participant> _activeParticipants = new List<Participant>();

    public delegate void OnLastParticipant(Participant participant);
    public static OnLastParticipant onLastParticipant;

    private void Start()
    {
         Participant[] _participants;
         _participants = GameObject.FindObjectsOfType<Participant>();
        
        foreach (var participant in _participants)
        {
            _activeParticipants.Add(participant);
        }
    }
    public List<Participant> GetParticipants()
    {
        return _activeParticipants;
    }
    public List<Participant> GetParticipantsWithoutMe(GameObject participant)
    {
        List<Participant> tempParticipants = new List<Participant>();

        foreach(var _participant in _activeParticipants)
        {
            if(_participant != participant.GetComponent<Participant>())
            {
                tempParticipants.Add(_participant);
            }
        }
        return tempParticipants;
    }
    public void DeleteParticipant(Participant participant)
    {
        _activeParticipants.Remove(participant);
        if(_activeParticipants.Count == 1)
        {
            onLastParticipant?.Invoke(_activeParticipants[0]);
        }
    }
    public void DecrementAllWalkNumbers(Participant _participant)
    {
        foreach (var participant in _activeParticipants)
        {
            if(participant.GetWalkNumber() > _participant.GetWalkNumber())
            {
                participant.DecrementWalkNumber();
            }
        }
    }
}
