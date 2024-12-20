


using System.Collections.Generic;
using UnityEngine;

public class SuckPoint : MonoBehaviour
{
    private bool isVacuumActive = false;

    public void StartVacuum()
    {
        isVacuumActive = true;
    }

    public void StopVacuum()
    {
        isVacuumActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isVacuumActive)
        {
            if ( other.CompareTag("Suckable") )  // Tag objects to be sucked up with "Suckable"
            {
                ObtainableItem obtainableItem = other.GetComponent<ObtainableItem>();
                if (obtainableItem != null)
                {
                    Player.Instance.Inventory.AddItem(obtainableItem.Item, obtainableItem.Amount);
                    Destroy(other.gameObject);
                }
            }
        }
    }

}