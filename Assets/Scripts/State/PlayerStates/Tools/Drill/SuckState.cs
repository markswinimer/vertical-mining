using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SuckState : State
{

    public Transform firePoint;

    public GameObject whirlwindEffect;
    private GameObject _spawnedVacuumEffect;
    public VacuumEffect _vacuumEffect;
    public SuckPoint _suckPoint;

    private float _actionDelay;
    private float _playerAttackSpeed;

    public AudioSource pickaxeHitSound;

    public float vacuumForce = 5f;

    bool _isSucking = false;

    public override void Enter()
    {
        _playerAttackSpeed = Player.Instance.AttackSpeed;
        _actionDelay = 0;
        StartSuck();
    }

    public override void Do()
    {
        // _actionDelay -= Time.deltaTime;

        if (_isSucking == false)
        {
            // StartSuck();
        }
    }

    void StartSuck()
    {
        _isSucking = true;
        _vacuumEffect.StartVacuum();
        _suckPoint.StartVacuum();
        _spawnedVacuumEffect = Instantiate(whirlwindEffect, firePoint.position, firePoint.rotation, firePoint);
    }

    public override void Exit()
    {
        _vacuumEffect.StopVacuum();
        _suckPoint.StopVacuum();
        StopSuck();
    }

    void StopSuck()
    {
        _isSucking = false;
        Destroy(_spawnedVacuumEffect);
    }

    // When an object enters the vacuum's range
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isSucking)
        {
            // Check if the object is something you want to suck up
            if (other.CompareTag("Suckable"))  // Tag objects to be sucked up with "Suckable"
            {
                // Pull the object towards the vacuum center (usually the player)
                Vector2 direction = (transform.position - other.transform.position).normalized;
                other.GetComponent<Rigidbody2D>().AddForce(direction * vacuumForce);
            }
        }
    }

    // Activate or deactivate the vacuum when right-click is held or released
    void Update()
    {
        if (Input.GetMouseButton(1))  // Right-click held
        {
            _isSucking = true;
        }
        else
        {
            _isSucking = false;
        }
    }

    // pickaxeHitSound.Stop();
    // pickaxeHitSound.time = Random.Range(0.3f, .4f);
    // pickaxeHitSound.Play(0);
}
