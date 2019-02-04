using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class EmpathyAgent : Agent
{
    public Reader playbackReader;
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;
    public Text expectedText;
    [FormerlySerializedAs("actualText")] public Text guessText;

    private TransformNormalizer transformNormalizerLeft;
    private TransformNormalizer transformNormalizerRight;
    private Material material;
    
    public override void InitializeAgent()
    {
        transformNormalizerLeft = leftHand.GetComponent<TransformNormalizer>();
        transformNormalizerRight = rightHand.GetComponent<TransformNormalizer>();
        
        material = GetComponent<MeshRenderer>().material;
        material.color = Color.white;
    }

    public override void CollectObservations()
    {
        material.color = Color.gray;
        
        Vector3 currentPosition = transform.position;
        
        AddVectorObs(Vector3.Distance(currentPosition, player.transform.position));
        AddVectorObs(transformNormalizerLeft.GetNormalizedPosition());
        AddVectorObs(transformNormalizerLeft.GetNormalizedRotation());
        AddVectorObs(transformNormalizerRight.GetNormalizedPosition());
        AddVectorObs(transformNormalizerRight.GetNormalizedRotation());
    }

    private float CalculateReward(float predicted, float actual)
    {
        //2 (1 - 2.16395 tanh(abs(x)))
        float absDistance = Mathf.Abs(predicted - actual);
        double sigmoid = 2 * (1 - (2.16395  * Math.Tanh(absDistance)));
        //Debug.Log("Pred " + predicted+" Actual "+actual+" Rew "+sigmoid+" Dis "+absDistance);
        return (float) sigmoid;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float reward = CalculateReward(vectorAction[0], playbackReader.GetCurrentState());
        SetReward(reward);
        
        if (reward > 0)
        {
            material.color = new Color(0,reward,0);
        }
        else if(reward < 0)
        {
            material.color = new Color(Mathf.Abs(reward), 0, 0);
        }
        else
        {
            material.color = Color.white;
        }

        expectedText.text = "Expected: "+playbackReader.GetCurrentState();
        guessText.text = "Guess: "+vectorAction[0];
    }

    public override void AgentReset()
    {
        material.color = Color.white;
        transformNormalizerLeft.ClearStats();
        transformNormalizerRight.ClearStats();
        playbackReader.LoadRandomReplayFile();
    }
}
