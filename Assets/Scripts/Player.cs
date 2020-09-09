using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool m_useController;
    [SerializeField]
    int m_playerSpeed = 10;
    public int projectileDamage = 3;
    public float projectileSpeed = 2.0f;

    public float attackSpeed;
    public Animator animator;

    Vector3 previousMouseInput = Vector3.zero;
    Vector2 previousLookInput = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        animator.speed = attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxis("PlayerMovementX"));
        //Debug.Log(Input.GetButton("Confirm"));
        //Debug.Log(m_useController);
        HandleInput();
    }

    public void HandleInput()
    {
        //Axis for controller movement and for kb movement are different
        Vector2 movement = (new Vector2(Input.GetAxis("CPlayerMovementX"), Input.GetAxis("CPlayerMovementY")) + new Vector2(Input.GetAxis("KBPlayerMovementX"), Input.GetAxis("KBPlayerMovementY"))).normalized ;
        Vector2 controllerLookInput = new Vector2(Input.GetAxis("CPlayerShootX"), Input.GetAxis("CPlayerShootY"));


        //If we're using controller, use controller's input for angle
        //else use mouse as our input angle
        float angle = 0;
        if(m_useController)
        {
            if(controllerLookInput == Vector2.zero)
            {
                angle = 90;
            }
            else
            {
                angle = Mathf.Atan2(controllerLookInput.y, controllerLookInput.x) * Mathf.Rad2Deg;
            }
        }
        else
        {
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 playerToMouseDiff = Input.mousePosition - playerScreenPos;

            angle = Mathf.Atan2(playerToMouseDiff.y, playerToMouseDiff.x) * Mathf.Rad2Deg;
        }

        //Change position and rotate player
        transform.position = transform.position + (new Vector3(movement.x, movement.y) * m_playerSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if(controllerLookInput != Vector2.zero || Input.GetButton("KBPlayerShoot"))
        {
            animator.SetBool("Fire", true);
            //Fire projectile
        }
        else
        {
            animator.SetBool("Fire", false);
        }

        //Check if mouse has moved and we're using controller
        if(previousMouseInput != Input.mousePosition && m_useController)
        {
            //Disable 
            m_useController = false;
        }
        //Check if controller look input changed and we're using controller
        else if(previousLookInput != controllerLookInput && !m_useController)
        {
            m_useController = true;
        }

        previousMouseInput = Input.mousePosition;
        previousLookInput = controllerLookInput;
    }

    public void FireProjectile()
    {
        Projectile projectile = ProjectilePool.instance.Spawn(transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90)).GetComponent<Projectile>();
        projectile.SetDamage(projectileDamage);

        projectile.SetVelocity(transform.right * projectileSpeed);
    }
}
