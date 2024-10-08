using Unity.VisualScripting;
using UnityEngine;

// add new movement commands to this file from inputActions menu in editor
// then build out the functionality in capabilities if possible
// or add them to an existing capability

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
public class PlayerController : InputController
{
    private PlayerInputActions _inputActions;
    private bool _isJumping;
    private bool _isAttacking;
    private bool _isSecondaryAction;

    void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Jump.started += JumpStarted;
        _inputActions.Player.Jump.canceled += JumpCanceled;
        _inputActions.Player.Attack.started += AttackStarted;
        _inputActions.Player.Attack.canceled += AttackCanceled;
        _inputActions.Player.SecondaryAction.started += SecondaryActionStarted;
        _inputActions.Player.SecondaryAction.canceled += SecondaryActionCanceled;
    }

    void OnDisable()
    {
        _inputActions.Player.Disable();
        _inputActions.Player.Jump.started -= JumpStarted;
        _inputActions.Player.Jump.canceled -= JumpCanceled;
        _inputActions.Player.Attack.started -= AttackStarted;
        _inputActions.Player.Attack.canceled -= AttackCanceled;
        _inputActions.Player.SecondaryAction.started -= SecondaryActionStarted;
        _inputActions.Player.SecondaryAction.canceled -= SecondaryActionCanceled;
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

    void AttackStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isAttacking = true;
    }
    void AttackCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isAttacking = false;
    }

    void SecondaryActionStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isSecondaryAction = true;
    }
    void SecondaryActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isSecondaryAction = false;
    }

    public override bool RetrieveAttackInput()
    {
        return _isAttacking;
    }

    public override bool RetrieveSecondaryAction()
    {
        return _isSecondaryAction;
    }
    
    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return _isJumping;
    }

    public override float RetrieveMoveInput(GameObject gameObject)
    {
        return _inputActions.Player.Move.ReadValue<Vector2>().x;
    }
}
