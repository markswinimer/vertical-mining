using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillTargeter : MonoBehaviour
{
    // get all of the game objects that are children in this drill
    public List<GameObject> drillPoints = new List<GameObject>();

    public void Start()
    {
        foreach (Transform child in transform)
        {
            drillPoints.Add(child.gameObject);
            // add a visual representation of the drill points with gizmos

        }
    }

    public List<Vector3Int> GetDrillTargetTiles()
    {
        // loop through all of the drill points, and check if they have a tile on them
        // then add that tile to a list and return it
        // create a list to store all of the tilepositions
        List<Vector3Int> targetTiles = new List<Vector3Int>();

        foreach (GameObject drillPoint in drillPoints)
        {
            Vector3Int gridPosition = TileManager.Instance.GetTilemapWorldToCell(drillPoint.transform.position);
            if (TileManager.Instance.IsTileValid(gridPosition))
            {
                targetTiles.Add(gridPosition);
            }
        }

        return targetTiles.Count > 0 ? targetTiles : null;
    }


    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        // draw a gizmo for each drill point
        foreach (GameObject drillPoint in drillPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(drillPoint.transform.position, 0.1f);
        }
        #endif
    }
}