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

    private void Enemy_onDeathEvent(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
        enemy.onDeathEvent -= Enemy_onDeathEvent;
    }

    public void OnTriggerEnter(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Add(enemy);
            enemy.onDeathEvent += Enemy_onDeathEvent;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
            enemy.onDeathEvent -= Enemy_onDeathEvent;
        }
    }

    public void FixedUpdate()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            cooldownTimer = cooldown;
            target = GetFirstEnemy();
            if (target)
            {
                transform.right = target.transform.position - transform.position;
                FireProjectile();
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
