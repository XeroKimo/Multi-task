using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private Vector2 velocity;
    private Rigidbody cRigidbody;
    private Tower source;

    public delegate void OnDeath();
    public event OnDeath onDeathEvent;

    void Start()
    {
        cRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.ApplyDamage(damage);
        }
    }

    private void FixedUpdate()
    {
        cRigidbody.MovePosition(transform.position + new Vector3(velocity.x, velocity.y)*Time.deltaTime);
        if (isOutOfBounds())
        {
            onDeathEvent?.Invoke();
        }
    }

    private bool isOutOfBounds()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        return screenPos.y < 0 || screenPos.y > Camera.main.scaledPixelHeight || screenPos.x < 0 || screenPos.x > Camera.main.scaledPixelWidth;
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }
}
