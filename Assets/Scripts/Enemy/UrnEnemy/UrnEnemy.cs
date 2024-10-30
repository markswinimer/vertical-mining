using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UrnEnemy : Core, IDamageable
{
	public IdleFlyingState idleFlyingState;
	public IdleWanderState idleWanderState;
	public FlyingNavigateState flyingNavigateState;
	public UrnChaseAttackState urnChaseAttackState;
	public UrnDeathState urnDeathState;

    public bool Invincible { get; set; }
    private float _invincibleTime = 2f;
    private float _timeSpentInvincible { get; set; }

    public State StartingState;

    public Vector3 _spawnPosition;
    private bool _willDie = false;

    private Transform Player;

    private AudioSource audioSource;
    public float _health = 100;
    public AudioClip damageSound;

    private Rigidbody2D _rb;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        SetupInstances();

        Invincible = false;
        
        Player = FindFirstObjectByType<Player>().transform;

        _rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        _rb.velocity = Vector2.zero;
        _spawnPosition = transform.position;

        Set(StartingState);
    }

    void Update()
    {
        if (Invincible)
        {
            _timeSpentInvincible += Time.deltaTime;

            if (_timeSpentInvincible < _invincibleTime)
            {
                Invincible = false;
                _timeSpentInvincible = 0;
            }
        }

        // if were chasing but no target, go back to idle
        if (state.isComplete)
        {
            if (state == urnChaseAttackState)
            {
                Set(idleWanderState);
            }
            else if (state == urnDeathState)
            {
                _willDie = false;
                HandleDeath();
            }
        }

        if (_willDie)
        {
            Set(urnDeathState);
        }

        // if in idle or wander, look for target
        if (state == idleWanderState || state == idleFlyingState)
        {
            urnChaseAttackState.CheckForTarget();

            if (urnChaseAttackState.target != null)
            {
                Set(urnChaseAttackState, true);
            }
        }

        state.DoBranch();
    }

    void HandleDeath()
    {
        Destroy(gameObject);
    }
 
    void FixedUpdate()
    {
        state.FixedDoBranch();
    }

    public void SetInvincible()
    {
        Invincible = true;
        _timeSpentInvincible = 0;   
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("Taking damage: " + damage);
        _health -= damage;
        // PlayDamageSound();
        FlashRed();
        SetInvincible();

        if (_health <= 0)
        {
            _willDie = true;
        }
        else 
        {
            SetInvincible();
        }
    }

    public void TakeDamageWithForce(int damage, Vector2 force)
    {
        _health -= damage;
        // PlayDamageSound();
        FlashRed();
        SetInvincible();

        if (_health <= 0)
        {
            _willDie = true;
        }
        else
        {
            _rb.AddForce(force, ForceMode2D.Impulse);
            SetInvincible();
        }
    }


    void FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            Invoke("ResetColor", 0.1f); // Reset after 0.1 seconds
        }
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
		if (Application.isPlaying && state != null)
		{
			List<State> states = machine.GetActiveStateBranch();
			UnityEditor.Handles.Label(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), "Active States: " + string.Join(", ", states));
		}
#endif
    }
}