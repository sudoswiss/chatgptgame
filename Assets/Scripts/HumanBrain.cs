using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBrain : MonoBehaviour, Brain, PromptResponseUIDelegate
{
    public BrainCallback Callback { get; set; }
    public PromptResponseUI UI { get; private set; }

    void Awake() {
        this.UI = GameObject.FindGameObjectsWithTag(Tags.PromptResponseUI)[0].GetComponent<PromptResponseUI>();
        this.UI.Delegate = this;
    }

    public void SendPrompt(PromptData promptData) {
        this.UI.ReceivePrompt(promptData);
    }

    public void SendResponse(ResponseData responseData) {
        if(this.Callback != null) {
            this.Callback.DidReceiveResponse(responseData);
        }
    }
}
