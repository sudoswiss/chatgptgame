using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PromptResponseUI : MonoBehaviour
{
    private PromptData promptData;

    public PromptResponseUIDelegate Delegate { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void OnSubmitButtonPressed() {
        var inputField = GetComponentInChildren<TMP_InputField>();
        if (this.Delegate != null) {
            this.Delegate.SendResponse(new ResponseData(inputField.text, this.promptData.PossibleAbilities, this.promptData.PossibleOptions));
        }
    }

    public void ReceivePrompt(PromptData promptData) {
        var promptText = GetComponentInChildren<TMP_Text>();
        this.promptData = promptData;
        promptText.text = promptData.Prompt;
    }
}

public interface PromptResponseUIDelegate {
    void SendResponse(ResponseData responseData);
}
