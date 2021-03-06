﻿using System;
using MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class HandsEmpathyAgent : Agent
{
    public bool showDebug = false;
    public float filterBeta = 0.5f;
    public GameObject playerHead;
    public GameObject playerLeftHand;
    public GameObject playerRightHand;

    private LowpassFilter lowpassFilter;
    
    public override void InitializeAgent()
    {
        lowpassFilter = new LowpassFilter();
    }

    public override void CollectObservations()
    {
        Vector3 leftPosition = playerLeftHand.transform.position.normalized;
        Quaternion leftRotation = playerLeftHand.transform.rotation.normalized;

        Vector3 rightPosition = playerRightHand.transform.position.normalized;
        Quaternion rightRotation = playerRightHand.transform.rotation.normalized;
        
        Vector3 headPosition = playerHead.transform.position.normalized;
        Quaternion headRotation = playerHead.transform.rotation.normalized;
        
        AddVectorObs(leftPosition);
        AddVectorObs(leftRotation);
        AddVectorObs(rightPosition);
        AddVectorObs(rightRotation);
        AddVectorObs(headPosition);
        AddVectorObs(headRotation);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float valence = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);
        float arousal = Mathf.Clamp(vectorAction[1], -1.0f, 1.0f);

        Vector3 inference = lowpassFilter.GetFilteredVector(new Vector3(valence, arousal, 0), filterBeta);
        
        if (showDebug)
        {
            Monitor.Log("Valence", inference.x);
            Monitor.Log("Arousal", inference.y);
        }

        OnNewPrediction(inference);
    }

    /**
     * Override this method to receive predictions
     */
    public abstract void OnNewPrediction(Vector3 inference);

    public override void AgentReset()
    {
    }
}
