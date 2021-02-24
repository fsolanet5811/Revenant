using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFollow : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        offset.z = -0.09f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPosition.position + offset;
    }
}
