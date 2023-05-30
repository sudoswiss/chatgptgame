using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PromptResponseUI : MonoBehaviour
{
    private TMP_Text promptText;
    private TMP_InputField inputField;
    private PromptData promptData;

    public PromptResponseUIDelegate Delegate { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        promptText = GetComponentInChildren<TMP_Text>();
        inputField = GetComponentInChildren<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSubmitButtonPressed() {
        if(this.Delegate != null) {
            this.Delegate.SendResponse(new ResponseData(inputField.text, this.promptData.PossibleAbilities, this.promptData.PossibleOptions));
        }
    }

    public void ReceivePrompt(PromptData promptData) {
        this.promptData = promptData;
        promptText.text = promptData.Prompt;
    }
}

public interface PromptResponseUIDelegate {
    void SendResponse(ResponseData responseData);
}
