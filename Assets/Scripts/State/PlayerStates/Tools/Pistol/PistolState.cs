using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PistolState : Core
{
	public PistolReloadState pistolReloadState;
	public PistolShootState pistolShootState;
	public IdleActionState idleActionState;
	public PlayerController input;

	[SerializeField] private Pistol pistol;
	
	public bool mouseInput { get; private set; }
	public Vector2 mousePosition;
	public Vector2 direction;

	private float angle;

	public GameObject firePoint;
	public GameObject weaponSprite;

	private float _shootDelay;
	private float _timeToShoot;

	private float _direction = 1f;
	public NoiseMaker NoiseMaker;
	public float GunNoiseLevel;

	void Awake()
	{
		HideGun();
	}
 
	void Start()
	{
		SetupInstances();
		machine.Set(idleActionState);
		NoiseMaker = GetComponent<NoiseMaker>();
		NoiseMaker.NoiseLevel = GunNoiseLevel;

		_shootDelay = pistol.AttackDelay;
		_timeToShoot = 0f;
	}

	void Update()
	{
		if (isActive)
		{
			_timeToShoot -= Time.deltaTime;
			CheckInput();
			SelectState();
		}
	}

	void CheckInput()
	{
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseInput = input.RetrieveAttackInput();

		angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		direction = (mousePosition - (Vector2)transform.position).normalized;
		weaponSprite.transform.right = direction;

		Vector3 localScale = new Vector3(1f, 1f, 1f);

		if (angle > 90 || angle < -90)
		{
			localScale.y = -1f;
		}
		else
		{
			localScale.y = 1f;
		}
		weaponSprite.transform.localScale = localScale;
	}

	void SelectState()
	{
		// Always reload if ammo is empty
		// Shoot if has ammo and mouse input is true
		// Shoot if input is held down and other conditions are met
		if (pistol.CurrentAmmo == 0)
		{
			Debug.Log("Reloading");
			machine.Set(pistolReloadState);
		}
		else
		{
			if (mouseInput == true && _timeToShoot <= 0f)
			{
				if (machine.state == pistolShootState.isComplete)
				{
					Debug.Log("have hsot");
					_timeToShoot = _shootDelay;
					machine.Set(pistolShootState, true);
				}
				else
				{
					Debug.Log("shooting");
					_timeToShoot = _shootDelay;
					machine.Set(pistolShootState);
				}
				NoiseMaker.IsMakingNoise = true;
			}
			else
			{
				Debug.Log("idle");
				machine.Set(idleActionState);
				NoiseMaker.IsMakingNoise = false;
			}
		}

		machine.state.Do();
	}

	public override void EnterState()
	{
		isActive = true;
		ShowGun();
	}

	public override void ExitState()
	{
		machine.Set(idleActionState);
		isActive = false;
		HideGun();
	}

	// Hides the gun by disabling the SpriteRenderer
	void HideGun()
	{
		SpriteRenderer sr = weaponSprite.GetComponent<SpriteRenderer>();
		if (weaponSprite != null)
		{
			sr.enabled = false; // Hides the gun sprite
		}
	}


	// Shows the gun by enabling the SpriteRenderer
	void ShowGun()
	{
		SpriteRenderer sr = weaponSprite.GetComponent<SpriteRenderer>();
		if (weaponSprite != null)
		{
			sr.enabled = true; // Shows the gun sprite
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