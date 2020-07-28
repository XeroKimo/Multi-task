using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Tower : MonoBehaviour
{
    private Enemy target;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    private float cooldownTimer = 0;
    public float cooldown;

    public Projectile projectilePrefab;
    public int damage;
    public int projectileSpeed;
    public float unitSize;
    public float range;

    private void Enemy_onDeathEvent(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
        enemy.onDeath -= Enemy_onDeathEvent;
    }

    public void OnTriggerEnter(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Add(enemy);
            enemy.onDeath += Enemy_onDeathEvent;
            Debug.Log("enter");
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
            enemy.onDeath -= Enemy_onDeathEvent;
        }
    }

    public void Start()
    {
        GetComponent<SphereCollider>().radius = range;
        transform.localScale = new Vector3(unitSize, unitSize, 1);
    }

    public void FixedUpdate()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            target = GetFirstEnemy();
            if (target)
            {
                transform.right = target.transform.position - transform.position;
                FireProjectile();
                cooldownTimer = cooldown;
            }
        }
    }

    private Enemy GetFirstEnemy()
    {
        if (enemiesInRange.Count == 0) return null;
        Enemy target = enemiesInRange[0];
        enemiesInRange.ForEach(enemy => {
            if (enemy.distanceTraveled > target.distanceTraveled)
                target = enemy;
        });
        return target;
    }

    private Enemy GetLastEnemy()
    {
        if (enemiesInRange.Count == 0) return null;
        Enemy target = enemiesInRange[0];
        enemiesInRange.ForEach(enemy => {
            if (enemy.distanceTraveled < target.distanceTraveled)
                target = enemy;
        });
        return target;
    }

    private void FireProjectile()
    {
        if (!target) return;
        Projectile projectile = Instantiate<Projectile>(projectilePrefab, transform.position, transform.rotation);
        projectile.SetDamage(damage);
        projectile.SetVelocity((target.transform.position - transform.position) * projectileSpeed);
    }
}
