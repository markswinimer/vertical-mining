using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]
public class AIController : InputController
{
    [Header("Interaction")]
    [SerializeField] private LayerMask _layerMask = -1;
    [Header("Ray")]
    [SerializeField] private float _bottomDistance = 1f;
    [SerializeField] private float _topDistance = 1f;
    [SerializeField] private float _xOffset = 1f;

    private RaycastHit2D _groundInfoBottom;
    private RaycastHit2D _groundInfoTop;

    // this file can be used to create enemy controllers
    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return false;
    }

    public override float RetrieveMoveInput(GameObject gameObject)
    {
        _groundInfoBottom = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x), gameObject.transform.position.y), Vector2.down, _bottomDistance, _layerMask);

        Debug.DrawRay(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x), gameObject.transform.position.y), Vector2.down * _bottomDistance, Color.red);

        _groundInfoTop = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x), gameObject.transform.position.x), Vector2.right, _topDistance, _layerMask);

        Debug.DrawRay(new Vector2(gameObject.transform.position.x + (_xOffset * gameObject.transform.localScale.x), gameObject.transform.position.x), Vector2.right * _topDistance, Color.red);

        if (_groundInfoBottom.collider == true || _groundInfoTop.collider != false)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
        }

        return gameObject.transform.localScale.x;
    }
}