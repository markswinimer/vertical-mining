using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerManager : Core
{
    public AirState airState;
    public IdleState idleState;
    public RunState runState;

    public PlayerController input;

    [SerializeField] bool Grounded;

    public float xInput { get; private set; }
    private float _direction = 1f;

    void Start()
    {
        SetupInstances();
        machine.Set(idleState);
    }

    void Update()
    {
        CheckInput();
        SelectState();
    }

    void FixedUpdate()
    {
        FaceInput();
    }

    void CheckInput()
    {
        xInput = input.RetrieveMoveInput(this.gameObject);
        Debug.Log("xInput: " + xInput);
    }

    void SelectState()
    {
        Grounded = ground.OnGround; // visualize grounded
        if (ground.OnGround)
        {

            if (xInput == 0)
            {
                machine.Set(idleState);
            }
            else
            {
                machine.Set(runState);
            }
        }
        else
        {
            machine.Set(airState);
        }
        machine.state.Do();
    }

    void FaceInput()
    {        
        if (xInput < 0)
        {
            _direction = -1;
        } 
        else if (xInput > 0)
        {
            _direction = 1;
        }

        transform.localScale = new Vector3(_direction, 1, 1);
    }
}
