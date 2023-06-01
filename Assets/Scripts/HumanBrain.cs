using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBrain : MonoBehaviour, Brain, PromptResponseUIDelegate
{
    public BrainCallback Callback { get; set; }
    public RawBrainCallback RawCallback { get; set; }
    public PromptResponseUI UI { get; private set; }

    void Awake() {
        this.UI = GameObject.FindObjectOfType<PromptResponseUI>(true);
        this.UI.Delegate = this;
    }

    public void SendPrompt(PromptData promptData) {
        this.UI.Show();
        this.UI.ReceivePrompt(promptData);
    }

    public void SendResponse(ResponseData responseData) {
        if(this.Callback != null) {
            this.Callback.DidReceiveResponse(responseData);
        }
    }

    public void SendRawPrompt(string prompt)
    {
        if(this.RawCallback != null)
        {
            this.RawCallback.DidReceiveResponse("Ok");
        }
    }
}
