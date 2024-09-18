using UnityEngine;

public class TilemapBlockBreaker : MonoBehaviour
{
    private TileData _tileData;
    public Sprite[] breakingSprites;
    private SpriteRenderer _spriteRenderer;
    private int _totalAnimationFrames;

    private float _timeSinceLastDamage = 0f;
    private float _regenDelay = 2f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _totalAnimationFrames = breakingSprites.Length;
    }
    
    public void Instantiate(TileData tileData)
    {
        _tileData = tileData;
    }

    void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;

        if (_timeSinceLastDamage >= _regenDelay)
        {
            _tileData.RegenerateDurability();
        }
    }

    public void UpdateAnimation(float durabilityPercentage)
    {
        int currentFrame = Mathf.FloorToInt((1f - durabilityPercentage) * _totalAnimationFrames);

        currentFrame = Mathf.Clamp(currentFrame, 0, _totalAnimationFrames - 1);

        _spriteRenderer.sprite = breakingSprites[currentFrame];

        _timeSinceLastDamage = 0f;
    }

    public void DestroyHandler()
    {
        Destroy(gameObject); // Destroy the GameObject
    }
}
