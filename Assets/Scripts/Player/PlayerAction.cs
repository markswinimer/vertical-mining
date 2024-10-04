using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public static PlayerAction instance;
    public Core activeTool;

    void Awake() {
        instance = this;
        activeTool.EnterState();
    }

    public void SetActiveTool(Core tool)
    {
        if (activeTool == tool)
        {
            return;
        }
        else
        {
            activeTool.ExitState();
            tool.EnterState();
            activeTool = tool;
        }
    }
}
