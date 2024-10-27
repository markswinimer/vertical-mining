using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PistolReloadState : State
{
    public event Action OnReload;

    private float _reloadSpeed;
    private float _timeToReload;

    public AudioSource reloadSound;
    [SerializeField] private Pistol _pistol;

    public override void Enter()
    {
        OnReload?.Invoke();
        _reloadSpeed = _pistol.ReloadSpeed;
        _timeToReload = _reloadSpeed;
    }

    public override void Do()
    {
        _timeToReload -= Time.deltaTime;

        if (_timeToReload <= 0f)
        {
            Debug.Log("Reloading complete");
            _pistol.FillAmmo();
            isComplete = true;
        }
    }
    // reloadSound.Stop();
    // reloadSound.time = Random.Range(0.3f, .4f);
    // reloadSound.Play(0);
}
