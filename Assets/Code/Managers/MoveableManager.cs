using System.Collections.Generic;
using UnityEngine;

public struct Movement
{
    public Vector2 m_direction;
    public Moveable m_moveable;

    public Movement(Vector2 direction, Moveable moveable)
    {
        m_direction = direction;
        m_moveable = moveable;
    }
}

public class MoveableManager : MonoBehaviour
{
    static List<Movement> m_movements;

    private void Start() => m_movements = new List<Movement>();

    public static void RegisterMovement(Movement movement) => m_movements.Add(movement);

    public static void GoBack()
    {
        Movement currentMovement = m_movements[m_movements.Count - 1];

        if(m_movements.Count > 1)
        {
            Movement previousMovement = m_movements[m_movements.Count - 2];

            if(previousMovement.m_moveable is Box)
            {
                previousMovement.m_moveable.MoveBack(previousMovement.m_direction);
                m_movements.RemoveAt(m_movements.Count - 2);
            }
        }

        currentMovement.m_moveable.MoveBack(currentMovement.m_direction);
        m_movements.RemoveAt(m_movements.Count - 1);
    }

    public static void Restart() => m_movements = new List<Movement>();
}