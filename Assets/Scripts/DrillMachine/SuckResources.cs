using UnityEngine;
using System.Collections;

public class SuckResources : MonoBehaviour
{
    private bool _playerInDrillRoom;
    private Inventory _playerInventory;

    public GameObject _suckPoint;

    private int _playerInventoryCount = 0;

    private float _timeBetweenItemSucks = 0.5f;

    public float floatForce = 1f;  // The gentle force applied upward
    public float lifetime = 1.5f;  // How long the icon stays before being destroyed
    public float suctionForce = 5f;  // The force pulling the item towards the suction point
    public float suctionDelay = 0.5f;  // Delay before the suction starts


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
        _timeBetweenItemSucks -= Time.deltaTime;
        
        if (_playerInDrillRoom && _timeBetweenItemSucks <= 0)
        {
            ItemObject item = _playerInventory.TryRemoveAndGetItem(ItemType.Ore);
            
            if (item != null)
            {
                _timeBetweenItemSucks = 0.5f;
                PullItemFromInventory(item);
            }
        }
    }

    private void PullItemFromInventory(ItemObject item)
    {
        // Instantiate rockPrefab at the player's center position
        GameObject rock = Instantiate(item.Prefab, Player.Instance.transform.position, Quaternion.identity);

        // Get the Rigidbody2D component
        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Disable gravity for this Rigidbody2D
            rb.gravityScale = 0;

            // Apply a small upward velocity to make it float
            rb.velocity = new Vector2(0, floatForce);

            // Start the suction effect after a delay
            StartCoroutine(SuckUpAfterDelay(rb, _suckPoint.transform.position, suctionDelay));
        }

        // Destroy the rock after a certain time
        Destroy(rock, lifetime);
    }

    private IEnumerator SuckUpAfterDelay(Rigidbody2D rb, Vector3 suctionPoint, float delay)
    {
        // Wait for the delay before suction starts
        yield return new WaitForSeconds(delay);

        while (rb != null)
        {
            // Calculate direction to the suction point
            Vector2 direction = (suctionPoint - rb.transform.position).normalized;

            // Apply force towards the suction point
            rb.AddForce(direction * suctionForce);

            yield return null;
        }
    }
}
