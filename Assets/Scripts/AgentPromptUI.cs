using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AgentPromptUI : MonoBehaviour
{
    private TMP_InputField inputField;

    public AgentPromptUIDelegate Delegate { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void OnSendButtonPressed()
    {
        if (this.Delegate != null)
        {
            this.Delegate.SendPrompt(inputField.text);
        }
    }
}

public interface AgentPromptUIDelegate
{
    void SendPrompt(string prompt);
}
