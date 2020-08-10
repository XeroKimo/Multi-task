using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.U2D.Path;
using UnityEngine;

[RequireComponent(typeof(Spline2DRoadComponent))]
[RequireComponent(typeof(PolygonCollider2D))]
public class Spline2DRoadHitbox : MonoBehaviour
{
    PolygonCollider2D m_collider;
    Spline2DRoadComponent m_spline;

    [SerializeField]
    float iterpolationAccuracy = 0.05f;
    private void Awake()
    {
        m_collider = GetComponent<PolygonCollider2D>();
        m_spline = GetComponent<Spline2DRoadComponent>();

        List<Tuple<Vector2, Vector2>> sidePoints = new List<Tuple<Vector2, Vector2>>();
        float t = 0;
        for(; t <= 1.0f; t += iterpolationAccuracy)
        {
            Vector2[] p = m_spline.InterpolateSides(t);
            sidePoints.Add(new Tuple<Vector2, Vector2>(p[0], p[1]));
        }
        if(t > 1.0f)
        {
            Vector2[] p = m_spline.InterpolateSides(1.0f);
            sidePoints.Add(new Tuple<Vector2, Vector2>(p[0], p[1]));
        }
        //var sidePoints = m_spline.GetAllSidePoints();
        
        int pathCount = sidePoints.Count - 1;
        m_collider.pathCount = pathCount;
        Vector2[] path = new Vector2[4];
        for(int i = 0; i < pathCount; i++)
        {
            path[0] = sidePoints[i].Item1;
            path[1] = sidePoints[i + 1].Item1;
            path[2] = sidePoints[i + 1].Item2;
            path[3] = sidePoints[i].Item2;

            m_collider.SetPath(i, path);
        }
        //Vector2[] path = new Vector2[sidePoints.Count * 2];
        //int index = 0;
        //foreach(Tuple<Vector2, Vector2> points in sidePoints)
        //{
        //    path[index] = points.Item1;
        //    path[index + 1] = points.Item2;
        //    index += 2;
        //}
        //m_collider.SetPath(0, path);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
