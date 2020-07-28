//Module: Wave System
//Version: 0.1

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    [SerializeField]
    int m_health;

    public Spline2DComponent[] paths;

    public delegate void OnDeath(Town town);
    public event OnDeath onDeathEvent;

    public void ApplyDamage(int damage)
    {
        m_health -= damage;
        if(m_health < 0)
        {
            m_health = 0;
            onDeathEvent.Invoke(this);
        }
    }

    public bool IsAlive() { return m_health > 0; }
}
