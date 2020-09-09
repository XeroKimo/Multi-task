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
    public int health { get; private set; }
    [SerializeField]
    private int m_maxHealth;

    public Spline2DRoadComponent path;
    public float distanceTraveled { get; private set; }

    public delegate void OnDeath(Enemy enemy);
    public event OnDeath onDeath;
    public event OnDeath onPathFinished;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    public void OnEnable()
    {
    }

    public void FixedUpdate()
    {
        Vector3 oldPos = transform.position;
        distanceTraveled += speed * Time.fixedDeltaTime;
        transform.position = path.InterpolateDistanceWorldSpace(distanceTraveled);

        Vector3 posDiff = transform.position - oldPos;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(posDiff.y, posDiff.x) / Mathf.PI * 180);


        if(distanceTraveled > path.Length)
        {
            onPathFinished?.Invoke(this);
            //Destroy(gameObject);    //For Debugging
            //gameObject.SetActive(false);

            EnemyPool.instance.Despawn(gameObject);
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            onDeath?.Invoke(this);
            //Destroy(gameObject);    //For Debugging
            EnemyPool.instance.Despawn(gameObject);
            //gameObject.SetActive(false);
        }
    }

    public void Reset(Spline2DRoadComponent path)
    {
        onDeath = null;
        onPathFinished = null;
        this.path = path;
        distanceTraveled = 0;
        transform.position = path.GetPointWorldSpace(0);
        health = m_maxHealth;
    }

    public float GetPercentDistanceTraveled()
    {
        return path.DistanceToLinearT(distanceTraveled);
    }

    public void ConfigureEnemy(Enemy baseEnemy)
    {
        animator.runtimeAnimatorController = baseEnemy.animator.runtimeAnimatorController;
        spriteRenderer.sprite = baseEnemy.spriteRenderer.sprite;

        damage = baseEnemy.damage;
        speed = baseEnemy.speed;
        money = baseEnemy.money;
        m_maxHealth = baseEnemy.m_maxHealth;
}
}
