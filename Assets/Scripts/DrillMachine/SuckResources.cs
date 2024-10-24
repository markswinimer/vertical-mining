using UnityEngine;

public class SuckResources : MonoBehaviour
{
    private bool _playerInDrillRoom;
    private Inventory _playerInventory;

    private GameObject _suckPoint;

    private int _playerInventoryCount = 0;

    private float forceMin = 1f;
    private float forceMax = 2f;
    private float torqueMin = -20f;
    private float torqueMax = 20f;

    void Start()
    {
        _playerInventory = Player.Instance.Inventory;
        _playerInDrillRoom = false;
        _playerInventoryCount = _playerInventory.GetTotalInventoryCount();
    }

    public void TurnSuckOn() 
    {
        _playerInDrillRoom = true;
    }
    
    public void TurnSuckOff()
    {
        _playerInDrillRoom = false;
    }

    public void Update()
    {
        if (_playerInDrillRoom)
        {   
            ItemObject item = _playerInventory.TryRemoveAndGetItem(ItemType.Ore);
            
            if (item != null)
            {
                PullItemFromInventory(item);
            }
        }
    }

    private void PullItemFromInventory(ItemObject item)
    {
        // Define variables at the start of the function
        Vector2 forceDirection = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.01f, 0.01f)).normalized;  // Smaller range for less movement
        float forceMagnitude = Random.Range(0.0001f, 0.0005f);  // Tiny force for slight movement
        float torque = Random.Range(-0.001f, 0.001f);  // Small torque for gentle rotation

        // Instantiate rockPrefab at the player's center position
        GameObject rock = Instantiate(item.Prefab, Player.Instance.transform.position, Quaternion.identity);

        Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();

        // Disable gravity to allow floating
        rockRb.gravityScale = 0f;

        // Apply the pre-assigned force to the rock's Rigidbody2D
        rockRb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);

        // Apply the pre-assigned torque to the rock's Rigidbody2D
        rockRb.AddTorque(torque);

        // Clamp the velocity to limit how far the rock can move
        rockRb.velocity = Vector2.ClampMagnitude(rockRb.velocity, 0.005f);  // Limit the movement speed to a few pixels per second
    }
}
