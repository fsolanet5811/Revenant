using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAlpha : MonoBehaviour
{
    [SerializeField] Material fov;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        color = fov.color;
        color.a = 0f;
        fov.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        color = fov.color;
        color.a = 0f;
        fov.color = color;
    }
}
