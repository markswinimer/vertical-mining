using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 0.2f;
	private PlayerInput _playerInput;
	private Rigidbody2D _rigidBody;
	// Start is called before the first frame update
	void Start()
	{
		_playerInput = GetComponent<PlayerInput>();
		_rigidBody = GetComponent<Rigidbody2D>();
		
		var actions = new PlayerActionAsset();
		actions.Player.Enable();
		actions.Player.Movement.performed += HandleMovement;
		actions.Player.Movement.canceled += StopMoving;
	}

	// Update is called once per frame
	void Update()
	{
	}
	
	private void HandleMovement(InputAction.CallbackContext context)
	{
		var input = context.ReadValue<Vector2>();
		_rigidBody.velocity = input * speed;
		
	}
	
	private void StopMoving(InputAction.CallbackContext context)
	{
		_rigidBody.velocity = new Vector2();
	}
}
