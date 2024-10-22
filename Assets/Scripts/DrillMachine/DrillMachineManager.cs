using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DrillMachineManager : Core
{
    public IdleDrillMachineState idleDrillMachineState;
    public PoweringUpDrillMachineState poweringUpDrillMachineState;
    public DrillingMachineState drillingMachineState;

    public bool drillHasPoweredUp;

    [SerializeField] bool Grounded;

    void Start()
    {
        drillHasPoweredUp = false;
        SetupInstances();
        machine.Set(idleDrillMachineState);
    }

    void Update()
    {
        SelectState();
    }

    void SelectState()
    {
        // eventually we check if the drill is in the air
        Grounded = ground.OnGround; // visualize grounded

        if (DrillMachine.Instance.isPowered)
        {
            if (machine.state == poweringUpDrillMachineState.isComplete)
            {
                drillHasPoweredUp = true;
                machine.Set(drillingMachineState);
            }
            else if (drillHasPoweredUp == false)
            {
                machine.Set(poweringUpDrillMachineState);
            }
            else
            {
                machine.Set(drillingMachineState);
            }
        }
        else
        {
            machine.Set(idleDrillMachineState);
        }

        machine.state.Do();
    }

    void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        if (Application.isPlaying && state != null)
        {
            List<State> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), "Active States: " + string.Join(", ", states));
        }
    #endif
    }
}
