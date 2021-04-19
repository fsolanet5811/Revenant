using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public struct ActionJob : IJob
{
    private readonly Action _action;

    public ActionJob(Action action)
    {
        _action = action;
    }

    public void Execute()
    {
        _action();
    }

    public static JobHandle Start(Action action)
    {
        ActionJob job = new ActionJob(action);
        return job.Schedule();
    }
}
