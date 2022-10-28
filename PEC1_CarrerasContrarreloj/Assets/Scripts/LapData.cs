using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject to save and recover the data of a Lap or Race
/// </summary>
[CreateAssetMenu]
public class LapData : ScriptableObject
{
    List<Vector3> positions = new List<Vector3>();
    List<Quaternion> rotations = new List<Quaternion>();

    public void AddNewData(Transform transform)
    {
        positions.Add(transform.position);
        rotations.Add(transform.rotation);
    }

    public void AddNewData(Vector3 position, Quaternion rotation)
    {
        positions.Add(position);
        rotations.Add(rotation);
    }

    public void GetDataAt(int sample, out Vector3 position, out Quaternion rotation)
    {
        position = positions[sample];
        rotation = rotations[sample];
    }

    public void Reset()
    {
        positions.Clear();
        rotations.Clear();
    }
    
    public int GetNumberOfSamples()
    {
        return positions.Count;
    }
}
