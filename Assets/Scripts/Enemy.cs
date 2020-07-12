//Module: Enemy
//Version: 0.11

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage;
    public float speed;
    public Town target;
    public Spline2DComponent path;

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
}
