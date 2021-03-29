using System.Collections.Generic;
using UnityEngine;

public enum BombType
{
    TimeBomb = 0,
}

public enum Direction
{
    Backward,
    Forward,
    Left,
    Right,
    Up,
    Down,
}

/// <summary>
/// This MonoBehaviour Managers Explosions and the Direction and Spread of each bomb.
/// </summary>
public class ExplosionManager3D : MonoBehaviour
{
    #region VARIABLES

    [Header("Explosion Prefabs")]
    public List<GameObject> ExplosionPrefabs = new List<GameObject>();

    [Header("Explosion Offset")]
    public float OffsetPoint;

    private readonly List<Direction> _directions = new List<Direction>()
    {
        Direction.Backward,
        Direction.Forward,
        Direction.Left,
        Direction.Right,
    };

    #endregion

    #region UNITY METHODS

    #endregion

    #region METHODS

    /// <summary>
    /// Call this function to create a string of explosions in 4 directions, Backward, Forward, Left and Right.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    /// <param name="contactMask"></param>
    /// <param name="bombType"></param>
    public void CreateExplosions(Vector3 origin, float radius, LayerMask contactMask, BombType bombType)
    {
        // Create Center Explosion
        Instantiate(ExplosionPrefabs[(int) bombType], origin, Quaternion.identity);
        // Create an Explosion in 4 Directions Horizontal and Vertical (Z-Axis)
        foreach (Direction direction in _directions)
        {
            float distance = direction switch
            {
                Direction.Left     => GetExplosionRadius(origin, Vector3.left, radius, contactMask),
                Direction.Right    => GetExplosionRadius(origin, Vector3.right, radius, contactMask),
                Direction.Backward => GetExplosionRadius(origin, Vector3.back, radius, contactMask),
                Direction.Forward  => GetExplosionRadius(origin, Vector3.forward, radius, contactMask),
                _                  => 0,
            };


            if (Mathf.Approximately(distance, 0f))
            {
                SpawnExplosions(origin, radius, direction, bombType);
            }
            else
            {
                SpawnExplosions(origin, distance, direction, bombType);
            }
        }
    }

    private float GetExplosionRadius(Vector3 origin, Vector3 direction, float radius, LayerMask contactMask)
    {
        Physics.Raycast(origin, direction, out RaycastHit hit, radius, contactMask);
        return hit.distance;
    }

    private void SpawnExplosions(Vector3 origin, float radius, Direction direction, BombType bombType)
    {
        if (direction == Direction.Left)
            SpawnExplosionsInDirection(origin, -radius, direction, bombType);
        else if (direction == Direction.Right)
            SpawnExplosionsInDirection(origin, radius, direction, bombType);
        else if (direction == Direction.Backward)
            SpawnExplosionsInDirection(origin, -radius, direction, bombType);
        else if (direction == Direction.Forward)
            SpawnExplosionsInDirection(origin, radius, direction, bombType);
    }

    /// <summary>
    /// Spawns a String of Explosions in the Specified Direction.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    /// <param name="direction"></param>
    /// <param name="bombType"></param>
    private void SpawnExplosionsInDirection(Vector3 origin, float radius, Direction direction, BombType bombType)
    {
        float originPoint = 0f;

        // Set X or Z Origin Point Based on Direction
        if (direction == Direction.Left || direction == Direction.Right)
            originPoint = origin.x;
        else if (direction == Direction.Forward || direction == Direction.Backward)
            originPoint = origin.z;

        // Create Explosions in the Right or Forward Directions
        if (direction == Direction.Right || direction == Direction.Forward)
        {
            for (float i = originPoint + OffsetPoint; i <= originPoint + radius; i += OffsetPoint)
            {
                if (direction == Direction.Right)
                    origin.x = i;
                if (direction == Direction.Forward)
                    origin.z = i;

                Instantiate(ExplosionPrefabs[(int) bombType], origin, Quaternion.identity);
            }
        }
        // Create Explosions in the Left or Backward Directions
        else if (direction == Direction.Left || direction == Direction.Backward)
        {
            for (float i = originPoint + -OffsetPoint; i >= originPoint + radius; i += -OffsetPoint)
            {
                if (direction == Direction.Left)
                    origin.x = i;
                if (direction == Direction.Backward)
                    origin.z = i;

                Instantiate(ExplosionPrefabs[(int) bombType], origin, Quaternion.identity);
            }
        }
    }

    #endregion
}