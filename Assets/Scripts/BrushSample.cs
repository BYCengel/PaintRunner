using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushSample : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private int pathCounter = 0;

    private float deSpawnTime;
    private float creationTime;
    private float timer;

    void Start()
    {
        creationTime = Time.time;
        timer = creationTime;
        edgeCollider = GetComponent<EdgeCollider2D>();
        Destroy(gameObject, 5f);
    }

    public void SetPathForCollider(List<Vector2> points){
        edgeCollider.SetPoints(points);
        pathCounter++;
    }

    public void Erase(){//erases this instance of BrushSample
        Destroy(gameObject);
    }
}
