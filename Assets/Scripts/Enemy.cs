//Module: Enemy
//Version: 0.11

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage;
    public float health;
    public float speed;
    public Town target;
    public Spline2DComponent path;

    public delegate void OnDeath(Enemy enemy);
    public event OnDeath onDeathEvent;

    public float distanceTraveled = 0;

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
            target.ApplyDamage(damage);
            gameObject.SetActive(false);
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
            onDeathEvent?.Invoke(this);
        }
    }
}
