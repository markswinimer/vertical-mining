using System.Collections;
using UnityEngine;

public class TileInstance : MonoBehaviour
{
    private Vector3Int _position;

    public AudioSource rockBreakSound;

    public float _timeSinceLastDamage = 0f;

    public TileBreakView _tileBreakView;

    public int minDrops = 1;
    public int maxDrops = 4;

    private float forceMin = 3f;
    private float forceMax = 6f;
    private float torqueMin = -50f;
    private float torqueMax = 50f;

    public GameObject dropPrefab;

    public void Instantiate(Vector3Int position)
    {
        _position = position;
    }

    void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;

        if (_timeSinceLastDamage > 5f)
        {
            TileManager.Instance.RestoreTileDurability(this, _position);
        }
    }

    public void ProcessRestoreDurability(float durabilityPercentage)
    {
        _timeSinceLastDamage = 0f;
        _tileBreakView.UpdateAnimation(durabilityPercentage);
    }

    public void ProcessDamageTile(float damagePercentage)
    {
        _timeSinceLastDamage = 0f;
        PlayDamageTileSound();
        _tileBreakView.UpdateAnimation(damagePercentage);
    }

    public void ProcessDestroyTile()
    {
        Debug.Log("TileInstance.ProcessDestroyTile");
        PlayDestroyTileSound();
        _tileBreakView.ResetAnimation();
        SpawnDrops();

        // Start a coroutine to destroy the GameObject after the sound finishes
        StartCoroutine(DestroyAfterSound());
    }

    public void PlayDestroyTileSound()
    {
        rockBreakSound.Stop();
        rockBreakSound.time = Random.Range(.4f, .7f);
        rockBreakSound.Play(0);
    }

    // Coroutine that waits until the sound finishes, then destroys the GameObject
    private IEnumerator DestroyAfterSound()
    {
        // Wait until the sound has finished playing
        yield return new WaitForSeconds(rockBreakSound.clip.length - rockBreakSound.time);

        // Now destroy the GameObject
        Destroy(this.gameObject);
    }
    
    public void PlayDamageTileSound()
    {
        rockBreakSound.Stop();
        rockBreakSound.time = Random.Range(0.8f, 1.1f);
        rockBreakSound.Play(0);
    }

    void SpawnDrops()
    {
        // Choose a random number of rocks to spawn (between min and max)
        int rockCount = Random.Range(minDrops, maxDrops + 1);

        for (int i = 0; i < rockCount; i++)
        {
            // Instantiate rockPrefab at the tile's position
            GameObject rock = Instantiate(dropPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();

            // Apply a random force in an upward-biased direction
            Vector2 forceDirection = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f)).normalized; // More upward bias
            float forceMagnitude = Random.Range(forceMin, forceMax);
            rockRb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);

            // Apply a random torque (rotation force)
            float torque = Random.Range(torqueMin, torqueMax);
            rockRb.AddTorque(torque);
        }
    }
}