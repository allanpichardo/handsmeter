
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerLogger : MonoBehaviour
{
    public float smoothing = 0.3f;
    private LowpassFilter lowpassFilter;

    private void Start()
    {
        lowpassFilter = new LowpassFilter();
    }

    private void FixedUpdate()
    {
        Vector3 filteredAcceleration = lowpassFilter.GetFilteredVector(Input.acceleration, smoothing);

        Debug.Log(string.Format("Accel: X:{0} Y:{1} Z:{2}", filteredAcceleration.x, filteredAcceleration.y, filteredAcceleration.z));
    }

}
