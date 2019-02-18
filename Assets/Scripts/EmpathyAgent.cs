using System;
using MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

        playbackReader.agent = this;
    }

    public override void CollectObservations()
    {
        AddVectorObs(transformNormalizerLeft.GetNormalizedVelocity(leftHand.GetComponent<Rigidbody>().velocity));
        AddVectorObs(transformNormalizerLeft.GetNormalizedPosition(leftHand.transform));
        AddVectorObs(transformNormalizerLeft.GetNormalizedRotation(leftHand.transform));
        AddVectorObs(transformNormalizerRight.GetNormalizedVelocity(rightHand.GetComponent<Rigidbody>().velocity));
        AddVectorObs(transformNormalizerRight.GetNormalizedPosition(rightHand.transform));
        AddVectorObs(transformNormalizerRight.GetNormalizedRotation(rightHand.transform));
    }

    private float CalculateReward(float predicted, float actual)
    {
        float absDistance = Mathf.Abs(predicted - actual);
        double sigmoid = (1 - (2.16395  * Math.Tanh(absDistance)));
        return (float) sigmoid;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float action = Mathf.Clamp(vectorAction[0], -1.0f, 1.0f);
        float reward = CalculateReward(action, playbackReader.GetCurrentState());
        Debug.Log("Reward: "+reward);
        SetReward(reward);
        
        if (reward > 0)
        {
            material.color = new Color(0,reward,0);
            if (reward > 0.99)
            {
                Done();
            }
        }
        else if(reward < 0)
        {
            material.color = new Color(Mathf.Abs(reward), 0, 0);
            if (reward < -0.99f)
            {
                Done();
            }
        }
        else
        {
            material.color = Color.white;
        }
        
        expectedText.text = "Expected: "+playbackReader.GetCurrentState();
        guessText.text = "Guess: "+action;
    }

    public override void AgentReset()
    {
        material.color = Color.white;
        transformNormalizerLeft.ClearStats();
        transformNormalizerRight.ClearStats();
        playbackReader.LoadRandomReplayFile();
    }
}
