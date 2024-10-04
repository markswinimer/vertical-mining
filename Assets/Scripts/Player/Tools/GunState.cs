using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GunState : Core
{
    public SwingState swingState;
    public IdleActionState idleActionState;
    public PlayerController input;

    public bool mouseInput { get; private set; }
    private float _direction = 1f;

    void Start()
    {
        SetupInstances();
        machine.Set(idleActionState);
    }

    void Update()
    {
        if (isActive)
        {
            CheckInput();
            SelectState();
        }
    }

    void FixedUpdate()
    {
        FaceCusor();
    }

    void CheckInput()
    {
        mouseInput = input.RetrieveAttackInput();
    }

    void SelectState()
    {
        if (mouseInput == true || swingState.IsSwinging)
        {
            Debug.Log("Mouse Input is true --- GUN");
            if (machine.state == swingState.isComplete)
            {
                machine.Set(swingState, true);
            }
            else
            {
                machine.Set(swingState);
            }
        }
        else
        {
            animator.speed = 1f;
            machine.Set(idleActionState);
        }
        machine.state.Do();
    }

    void FaceCusor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            _direction = -1f;
        }
        else
        {
            _direction = 1f;
        }
        transform.localScale = new Vector3(_direction, 1, 1);
    }
    
    public override void ExitState()
    {
        machine.Set(idleActionState);
        isActive = false;
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (Application.isPlaying && state != null)
        {
            List<State> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), "Active States: " + string.Join(", ", states));
        }
#endif
    }
}
