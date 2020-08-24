using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.IO;
//Module: Core Module
//Version: 0.1

public enum TargetScheme
{
    First,
    Last,
    Closest,
    Strongest
}

public class Tower : MonoBehaviour
{
    private delegate void TargetFunction(Collider2D other);

    private TargetScheme m_targetScheme;
    private TargetFunction m_targetFunction;
    private Enemy m_currentTarget;

    private float cooldownTimer = 0;
    public float attackSpeed;

    public Projectile projectilePrefab;
    public int damage;
    public int projectileSpeed;
    public float unitSize;
    public float range;

    public Animator animator;

    private void Awake()
    {
        m_targetScheme = TargetScheme.First;
        m_targetFunction = CompareFirstTarget;
        m_currentTarget = null;
    }

    private void CompareFirstTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!m_currentTarget)
        {
            m_currentTarget = otherEnemy;
            return;
        }
        //Compare percent distances of the 2 enemies

        if(otherEnemy.GetPercentDistanceTraveled() > m_currentTarget.GetPercentDistanceTraveled())
            m_currentTarget = otherEnemy;
    }
    
    private void CompareLastTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!m_currentTarget)
        {
            m_currentTarget = otherEnemy;
            return;
        }
        if(otherEnemy.GetPercentDistanceTraveled() < m_currentTarget.GetPercentDistanceTraveled())
            m_currentTarget = otherEnemy;
    }
    
    private void CompareClosestTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!m_currentTarget)
        {
            m_currentTarget = otherEnemy;
            return;
        }

        float distanceToOtherEnemy = Vector3.Distance(transform.position, other.transform.position);
        float distanceToCurrent = Vector3.Distance(transform.position, m_currentTarget.transform.position);

        if(distanceToOtherEnemy < distanceToCurrent)
            m_currentTarget = otherEnemy;
    }
    
    private void CompareStrongestTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!m_currentTarget)
        {
            m_currentTarget = otherEnemy;
            return;
        }

        if(otherEnemy.health > m_currentTarget.health)
            m_currentTarget = otherEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && !m_currentTarget)
            m_currentTarget = collision.GetComponent<Enemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
            m_targetFunction(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() == m_currentTarget)
            m_currentTarget = null;
    }

    public void Start()
    {
        GetComponent<CircleCollider2D>().radius = range;
        transform.localScale = new Vector3(unitSize, unitSize, 1);
        animator.speed = attackSpeed;
    }

    public void Update()
    {
        //For debugging purposes
        if(Input.GetKeyDown(KeyCode.A))
            PreviousTargetScheme();
        if(Input.GetKeyDown(KeyCode.D))
            NextTargetScheme();
        //End debugging purposes
    }

    public void FixedUpdate()
    {
        if(m_currentTarget)
        {
            //Set animator fire bool to true
            animator.SetBool("Fire", true);
            Vector2 difference = m_currentTarget.transform.position - transform.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(difference.y, difference.x) / Mathf.PI * 180);
        }
        else
        {
            //Set animator fire bool to false
            animator.SetBool("Fire", false);
        }
    }

    public void NextTargetScheme()
    {
        m_targetScheme = (TargetScheme)(((int)m_targetScheme + 1) % 4);
        SelectTargetFunction();
    }

    public void PreviousTargetScheme()
    {
        int targetScheme = (int)m_targetScheme - 1;
        if(targetScheme < 0)
            targetScheme += 4;
        m_targetScheme = (TargetScheme)targetScheme;
        SelectTargetFunction();
    }

    private void SelectTargetFunction()
    {
        switch(m_targetScheme)
        {
        case TargetScheme.First:
            m_targetFunction = CompareFirstTarget;
            break;
        case TargetScheme.Last:
            m_targetFunction = CompareLastTarget;
            break;
        case TargetScheme.Closest:
            m_targetFunction = CompareClosestTarget;
            break;
        case TargetScheme.Strongest:
            m_targetFunction = CompareStrongestTarget;
            break;
        }
    }

    public void FireProjectile()
    {
        if(!m_currentTarget)
            return;
        Projectile projectile = Instantiate<Projectile>(projectilePrefab, transform.position, Quaternion.identity);
        projectile.SetDamage(damage);
        projectile.SetVelocity((m_currentTarget.transform.position - transform.position) * projectileSpeed);
    }
}
