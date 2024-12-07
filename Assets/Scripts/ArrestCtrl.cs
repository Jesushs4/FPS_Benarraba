using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrestCtrl : MonoBehaviour
{
    public float speed;
    private Transform target;
    private Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.LookAt(parent);
    }

    public void Arrest(Transform objetive, Transform parentt)
    {
        target = objetive;
        parent = parentt;
    }
}
