
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Filters
{
    public class LowpassFilter
    {
        private Vector3 lastVector;

        public LowpassFilter()
        {
            this.lastVector = new Vector3();
        }

        public void SetPreviousValue(Vector3 lastVector)
        {
            this.lastVector = lastVector;
        }

        private float Filter(float unfiltered, float beta, float lastValue)
        {
            float Y;
            Y = beta * unfiltered + (1 - beta) * lastValue;
            return Y;
        }

        public Vector3 GetFilteredVector(Vector3 unfiltered, float beta)
        {
            float x = Filter(unfiltered.x, beta, lastVector.x);
            float y = Filter(unfiltered.y, beta, lastVector.y);
            float z = Filter(unfiltered.z, beta, lastVector.z);

            Vector3 filtered = new Vector3(x, y, z);

            this.lastVector = filtered;

            return lastVector;
        }

    }
}
