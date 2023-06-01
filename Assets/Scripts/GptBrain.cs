using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GptBrain : MonoBehaviour, Brain {
    // TODO hook up chat gpt API here
    public BrainCallback Callback { get; set; }
    public RawBrainCallback RawCallback { get; set; }

    public void SendPrompt(PromptData promptData) {
        // TODO 
    }

    public void SendRawPrompt(string prompt)
    {
        // TODO
    }
}

