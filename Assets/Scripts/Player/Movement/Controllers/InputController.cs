using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput(GameObject gameObject);
    public abstract bool RetrieveJumpInput(GameObject gameObject);
    public abstract bool RetrieveAttackInput();
    public abstract bool RetrieveSecondaryAction();
}
