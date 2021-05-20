using System;
using UnityEngine;

public class Player : Moveable
{
    [SerializeField] Sprite front;
    [SerializeField] Sprite back;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;

    public static event Action OnMove;
    public static bool LevelStarted;

    SpriteRenderer m_sprite;

    private void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_sprite.sprite = front;
    }

    private void Update() => HandleMovement();

    void HandleMovement()
    {
        if(!LevelStarted)
            return;

        if(Input.GetButtonDown("Up"))
            MoveTo(Vector2.up);
        if(Input.GetButtonDown("Left"))
            MoveTo(Vector2.left);
        if(Input.GetButtonDown("Down"))
            MoveTo(Vector2.down);
        if(Input.GetButtonDown("Right"))
            MoveTo(Vector2.right);
    }

    public override bool MoveTo(Vector2 direction)
    {
        if(!base.MoveTo(direction))
            return false;

        OnMove();
        SetCorrectSprite(direction);

        return true;
    }

    protected override bool CanMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, (1 << 8) | (1 << 9));

        if(!hit)
            return true;

        if(hit.collider.gameObject.layer == 9)
        {
            Box box = hit.collider.gameObject.GetComponent<Box>();

            if(box.MoveTo(direction))
                return true;
        }

        return false;
    }

    public override void MoveBack(Vector2 direction)
    {
        base.MoveBack(direction);
        SetCorrectSprite(direction);
    }

    void SetCorrectSprite(Vector2 direction)
    {
        if(direction == Vector2.up)
            m_sprite.sprite = back;
        else if(direction == Vector2.left)
            m_sprite.sprite = left;
        else if(direction == Vector2.right)
            m_sprite.sprite = right;
        else
            m_sprite.sprite = front;
    }
}