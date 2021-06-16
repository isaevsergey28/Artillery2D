using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Participant
{
    public delegate void OnPlayerWalks(bool flag);
    public static event OnPlayerWalks onPlayerWalks;

    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;

    private void Start()
    {
        GameRound.onRoundStart += IsPlayerWalks;
        Participant.onDeath += DestroyPlayer;
        
    }
    private void OnDisable()
    {
        GameRound.onRoundStart -= IsPlayerWalks;
        Participant.onDeath -= DestroyPlayer;
    }
    private void IsPlayerWalks(int round)
    {
        if(round == _walkNumber)
        {
            onPlayerWalks?.Invoke(true);
        }
        else
        {
            onPlayerWalks?.Invoke(false);
        }
    }
    private void DestroyPlayer(Participant participant)
    {
        if(participant.TryGetComponent<Player>(out Player player))
        {
            participant.GiveDamage(-50);
            onPlayerDeath?.Invoke();
        }
    }
}
