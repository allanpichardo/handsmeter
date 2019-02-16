using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reader : MonoBehaviour
{
    public const string BasePath = "Assets/Recordings/";
    
    public bool playback = false;
    public int playbackIndex = 0;
    public Transform leftHand;
    public Transform rightHand;
    public int maxRecordingFile = 152;

    private List<Observation> observations;
    
    public void InitializeWithRecording(string data)
    {
        observations = new List<Observation>();
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data));  
        DataContractJsonSerializer ser = new DataContractJsonSerializer(observations.GetType());  
        observations = ser.ReadObject(ms) as List<Observation>;  
        ms.Close();  
    }

    private void Start()
    {
        LoadRandomReplayFile();
    }
    
    public void LoadRandomReplayFile()
    {
        playback = false;
        playbackIndex = 0;
        int num = Random.Range(0, maxRecordingFile + 1);
        TextAsset textAsset = Resources.Load<TextAsset>(num.ToString());
        InitializeWithRecording(textAsset.text);
        leftHand.GetComponent<TransformNormalizer>().ClearStats();
        rightHand.GetComponent<TransformNormalizer>().ClearStats();
        playback = true;
    }

    private void FixedUpdate()
    {
        if (playback && playbackIndex < observations.Count)
        {
            Observation observation = observations[playbackIndex];
            
            if (leftHand != null)
            {
                leftHand.localPosition = new Vector3(observation.leftHandPositionX, observation.leftHandPositionY, observation.leftHandPositionZ);
                leftHand.localEulerAngles = new Vector3(observation.leftHandRotationX, observation.leftHandRotationY, observation.leftHandRotationZ);
            }

            if (rightHand != null)
            {
                rightHand.localPosition = new Vector3(observation.rightHandPositionX, observation.rightHandPositionY, observation.rightHandPositionZ);
                rightHand.localEulerAngles = new Vector3(observation.rightHandRotationX, observation.rightHandRotationY, observation.rightHandRotationZ);
            }

            playbackIndex++;
        }

        if (playbackIndex >= observations.Count)
        {
            LoadRandomReplayFile();
        }
    }

    public float GetCurrentState()
    {
        return observations[playbackIndex].state;
    }

    string ReadFromFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        string data = reader.ReadToEnd();
        reader.Close();
        return data;
    }
}