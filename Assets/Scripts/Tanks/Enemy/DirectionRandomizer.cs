using System.Collections.Generic;
using UnityEngine;

public class DirectionRandomizer 
{
    private Dictionary<Vector2, int> directionWeights;

    public DirectionRandomizer()
    {
        InitializeDirectionRandomPercentage();
    }

    private void InitializeDirectionRandomPercentage()
    {
        directionWeights = new Dictionary<Vector2, int>();
        directionWeights.Add(Vector2.down, 5);
        directionWeights.Add(Vector2.left, 2);
        directionWeights.Add(Vector2.right, 2);
        directionWeights.Add(Vector2.up, 1);
    }
    public Vector2 GetRandomDirection(List<Vector2> possibleDirections)
    {
        var totalWeight = 0;
        var processedWeight = 0;

        foreach (var direction in possibleDirections)
        {
            totalWeight += directionWeights.GetValueOrDefault(direction);
        }

        int rndWeightValue = Random.Range(1, totalWeight + 1);
        foreach (var direction in possibleDirections)
        {
            processedWeight += directionWeights.GetValueOrDefault(direction);
          
            if (rndWeightValue <= processedWeight)
            {
                return direction;
            }
        }
        return Vector2.zero;
    }
}
