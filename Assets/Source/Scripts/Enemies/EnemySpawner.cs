using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EnemySpawner
{
    private readonly Dictionary<Type, GameObject> _resourceCache;

    public EnemySpawner()
    {
        _resourceCache = new Dictionary<Type, GameObject>();
    }

    public T Spawn<T>(Vector2 position) where T : Enemy
    {
        // Grab the enemy resource so we know what to spawn.
        GameObject resource = GetResource(typeof(T));
        GameObject instantiated = UnityEngine.Object.Instantiate(resource, position, Quaternion.identity);
        instantiated.layer = LayerMask.NameToLayer("Target");
        instantiated.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        return instantiated.GetComponent<T>();
    }

    private GameObject GetResource(Type resourceType)
    {
        // If we already loaded this guy from resources, we do not have to again.
        if (_resourceCache.TryGetValue(resourceType, out GameObject resource))
            return resource;

        // This is s new resource type. Load it and put it in the cache.
        GameObject fromResources = Resources.Load<GameObject>(BuildResourcePath(PascalCaseToSpaced(resourceType.Name)));
        _resourceCache.Add(resourceType, fromResources);
        return fromResources;
    }

    private static string BuildResourcePath(string resourceName)
    {
        return $"Enemies/{resourceName}";
    }

    private static string PascalCaseToSpaced(string pascalCased)
    {
        StringBuilder builder = new StringBuilder();
        if(!string.IsNullOrEmpty(pascalCased))
        {
            builder.Append(pascalCased[0]);
            for(int i = 1; i < pascalCased.Length; i++)
            {
                if (char.IsUpper(pascalCased[i]))
                    builder.Append(' ');

                builder.Append(pascalCased[i]);
            }
        }

        return builder.ToString();
    }
}
