using System.Collections;
using UnityEngine;

public class TileInstance : MonoBehaviour
{
    private Vector3Int _position;

    public AudioSource rockBreakSound;

    public float _timeSinceLastDamage = 0f;

    public TileBreakView _tileBreakView;
    
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
}