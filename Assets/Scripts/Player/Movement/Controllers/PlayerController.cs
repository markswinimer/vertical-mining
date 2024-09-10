using UnityEngine;

// add new movement commands to this file from inputActions menu in editor
// then build out the functionality in capabilities if possible
// or add them to an existing capability

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
public class PlayerController : InputController
{
    private PlayerInputActions _inputActions;
    private bool _isJumping;

    void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Jump.started += JumpStarted;
        _inputActions.Player.Jump.canceled += JumpCanceled;
    }

    void OnDisable()
    {
        _inputActions.Player.Disable();
        _inputActions.Player.Jump.started -= JumpStarted;
        _inputActions.Player.Jump.canceled -= JumpCanceled;
        _inputActions = null;
    }

    void JumpStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isJumping = true;
    }

    void JumpCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isJumping = false;
    }

    public override bool RetrieveJumpInput()
    {
        return _isJumping;
    }

    public override float RetrieveMoveInput()
    {
        return _inputActions.Player.Move.ReadValue<Vector2>().x;
    }
}
