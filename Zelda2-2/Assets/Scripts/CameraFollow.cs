﻿using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float dampTime = 0f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public float minX, minY, maxX, maxY;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 dest = transform.position + delta;
            Vector3 temp = Vector3.SmoothDamp(transform.position, dest, ref velocity, dampTime);
            temp.y = Mathf.Max(minY, temp.y);
            transform.position = temp;
        }

    }
}

