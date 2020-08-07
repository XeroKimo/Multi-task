using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField]
    private float               m_PlayerSpeed = 10.0f;
    [SerializeField]
    private float               m_BulletSpeed = 20.0f;
    [SerializeField]
    private Transform           m_BulletFirePoint;
    protected List<GameObject>  m_ListOfNearbyTowers = new List<GameObject>();
    public GameObject[]         m_TowerTemplateObjects = new GameObject[0];
    public Rigidbody            m_BulletTemplateObject;

    protected void HandlePlayerMovement()
    {
        Vector3 CurrentPosition = gameObject.transform.position;
        CurrentPosition.x += (Input.GetAxis("Horizontal") * m_PlayerSpeed * Time.fixedDeltaTime);
        CurrentPosition.z += (Input.GetAxis("Vertical") * m_PlayerSpeed * Time.fixedDeltaTime);
        gameObject.transform.position = CurrentPosition;
    }

    protected void HandlePlayerRotation()
    {
        Vector3 MousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        Vector3 DirectionToMouse = MousePositionInWorld - transform.position;
        DirectionToMouse.y = 0.0f;
        DirectionToMouse = DirectionToMouse.normalized;
        //transform.localRotation = Quaternion.LookRotation(DirectionToMouse);
        transform.rotation = Quaternion.LookRotation(DirectionToMouse);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
        HandlePlayerRotation();

        if(Input.GetMouseButtonDown(0))
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
            BulletObject.velocity = transform.forward * m_BulletSpeed;
        }
    }

    protected void HandleQuickSelectBarInput(int InputDir)
    {
        //Handle the left/right movement of the highlighted quick select item
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
