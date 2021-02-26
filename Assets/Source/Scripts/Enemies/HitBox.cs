using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitBox : MonoBehaviour
{
    public float Damage { get; set; }

    public int AliveFrames { get; set; }

    private void Start()
    {
        StartCoroutine(SelfDestructCoroutine(AliveFrames));
    }

    private IEnumerator SelfDestructCoroutine(int aliveFrames)
    {
        for (int i = 0; i < aliveFrames; i++)
            yield return null;

        Destroy(gameObject);
    }
}
