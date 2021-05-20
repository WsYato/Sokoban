using System;
using UnityEngine;

public class Box : Moveable
{
    [SerializeField] Sprite unSolvedColor;
    [SerializeField] Sprite SolvedColor;

    [HideInInspector] public bool m_solved;

    public static event Action OnSolve;

    SpriteRenderer m_sprite;

    private void Start()
    {
        LevelManager.Instance.RegisterBox(this);
        m_sprite = GetComponent<SpriteRenderer>();

        CheckIfSolved();
    }

    void CheckIfSolved()
    {
        if(!Physics2D.Raycast(transform.position + Vector3.right, Vector2.left, 1f, (1 << 10)))
            return;

        m_sprite.sprite = SolvedColor;
        m_solved = true;

        OnSolve?.Invoke();
    }

    protected override bool CanMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, (1 << 8) | (1 << 9) | (1 << 10));

        if(!hit)
        {
            if(m_solved)
            {
                m_sprite.sprite = unSolvedColor;
                m_solved = false;
            }

            return true;
        }

        if(hit.collider.gameObject.layer == 10)
        {
            m_sprite.sprite = SolvedColor;
            m_solved = true;

            OnSolve?.Invoke();

            return true;
        }

        return false;
    }

    public override void MoveBack(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction, 1f, (1 << 10));
        
        if(!hit)
        {
            if(m_solved)
            {
                m_sprite.sprite = unSolvedColor;
                m_solved = false;
            }
        }
        else
        {
            m_sprite.sprite = SolvedColor;
            m_solved = true;

            OnSolve?.Invoke();
        }

        base.MoveBack(direction);
    }
}