using UnityEngine;

public class TileBreakView : MonoBehaviour
{
    public Sprite[] breakingSprites;
    private SpriteRenderer _spriteRenderer;
    private int _totalAnimationFrames;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _totalAnimationFrames = breakingSprites.Length;
    }

    public void UpdateAnimation(float durabilityPercentage)
    {
        int currentFrame = Mathf.FloorToInt((1f - durabilityPercentage) * _totalAnimationFrames);

        currentFrame = Mathf.Clamp(currentFrame, 0, _totalAnimationFrames - 1);

        _spriteRenderer.sprite = breakingSprites[currentFrame];
    }

    public void ResetAnimation()
    {
        _spriteRenderer.sprite = null;
    }
}
