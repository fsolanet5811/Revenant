using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform playerPosition;

    //[SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] Vector3 offset;

    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        offset.z = -3.0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerPosition.position + offset;

        
    }
}
