using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DrillState : Core
{
	public DrillingState drillingState;
	public SuckState suckState;
	public IdleActionState idleActionState;

	public PlayerController input;
	public Vector2 mousePosition;
	public Vector2 direction;

	private float angle;
	public GameObject firePoint;

	public GameObject toolSprite;
	public GameObject sawSprite;

	public bool primaryMouseInput { get; private set; }
	public bool secondaryMouseInput { get; private set; }
	private float _direction = 1f;
	public NoiseMaker NoiseMaker;
	public float DrillNoiseLevel;
	public float SuckNoiseLevel;

	void Awake() {
		HideTool();
	}

	void Start()
	{
		SetupInstances();
		machine.Set(idleActionState);
		NoiseMaker = GetComponent<NoiseMaker>();
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
		primaryMouseInput = input.RetrieveAttackInput();
		secondaryMouseInput = input.RetrieveSecondaryAction();

		angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		direction = (mousePosition - (Vector2)transform.position).normalized;
		toolSprite.transform.right = direction;

		Vector3 localScale = new Vector3(1f, 1f,1f);

		if (angle > 90 || angle < -90)
		{
			localScale.y = -1f;
		}
		else
		{
			localScale.y = 1f;
		}
		toolSprite.transform.localScale = localScale;
	}

	void SelectState()
	{
		if (primaryMouseInput == true)
		{
			if (machine.state == drillingState.isComplete)
			{
				machine.Set(drillingState, true);
			}
			else
			{
				machine.Set(drillingState);
			}
			NoiseMaker.NoiseLevel = DrillNoiseLevel;
			NoiseMaker.IsMakingNoise = true;
		}
		else if (secondaryMouseInput == true)
		{
			machine.Set(suckState);
			NoiseMaker.NoiseLevel = SuckNoiseLevel;
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
		ShowTool();
	}
	
	public override void ExitState()
	{
		machine.Set(idleActionState);
		isActive = false;
		HideTool();
	}

	// Hides the gun by disabling the SpriteRenderer
	void HideTool()
	{
		SpriteRenderer toolSR = toolSprite.GetComponent<SpriteRenderer>();
		SpriteRenderer sawSR = sawSprite.GetComponent<SpriteRenderer>();
		if (toolSprite != null)
		{
			toolSR.enabled = false; // Hides the gun sprite
			sawSR.enabled = false; // Hides the gun sprite
		}
	}


	// Shows the gun by enabling the SpriteRenderer
	void ShowTool()
	{
		SpriteRenderer toolSR = toolSprite.GetComponent<SpriteRenderer>();
		SpriteRenderer sawSR = sawSprite.GetComponent<SpriteRenderer>();
		if (toolSprite != null)
		{
			toolSR.enabled = true; // Shows the gun sprite
			sawSR.enabled = true; // Shows the gun sprite
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
