using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data<T> : ScriptableObject
{
    public T Value;

    public static implicit operator T(Data<T> data)
    {
        return data.Value;
    }
}
