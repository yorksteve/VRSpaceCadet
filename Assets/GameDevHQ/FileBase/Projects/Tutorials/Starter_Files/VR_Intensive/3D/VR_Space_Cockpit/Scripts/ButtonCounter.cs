using Scripts.Interactables;
using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCounter : MonoBehaviour
{
    private int _buttonsClicked;
    private bool _sequenceComplete;

    public static Action onBootupComplete;
    public static Action onNextAudio;
    public static Action onReactorReset;

    private void OnEnable()
    {
        BootupButtons.onClick += Counter;
        GameSceneManager.onCheckBootupButtonStatus += ButtonCheck;
        ReactorButtons.onReactorClick += ReactorCounter;
    }

    private void Start()
    {
        _buttonsClicked = 0;
    }

    private void Counter()
    {
        _buttonsClicked++;

        if (_buttonsClicked >= 16)
        {
            onBootupComplete?.Invoke();
            _sequenceComplete = true;
            _buttonsClicked = 0;
        }
    }

    private void ReactorCounter()
    {
        _buttonsClicked++;

        if (_buttonsClicked == 4)
        {
            onReactorReset?.Invoke();
        }
    }

    private void ButtonCheck()
    {
        if (_buttonsClicked <= 8 && _sequenceComplete == false)
        {
            onNextAudio?.Invoke();
        }
    }

    private void OnDisable()
    {
        BootupButtons.onClick -= Counter;
        GameSceneManager.onCheckBootupButtonStatus -= ButtonCheck;
        ReactorButtons.onReactorClick -= ReactorCounter;
    }
}
