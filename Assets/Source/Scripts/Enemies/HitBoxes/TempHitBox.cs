using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHitBox : HitBox
{
    public int AliveFrames { get; set; }

    protected virtual void Start()
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
