using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerDetector : MonoBehaviour
{
    public float m_towerRadius = 0;
    public int m_towerCollisionCounter = 0;

    private void Awake() {}

    private void FixedUpdate()
    {
        Collider2D[] cast = Physics2D.OverlapCircleAll(transform.position, m_towerRadius, 1 << LayerMask.NameToLayer("Unit Collider") | 1 << LayerMask.NameToLayer("Path Collider"));
        m_towerCollisionCounter = cast.Length;
    }
    
    public bool isTowerPlaceable()
    {
        return m_towerCollisionCounter == 0 && m_towerRadius != 0;
    }

    public void Enable(float unitSize)
    {
        m_towerRadius = unitSize;
    }

    public void Disable()
    {
        m_towerRadius = 0;
    }
}
