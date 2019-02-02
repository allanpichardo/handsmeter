using System;
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

    private float CalculateReward(float predicted, float actual)
    {
        float absDistance = Mathf.Abs(predicted - actual);
        float sigmoid = (float) (1.0f / (Math.Exp(absDistance) + 1));
        return 1 - sigmoid;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Debug.Log(vectorAction[0]);
        float reward = CalculateReward(vectorAction[0], playbackReader.GetCurrentState());
        SetReward(reward);
        
        if (reward > 0)
        {
            material.color = new Color(0,reward,0);
        }
        else if(reward < 0)
        {
            material.color = new Color(reward, 0, 0);
        }
        else
        {
            material.color = Color.white;
        }
    }

    public override void AgentReset()
    {
        material.color = Color.white;
    }
}
