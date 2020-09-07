using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
//using System.Numerics;
using UnityEngine;

/*
 * Need to add a few places for alternate input for controller
 */
public class PlayerInputScript : MonoBehaviour
{
    bool                        m_useController;
    [SerializeField]
    private float               m_PlayerSpeed = 10.0f;
    [SerializeField]
    private float               m_BulletSpeed = 20.0f;
    [SerializeField]
    private Transform           m_BulletFirePoint = null;
    private bool                m_IsPlacingTower = false;
    private int                 m_TowerTemplateListIndex = 0;
    [SerializeField]
    private int                 m_TowerPlacementDistance = 5;
    protected GameObject        m_CurrentlyPlaceableTower = null;
    public GameObject[]         m_TowerTemplateObjects = new GameObject[0];
    public Rigidbody            m_BulletTemplateObject;

    Vector3 previousMouseInput = Vector3.zero;
    Vector2 previousLookInput = Vector2.zero;

    protected void HandlePlayerMovement()
    {
        Vector2 movement = (new Vector2(Input.GetAxis("CPlayerMovementX"), Input.GetAxis("CPlayerMovementY")) + new Vector2(Input.GetAxis("KBPlayerMovementX"), Input.GetAxis("KBPlayerMovementY"))).normalized;
        transform.position = transform.position + (new Vector3(movement.x, movement.y) * m_playerSpeed * Time.fixedDeltaTime);
        //Vector2 CurrentPosition = gameObject.transform.position;
        //CurrentPosition.x += (Input.GetAxis("Horizontal") * m_PlayerSpeed * Time.fixedDeltaTime);
        //CurrentPosition.y += (Input.GetAxis("Vertical") * m_PlayerSpeed * Time.fixedDeltaTime);
        //gameObject.transform.position = CurrentPosition;
    }

    protected void HandlePlayerRotation()
    {
        //Lucas' code for rotation
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = Camera.main.transform.position.z - transform.position.z;

        //Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        //mousePos.x = mousePos.x - objectPos.x;
        //mousePos.y = mousePos.y - objectPos.y;

        //float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90.0f));

        Vector2 controllerLookInput = new Vector2(Input.GetAxis("CPlayerShootX"), Input.GetAxis("CPlayerShootY")).normalized;

        //If we're using controller, use controller's input for angle
        //else use mouse as our input angle
        float angle = 0;
        if (m_useController)
        {
            if (controllerLookInput == Vector2.zero)
            {
                Debug.Log("No look");
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
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    //protected void HandlePlacingTower()
    //{
    //    if(m_IsPlacingTower == true)
    //    {
    //        m_CurrentlyPlaceableTower.transform.position = transform.position + (transform.up * m_TowerPlacementDistance);
    //        //Change tower opacity until confirmed
    //    }
    //}

    protected void FireBullet()
    {
        //Instantiate bullet prefab in here
        if (m_BulletTemplateObject != null && m_BulletFirePoint != null)
        {
            Rigidbody BulletObject = (Rigidbody)Instantiate(m_BulletTemplateObject, m_BulletFirePoint.position, m_BulletFirePoint.rotation);
            //BulletObject.velocity = transform.forward * m_BulletSpeed;
            BulletObject.velocity = transform.up * m_BulletSpeed;
        }
    }

    //protected void PlaceCurrentlySelectedtower()
    //{
    //    //Place a new tower of the currently selcted/highlighted version
    //    //Current selected tower will be hard coded as element 0 until UI is going to be implemented
    //    if(m_TowerTemplateObjects[m_TowerTemplateListIndex] != null && m_CurrentlyPlaceableTower == null && m_IsPlacingTower == false)
    //    {
    //        Vector3 TowerStartPos = transform.position + (transform.up * m_TowerPlacementDistance);
    //        m_CurrentlyPlaceableTower = Instantiate(m_TowerTemplateObjects[m_TowerTemplateListIndex], TowerStartPos, Quaternion.Euler(0, 0, 90));
    //        m_IsPlacingTower = true;
    //        //change tower opacity here - TODO
    //    }
    //}

    //protected void ConfirmTowerPlacement()
    //{
    //    //corresponds to second key press to place tower
    //    m_CurrentlyPlaceableTower = null;
    //    m_IsPlacingTower = false;
    //}

    protected void HandlePlayerInput()
    {
        HandlePlayerMovement();
        HandlePlayerRotation();
        HandlePlacingTower();

        //Pressing P will prep to place tower && pressing O will place tower if one is being prepped
        if (Input.GetAxis("PrepareTower") > 0)
        {
            PlaceCurrentlySelectedtower();
        }
        if (Input.GetAxis("PlaceTower") > 0)
        {
            ConfirmTowerPlacement();
        }
        if (controllerLookInput != Vector2.zero || Input.GetButton("KBPlayerShoot"))
        {
            //Fire projectile
            FireBullet();
        }

        //Check if mouse has moved and we're using controller
        if (previousMouseInput != Input.mousePosition && m_useController)
        {
            //Disable 
            m_useController = false;
        }
        //Check if controller look input changed and we're using controller
        else if (previousLookInput != controllerLookInput && !m_useController)
        {
            m_useController = true;
        }

        previousMouseInput = Input.mousePosition;
        previousLookInput = controllerLookInput;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //HandlePlayerMovement();
        //HandlePlayerRotation();
        HandlePlayerInput();
    }

    
}
