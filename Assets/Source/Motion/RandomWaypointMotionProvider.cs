using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class RandomWaypointMotionProvider : WaypointMotionProvider
{
    private readonly Tuple<int, float>[] _allWeightIndecies;

    public RandomWaypointMotionProvider(AIPath path, AIDestinationSetter destinationSetter, Transform self, IEnumerable<WeightedTransform> waypoints)
        : base(path, destinationSetter, self, waypoints?.Select(wp => wp.transform)) 
    {
        _allWeightIndecies = GetWeightIndecies(waypoints).ToArray();
    }

    protected override int GetInitialWaypoint()
    {
        return GetRandomWaypoint(_allWeightIndecies);
    }

    protected override int GetNextWaypoint(int currentWaypoint)
    {
        return GetRandomWaypoint(_allWeightIndecies.Where(wi => wi.Item1 != currentWaypoint).ToArray());
    }

    private int GetRandomWaypoint(Tuple<int, float>[] weightIndecies)
    {
        // Calculate the total weight we have to deal with and the cumulative weight up to that point.
        float totalWeight = 0;
        Tuple<int, float>[] cumulative = new Tuple<int, float>[weightIndecies.Length];
        for (int i = 0; i < weightIndecies.Length; i++)
            cumulative[i] = new Tuple<int, float>(weightIndecies[i].Item1, totalWeight += weightIndecies[i].Item2);

        float target = Random.Range(0, totalWeight);

        // Find the value that encompasses the target.
        foreach (Tuple<int, float> weightIndex in cumulative)
            if (weightIndex.Item2 > target)
                return weightIndex.Item1;

        throw new Exception("Error getting random waypoint.");
    }

    private static IEnumerable<Tuple<int, float>> GetWeightIndecies(IEnumerable<WeightedTransform> weightedTransforms)
    {
        if(weightedTransforms != null)
        {
            int index = 0;
            foreach (WeightedTransform wt in weightedTransforms)
                yield return new Tuple<int, float>(index++, wt.Weight);
        }
    }
}
