using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InfoIndicator : MonoBehaviour
{
    protected Camera _mainCamera;
    protected Text _nickname;
    protected Participant _participant;

    [Inject]
    private void Construct(Camera mainCamera)
    {
        _mainCamera = mainCamera;
    }
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = _mainCamera;
        _nickname = GetComponentInChildren<Text>();
        _participant = transform.parent.GetComponent<Participant>();
    }
}
