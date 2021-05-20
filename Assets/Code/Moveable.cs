using UnityEngine;

public class Moveable : MonoBehaviour
{
    protected Rigidbody2D m_rigidbody;

    private void Awake() => m_rigidbody = GetComponent<Rigidbody2D>();

    Vector2 m_direction;

    public virtual bool MoveTo(Vector2 direction)
    {
        if(!CanMove(direction))
            return false;

        Move(direction);

        Movement movement = new Movement(direction, this);
        MoveableManager.RegisterMovement(movement);

        return true;
    }

    private void Move(Vector2 direction) =>
        m_direction = direction + new Vector2(transform.position.x, transform.position.y);

    private void FixedUpdate()
    {
        if(m_direction != Vector2.zero)
        {
            m_rigidbody.MovePosition(m_direction);
            m_direction = Vector2.zero;
        }
    }

    protected virtual bool CanMove(Vector2 direction) { return false; }
    public virtual void MoveBack(Vector2 direction) => Move(-direction);
}