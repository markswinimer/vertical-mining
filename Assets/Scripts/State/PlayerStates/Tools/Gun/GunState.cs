using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GunState : Core
{
	public ShootState shootState;
	public IdleActionState idleActionState;
	public PlayerController input;
	public Vector2 mousePosition;
	public Vector2 direction;

	private float angle;
	public GameObject firePoint;

	public GameObject gunSprite;

	public bool mouseInput { get; private set; }
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
	}

	void Update()
	{
		if (isActive)
		{
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
		gunSprite.transform.right = direction;

		Vector3 localScale = new Vector3(1f, 1f, 1f);

		if (angle > 90 || angle < -90)
		{
			localScale.y = -1f;
		}
		else
		{
			localScale.y = 1f;
		}
		gunSprite.transform.localScale = localScale;
	}

	void SelectState()
	{
		if (mouseInput == true)
		{
			if (machine.state == shootState.isComplete)
			{
				machine.Set(shootState, true);
			}
			else
			{
				machine.Set(shootState);
			}
			NoiseMaker.IsMakingNoise = true;
		}
		else
		{
			machine.Set(idleActionState);
			NoiseMaker.IsMakingNoise = false;
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
		SpriteRenderer sr = gunSprite.GetComponent<SpriteRenderer>();
		if (gunSprite != null)
		{
			sr.enabled = false; // Hides the gun sprite
		}
	}


	// Shows the gun by enabling the SpriteRenderer
	void ShowGun()
	{
		SpriteRenderer sr = gunSprite.GetComponent<SpriteRenderer>();
		if (gunSprite != null)
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