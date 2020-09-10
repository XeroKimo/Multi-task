using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestScript : MonoBehaviour
{
    public GameObject start;
    public ScrollRect scrollRect;

    int startIndex = 0;

    public int scrollStart = 4;

    public Vector3 startPos;
    public Vector3 endPos;
    float distance = 0;
    public GameObject end;
}
