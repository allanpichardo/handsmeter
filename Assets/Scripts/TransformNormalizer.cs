using UnityEngine;

public class TransformNormalizer : MonoBehaviour
{
    private VectorStatistics positionStats;
    private VectorStatistics rotationStats;

    private void Start()
    {
        positionStats = new VectorStatistics();
        rotationStats = new VectorStatistics();
    }

    public void UpdateValues(Transform transform)
    {
        positionStats.AddValue(transform.localPosition);
        rotationStats.AddValue(transform.localEulerAngles);
    }

    public Vector3 GetNormalizedPosition()
    {
        return positionStats.Normalize(transform.parent.localPosition);
    }

    public Vector3 GetNormalizedRotation()
    {
        return rotationStats.Normalize(transform.parent.localEulerAngles);
    }

    public void ClearStats()
    {
        positionStats = new VectorStatistics();
        rotationStats = new VectorStatistics();
    }
}