using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleFlyingState : State
{
    public AnimationClip clip;

    public float rotationSpeed = 200f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private Vector3 startPosition;

    private Rigidbody2D _rb;
    public UrnEnemy _urnEnemy;

    public override void Enter()
    {
        isComplete = false;

        animator.Play(clip.name);
        _rb = _urnEnemy.GetComponent<Rigidbody2D>();
        startPosition = _urnEnemy.transform.position;

        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }
    }

    public override void Do()
    {
        if (_rb != null)
        {
            _rb.MoveRotation(_rb.rotation + rotationSpeed * Time.deltaTime);
        }
        else
        {
            _urnEnemy.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        _urnEnemy.transform.position = new Vector3(_urnEnemy.transform.position.x, newY, _urnEnemy.transform.position.z);
    }
}