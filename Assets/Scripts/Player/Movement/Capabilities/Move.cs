using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

    private float _activeMaxSpeed;
    
    private Controller _controller;
    private Vector2 _direction, _desiredVelocity, _velocity;
    private Rigidbody2D _body;
    private Ground _ground;

    private float _maxSpeedChange, _acceleration;
    private bool _onGround;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _controller = GetComponent<Controller>();
    }

    private void Start()
    {
        _activeMaxSpeed = _maxSpeed;
    }

    private void Update()
    {
        _direction.x = _controller.input.RetrieveMoveInput(this.gameObject);
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_activeMaxSpeed - _ground.Friction, 0f);
    }

    private void FixedUpdate()
    {
        _onGround = _ground.OnGround;
        _velocity = _body.velocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        _body.velocity = _velocity;
    }

    public void ModifyMoveSpeed(float speedAdjustment)
    {
        _activeMaxSpeed = _maxSpeed * speedAdjustment;
    }

    public void ResetMoveSpeed()
    {
        _activeMaxSpeed = _maxSpeed;
    }
}