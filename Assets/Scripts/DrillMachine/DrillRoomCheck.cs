using UnityEngine;

public class DrillRoomCheck : MonoBehaviour
{
    public SpriteMask drillMask;

    // The room inside the drill  
    public GameObject roomObject; 

    public SuckResources suckResources;
    
    public bool playerInDrillRoom;

    private float _timeInsideDrillRoom = 0f;
    private float _timeToTriggerSuck = 1.5f;

    void Start()
    {
        drillMask.enabled = false;
        playerInDrillRoom = false;
    }

    void Update()
    {
        if (playerInDrillRoom)
        {
            _timeInsideDrillRoom += Time.deltaTime;
            if (_timeInsideDrillRoom > _timeToTriggerSuck)
            {
                suckResources.TurnSuckOn();
            }
        }
        else
        {
            _timeInsideDrillRoom = 0f;
            suckResources.TurnSuckOff();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);

        if (other.CompareTag("Player"))
        {
            playerInDrillRoom = true;
            drillMask.enabled = true;

            roomObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            playerInDrillRoom = false;
            drillMask.enabled = false;

            roomObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
