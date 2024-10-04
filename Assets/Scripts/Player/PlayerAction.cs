using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public static PlayerAction instance;
    public Core activeTool;

    void Start()
    {
        instance = this;
    }

    public void SetActiveTool(Core tool)
    {
        if (activeTool == tool)
        {
            return;
        }
        else
        {
            activeTool.isActive = false;
            tool.isActive = true;
            activeTool = tool;
        }
    }
}
