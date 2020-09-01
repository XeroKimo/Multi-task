using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetScheme
{
    First,
    Last,
    Closest,
    Strongest
}

public class TowerEnemyDetector : MonoBehaviour
{
    private delegate void TargetFunction(Collider2D other);

    private TargetScheme m_targetScheme;
    private TargetFunction m_targetFunction;
    public Enemy currentTarget { get; private set; }
    private Enemy m_nextTarget;

    private void Awake()
    {
        m_targetScheme = TargetScheme.First;
        m_targetFunction = CompareFirstTarget;
        currentTarget = null;
    }

    private void CompareFirstTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!currentTarget)
        {
            currentTarget = otherEnemy;
            return;
        }


        if(otherEnemy.GetPercentDistanceTraveled() > currentTarget.GetPercentDistanceTraveled())
            currentTarget = otherEnemy;
        else if(otherEnemy != currentTarget && !m_nextTarget)
            m_nextTarget = otherEnemy;
        else if(m_nextTarget)
        {
            if(m_nextTarget == currentTarget)
                m_nextTarget = null;
            else if(otherEnemy.GetPercentDistanceTraveled() > m_nextTarget.GetPercentDistanceTraveled())
                m_nextTarget = otherEnemy;
        }
    }

    private void CompareLastTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!currentTarget)
        {
            currentTarget = otherEnemy;
            return;
        }

        if(otherEnemy.GetPercentDistanceTraveled() < currentTarget.GetPercentDistanceTraveled())
            currentTarget = otherEnemy;
        else if(otherEnemy != currentTarget && !m_nextTarget)
            m_nextTarget = otherEnemy;
        else if(m_nextTarget)
        {
            if(m_nextTarget == currentTarget)
                m_nextTarget = null;
            else if(otherEnemy.GetPercentDistanceTraveled() < m_nextTarget.GetPercentDistanceTraveled())
                m_nextTarget = otherEnemy;
        }
    }

    private void CompareClosestTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!currentTarget)
        {
            currentTarget = otherEnemy;
            return;
        }

        float distanceToOtherEnemy = Vector3.Distance(transform.position, other.transform.position);
        float distanceToCurrent = Vector3.Distance(transform.position, currentTarget.transform.position);

        if(distanceToOtherEnemy < distanceToCurrent)
            currentTarget = otherEnemy;
        else if(otherEnemy != currentTarget && !m_nextTarget)
            m_nextTarget = otherEnemy;
        else if(m_nextTarget)
        {
            if(m_nextTarget == currentTarget)
                m_nextTarget = null;
            else
            {
                float distanceToNext = Vector3.Distance(transform.position, m_nextTarget.transform.position);
                if(distanceToOtherEnemy < distanceToNext)
                    m_nextTarget = otherEnemy;
            }
        }
    }

    private void CompareStrongestTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!currentTarget)
        {
            currentTarget = otherEnemy;
            return;
        }

        if(otherEnemy.health > currentTarget.health)
            currentTarget = otherEnemy;
        else if(otherEnemy != currentTarget && !m_nextTarget)
            m_nextTarget = otherEnemy;
        else if(m_nextTarget)
        {
            if(m_nextTarget == currentTarget)
                m_nextTarget = null;
            else if(otherEnemy.health > m_nextTarget.health)
                m_nextTarget = otherEnemy;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && !currentTarget)
            currentTarget = collision.GetComponent<Enemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
            m_targetFunction(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == currentTarget.gameObject)
        {
            currentTarget = m_nextTarget;
            m_nextTarget = null;
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
}
