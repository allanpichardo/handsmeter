using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class EmpathyAgent : Agent
{
    public Reader playbackReader;
    public GameObject leftHand;
    public GameObject rightHand;

    private Material material;
    
    public override void InitializeAgent()
    {
        material = GetComponent<MeshRenderer>().material;
        material.SetColor("white", Color.white);
    }

    public override void CollectObservations()
    {
        material.color = Color.gray;
        Vector3 currentPosition = transform.position;
        
        AddVectorObs(Vector3.Distance(currentPosition, leftHand.transform.position));
        AddVectorObs(Vector3.Distance(currentPosition, rightHand.transform.position));
        AddVectorObs(leftHand.transform.localPosition);
        AddVectorObs(leftHand.transform.localEulerAngles);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Debug.Log(vectorAction[0]);
        if (playbackReader.GetCurrentState() == vectorAction[0])
        {
            material.color = Color.green;
            SetReward(1.0f);
            Done();
        }
        else
        {
            material.color = Color.red;
        }
    }

    public override void AgentReset()
    {
        material.color = Color.white;
    }
}
