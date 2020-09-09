using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
//using System.Numerics;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
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
    protected List<GameObject>  m_ListOfNearbyTowers = new List<GameObject>();
    protected GameObject        m_CurrentlyPlaceableTower = null;
    public GameObject[]         m_TowerTemplateObjects = new GameObject[0];
    public Rigidbody            m_BulletTemplateObject;

    [SerializeField]
    Player m_player;


    protected void HandlePlayerMovement()
    {
        Vector2 CurrentPosition = gameObject.transform.position;
        CurrentPosition.x += (Input.GetAxis("Horizontal") * m_PlayerSpeed * Time.fixedDeltaTime);
        CurrentPosition.y += (Input.GetAxis("Vertical") * m_PlayerSpeed * Time.fixedDeltaTime);
        gameObject.transform.position = CurrentPosition;
    }

    protected void HandlePlayerRotation()
    {
        //Old Code made for 3D
        //Vector3 MousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition /*+ Vector3.up * 10*/);
        //Vector3 DirectionToMouse = MousePositionInWorld - transform.position;
        //DirectionToMouse.z = 0.0f;
        //DirectionToMouse = DirectionToMouse.normalized;
        ////transform.localRotation = Quaternion.LookRotation(DirectionToMouse);
        //transform.rotation = Quaternion.LookRotation(DirectionToMouse);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.z - transform.position.z;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90.0f));
    }

    protected void HandlePlacingTower()
    {
        if(m_IsPlacingTower == true)
        {
            m_CurrentlyPlaceableTower.transform.position = transform.position + (transform.up * m_TowerPlacementDistance);
            //Change tower opacity until confirmed
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        m_player.HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandlePlayerMovement();
        HandlePlayerRotation();
        HandlePlacingTower();

        //Pressing P will prep to place tower && pressing O will place tower if one is being prepped
        if(Input.GetAxis("PrepareTower") > 0)
        {
            PlaceCurrentlySelectedtower();
        }
        if (Input.GetAxis("PlaceTower") > 0)
        {
            ConfirmTowerPlacement();
        }
        if (Input.GetMouseButtonDown(0) && m_IsPlacingTower == false)
        {
            FireBullet();
        }
    }

    protected void FireBullet()
    {
        //Instantiate bullet prefab in here
        if(m_BulletTemplateObject != null && m_BulletFirePoint != null)
        {
            Rigidbody BulletObject = (Rigidbody)Instantiate(m_BulletTemplateObject, m_BulletFirePoint.position, m_BulletFirePoint.rotation);
            //BulletObject.velocity = transform.forward * m_BulletSpeed;
            BulletObject.velocity = transform.up * m_BulletSpeed;
        }
    }

    protected void HandleQuickSelectBarInput(int InputDir)
    {
        //Handle the left/right movement of the highlighted quick select item
        //Will set which tower template to use

        //Need to verify last idex position as well - TODO
        if(m_TowerTemplateListIndex > 0 /*&& m_TowerTemplateListIndex < List.Length*/)
        {
            m_TowerTemplateListIndex += InputDir;
        }
       
    }

    protected void DisplayCurrentSelectedTowerInfo()
    {
        //Display the current selected tower info for player
        //health
        //dmg
        //etc
    }

    protected void PlaceCurrentlySelectedtower()
    {
        //Place a new tower of the currently selcted/highlighted version
        //Current selected tower will be hard coded as element 0 until UI is going to be implemented
        if(m_TowerTemplateObjects[m_TowerTemplateListIndex] != null && m_CurrentlyPlaceableTower == null && m_IsPlacingTower == false)
        {
            Vector3 TowerStartPos = transform.position + (transform.up * m_TowerPlacementDistance);
            m_CurrentlyPlaceableTower = Instantiate(m_TowerTemplateObjects[m_TowerTemplateListIndex], TowerStartPos, Quaternion.Euler(0, 0, 90));
            m_IsPlacingTower = true;
            //change tower opacity here - TODO
        }
    }

    protected void ConfirmTowerPlacement()
    {
        //corresponds to second key press to place tower
        m_CurrentlyPlaceableTower = null;
        m_IsPlacingTower = false;
    }

    protected void CancelCurrentUIInput()
    {
        //Close the UI currently open
    }

    protected void DisplayInfoOfNearbyTower(GameObject CurrNearbyTower)
    {
        //Display current stats of the closest tower within range
    }

    protected void OpenPlayerMenu()
    {
        //opens player menu UI
    }
}
