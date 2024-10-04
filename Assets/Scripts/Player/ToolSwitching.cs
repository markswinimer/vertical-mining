using UnityEditor.Tilemaps;
using UnityEngine;

public class ToolSwitching : MonoBehaviour
{
    public int selectedTool = 0;
    public PlayerController input;

    void Start()
    {
        SelectTool();
    }

    void Update()
    {
        // if pressed 1
        if (Input.GetKeyDown(KeyCode.Alpha1) && transform.childCount >= 1)
        {
            selectedTool = 0;
            SelectTool();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedTool = 1;
            SelectTool();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedTool = 2;
            SelectTool();
        }
    }


    void SelectTool()
    {
        int i = 0;
        // this will get the first tool under the toolholder object
        foreach (Transform tool in transform)
        {
            if (i == selectedTool)
            {
                PlayerAction.instance.SetActiveTool(tool.GetComponent<Core>());
            }
            i++;
        }
    }
}
