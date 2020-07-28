//Module: Wave System
//Version: 0.1

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

    public Spline2DComponent path { get; private set; }
    public float distanceTraveled { get; private set; }

    public delegate void OnDeath(Enemy enemy);
    public event OnDeath onDeath;
    public event OnDeath onPathFinished;


    public void OnEnable()
    {
    }

    public void FixedUpdate()
    {
        distanceTraveled += speed * Time.fixedDeltaTime;
        transform.position = path.InterpolateDistanceWorldSpace(distanceTraveled);

        if(distanceTraveled > path.Length)
        {
            onPathFinished?.Invoke(this);
            Destroy(gameObject);    //For Debugging
            //gameObject.SetActive(false);
        }
    }

    public void ApplyDamage(int damage)
    {
        m_health -= damage;
        if(m_health < 0)
        {
            onDeath?.Invoke(this);
            Destroy(gameObject);    //For Debugging
            //gameObject.SetActive(false);
        }
    }

    public void Reset(Spline2DComponent path)
    {
        onDeath = null;
        onPathFinished = null;
        this.path = path;
        distanceTraveled = 0;
        transform.position = path.GetPointWorldSpace(0);
    }
}
