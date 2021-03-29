using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This MonoBehaviour Managers Explosions and the Direction and Spread of each bomb.
/// </summary>
public class ExplosionManager2D : MonoBehaviour
{
    #region VARIABLES

    [Header("Explosion Prefabs")]
    public List<GameObject> ExplosionPrefabs = new List<GameObject>();

    [Header("Explosion Offset")]
    public float OffsetPoint;

    private readonly List<Direction> _directions = new List<Direction>()
    {
        Direction.Down,
        Direction.Left,
        Direction.Right,
        Direction.Up,
    };

    #endregion

    #region UNITY METHODS

    #endregion

    #region METHODS

    /// <summary>
    /// Call this function to create a string of explosions in 4 directions, Down, Up, Left and Right.
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
                Direction.Left  => GetExplosionRadius(origin, Vector3.left, radius, contactMask),
                Direction.Right => GetExplosionRadius(origin, Vector3.right, radius, contactMask),
                Direction.Down  => GetExplosionRadius(origin, Vector3.down, radius, contactMask),
                Direction.Up    => GetExplosionRadius(origin, Vector3.up, radius, contactMask),
                _               => 0,
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
        return Physics2D.Raycast(origin, direction, radius, contactMask).distance;
    }

    private void SpawnExplosions(Vector3 origin, float radius, Direction direction, BombType bombType)
    {
        if (direction == Direction.Left)
            SpawnExplosionsInDirection(origin, -radius, direction, bombType);
        else if (direction == Direction.Right)
            SpawnExplosionsInDirection(origin, radius, direction, bombType);
        else if (direction == Direction.Down)
            SpawnExplosionsInDirection(origin, -radius, direction, bombType);
        else if (direction == Direction.Up)
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
        else if (direction == Direction.Up || direction == Direction.Down)
            originPoint = origin.y;

        // Create Explosions in the Right or Forward Directions
        if (direction == Direction.Right || direction == Direction.Up)
        {
            for (float i = originPoint + OffsetPoint; i <= originPoint + radius; i += OffsetPoint)
            {
                if (direction == Direction.Right)
                    origin.x = i;
                if (direction == Direction.Up)
                    origin.y = i;

                Instantiate(ExplosionPrefabs[(int) bombType], origin, Quaternion.identity);
            }
        }
        // Create Explosions in the Left or Backward Directions
        else if (direction == Direction.Left || direction == Direction.Down)
        {
            for (float i = originPoint + -OffsetPoint; i >= originPoint + radius; i += -OffsetPoint)
            {
                if (direction == Direction.Left)
                    origin.x = i;
                if (direction == Direction.Down)
                    origin.y = i;

                Instantiate(ExplosionPrefabs[(int) bombType], origin, Quaternion.identity);
            }
        }
    }

    #endregion
}