using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRectTest : MonoBehaviour
{
    public RectTransform content;



    private int selectedObjectID = 0;

    float scrollDelay = 0.2f;
    bool canScroll = true;

    public GameObject start;
    public GameObject end;

    private void Start()
    {
        Debug.Log(start.transform.localPosition);   
        Debug.Log(end.transform.localPosition);

        EventSystem.current.SetSelectedGameObject(content.GetChild(0).gameObject);
    }

    void EnableScroll()
    {
        canScroll = true;
    }

    private void FixedUpdate()
    {
        Debug.Log(selectedObjectID);
        if(canScroll)
        {
            if(Input.GetAxis("DPadY") > 0 && selectedObjectID > 0)
            {
                selectedObjectID--;

                content.position -= new Vector3(0, ((RectTransform)content.GetChild(selectedObjectID).transform).rect.height, 0);

                EventSystem.current.SetSelectedGameObject(content.GetChild(selectedObjectID).gameObject);

                canScroll = false;
                Invoke("EnableScroll", scrollDelay);
            }
            else if(Input.GetAxis("DPadY") < 0 && selectedObjectID < content.childCount - 1)
            {
                selectedObjectID++;

                content.position += new Vector3(0, ((RectTransform)content.GetChild(selectedObjectID).transform).rect.height, 0);
                EventSystem.current.SetSelectedGameObject(content.GetChild(selectedObjectID).gameObject);
                canScroll = false;
                Invoke("EnableScroll", scrollDelay);
            }
        }
    }
}