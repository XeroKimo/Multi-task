using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.IO;
//Module: Core Module
//Version: 0.11

public class Tower : MonoBehaviour
{

    private float cooldownTimer = 0;
    public float attackSpeed;

    public Projectile projectilePrefab;
    public int damage;
    public int projectileSpeed;
    public float unitSize;
    public float range;

    public Animator animator;

    public CircleCollider2D enemyDetectionHitbox { get; private set; }
    public CircleCollider2D unitHitbox { get; private set; }

    private TowerEnemyDetector m_enemyDetector;

    private void Awake()
    {
        CircleCollider2D[] colliders = transform.GetComponentsInChildren<CircleCollider2D>();

        foreach(CircleCollider2D collider in colliders)
        {
            if(collider.gameObject.layer == LayerMask.NameToLayer("Unit Collider"))
                unitHitbox = collider;
            else
            {
                enemyDetectionHitbox = collider;
                m_enemyDetector = enemyDetectionHitbox.GetComponent<TowerEnemyDetector>();
            }
        }
    }

    private void Start()
    {
        enemyDetectionHitbox.radius = range;
        unitHitbox.radius = unitSize;
        //transform.localScale = new Vector3(unitSize, unitSize, 1);
        animator.speed = attackSpeed;
    }

    private void Update()
    {
        //For debugging purposes
        if(Input.GetKeyDown(KeyCode.A))
            m_enemyDetector.PreviousTargetScheme();
        if(Input.GetKeyDown(KeyCode.D))
            m_enemyDetector.NextTargetScheme();
        //End debugging purposes
    }

    private void FixedUpdate()
    {
        if(m_enemyDetector.currentTarget)
        {
            //Set animator fire bool to true
            animator.SetBool("Fire", true);
            Vector2 difference = m_enemyDetector.currentTarget.transform.position - transform.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(difference.y, difference.x) / Mathf.PI * 180);
        }
        else
        {
            //Set animator fire bool to false
            animator.SetBool("Fire", false);
        }
    }

    public void FireProjectile()
    {
        if(!m_enemyDetector.currentTarget)
            return;
        Projectile projectile = ProjectilePool.instance.Spawn(transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90)).GetComponent<Projectile>();
        projectile.SetDamage(damage);
        projectile.SetVelocity((m_enemyDetector.currentTarget.transform.position - transform.position) * projectileSpeed);
    }
}
