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

    public TargetScheme targetScheme { get; private set; }
    private TargetFunction m_targetFunction;
    public Enemy currentTarget { get; private set; }
    private Enemy m_nextTarget;

    private void Awake()
    {
        targetScheme = TargetScheme.First;
        m_targetFunction = CompareFirstTarget;
        currentTarget = null;
    }

    private void CompareFirstTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!currentTarget)
        {
            currentTarget = otherEnemy;
            OnEnemyChanged(null, currentTarget);
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
            {
                Enemy target = currentTarget;
                currentTarget = otherEnemy;
                OnEnemyChanged(target, currentTarget);
            }
        }
    }

    private void CompareLastTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!currentTarget)
        {
            currentTarget = otherEnemy;
            OnEnemyChanged(null, currentTarget);
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
            {
                Enemy target = currentTarget;
                currentTarget = otherEnemy;
                OnEnemyChanged(target, currentTarget);
            }
        }
    }

    private void CompareClosestTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!currentTarget)
        {
            currentTarget = otherEnemy;
            OnEnemyChanged(null, currentTarget);
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
                {
                    Enemy target = currentTarget;
                    currentTarget = otherEnemy;
                    OnEnemyChanged(target, currentTarget);
                }
            }
        }
    }

    private void CompareStrongestTarget(Collider2D other)
    {
        Enemy otherEnemy = other.GetComponent<Enemy>();
        if(!currentTarget)
        {
            currentTarget = otherEnemy;
            OnEnemyChanged(null, currentTarget);
            return;
        }

        if(otherEnemy.health > currentTarget.health)
        {
            Enemy target = currentTarget;
            currentTarget = otherEnemy;
            OnEnemyChanged(target, currentTarget);
        }
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
        if(currentTarget && collision.gameObject == currentTarget.gameObject)
        {
            Enemy target = currentTarget;
            currentTarget = m_nextTarget;
            m_nextTarget = null;

            OnEnemyChanged(target, currentTarget);
        }
    }

    void OnEnemyChanged(Enemy oldEnemy, Enemy newEnemy)
    {
        if(oldEnemy)
        {
            oldEnemy.onDeath -= OldEnemy_onDeath;
            oldEnemy.onPathFinished -= OldEnemy_onDeath;
        }
        if(newEnemy)
        {
            newEnemy.onDeath += OldEnemy_onDeath;
            newEnemy.onPathFinished += OldEnemy_onDeath;
        }
    }


    private void OldEnemy_onDeath(Enemy enemy)
    {
        currentTarget = null;
        OnEnemyChanged(enemy, null);
        m_nextTarget = null;
    }

    public void NextTargetScheme()
    {
        targetScheme = (TargetScheme)(((int)targetScheme + 1) % 4);
        SelectTargetFunction();
    }

    public void PreviousTargetScheme()
    {
        int targetScheme = (int)this.targetScheme - 1;
        if(targetScheme < 0)
            targetScheme += 4;
        this.targetScheme = (TargetScheme)targetScheme;
        SelectTargetFunction();
    }

    private void SelectTargetFunction()
    {
        switch(targetScheme)
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
