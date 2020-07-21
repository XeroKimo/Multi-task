//Module: Enemy
//Version: 0.13

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage;
    public float speed;
    public int money;
    public int m_health { get; private set; }

    public Spline2DComponent path;
    public float distanceTraveled = 0;

    public delegate void OnDeath(Enemy enemy);
    public event OnDeath onDeath;
    public event OnDeath onPathFinished;


    public void OnEnable()
    {
        transform.position = path.GetPointWorldSpace(0);
        distanceTraveled = 0;
    }

    public void FixedUpdate()
    {
        distanceTraveled += speed * Time.fixedDeltaTime;
        transform.position = path.InterpolateDistanceWorldSpace(distanceTraveled);

        if(distanceTraveled > path.Length)
        {
            onPathFinished?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public void SetHealth(int health)
    {
        Debug.Assert(health > 0, "Setting enemy health must be greater than 0");
        m_health = health;
    }

    public void ApplyDamage(int damage)
    {
        m_health -= damage;
        if(m_health < 0)
        {
            onDeath?.Invoke(this);
        }
    }
}
