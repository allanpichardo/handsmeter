using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerLogger : MonoBehaviour
{
    public string blah;
    public float velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Acceleration X: " + Input.acceleration.x);
    }
}
