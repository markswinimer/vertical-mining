using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool OnGround { get; private set; }

    public bool OnWall { get; private set; }
    
    public float Friction { get; private set; }

    public Vector2 ContactNormal { get; private set; }
    private PhysicsMaterial2D _material;

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        OnWall = false;
        Friction = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    public void EvaluateCollision(Collision2D collision)
    {
        OnGround = false;
        OnWall = false;

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 contactNormal = collision.GetContact(i).normal;

            // detection to be mostly vertical (i.e., y is very high)
            if (contactNormal.y > 0.99f && Mathf.Abs(contactNormal.x) < 0.1f)
            {
                OnGround = true;
            }

            // checked should be more horizontal
            if (Mathf.Abs(contactNormal.x) > 0.9f && Mathf.Abs(contactNormal.y) < 0.1f)
            {
                OnWall = true;
            }
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        _material = collision.rigidbody.sharedMaterial;

        Friction = 0;

        if(_material != null)
        {
            Friction = _material.friction;
        }
    }
}
