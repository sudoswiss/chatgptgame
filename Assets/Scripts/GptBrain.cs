using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAi.Unity.V1;
using System.Data;

public class GptBrain : MonoBehaviour, Brain {
    // TODO hook up chat gpt API here
    public BrainCallback Callback { get; set; }
    public RawBrainCallback RawCallback { get; set; }

    public void Awake()
    {
        OpenAiChatCompleterV1.Instance.dialogue = new List<OpenAi.Api.V1.MessageV1>();
    }

    public void SendPrompt(PromptData promptData) {
        DoAddToDialogue(promptData.Prompt);
        OpenAiChatCompleterV1.Instance.Complete(
                promptData.Prompt,
                s => {
                    if (Callback != null)
                    {
                        Debug.Log("GptBrain> Did Receive Response from ChatGPT: " + s);
                        var response = new ResponseData(s, promptData.PossibleAbilities, promptData.PossibleOptions);
                        Callback.DidReceiveResponse(response);
                    }
                },
                e => {
                    if (Callback != null)
                    {
                        // TODO implement error handling
                        var errorMessage = $"ERROR: StatusCode: {e.responseCode} - {e.error}";
                        Debug.Log("GptBrain> Did Receive Error from ChatGPT: " + errorMessage);
                    }
                }
            );
    }

    public void SendRawPrompt(string prompt)
    {
        DoAddToDialogue(prompt);
        OpenAiChatCompleterV1.Instance.Complete(
                prompt,
                s => {
                    if(RawCallback != null)
                    {
                        Debug.Log("GptBrain> Did Receive Raw Response from ChatGPT: " + s);
                        RawCallback.DidReceiveResponse(s);
                    }
                },
                e => {
                    if (RawCallback != null)
                    {
                        var errorMessage = $"ERROR: StatusCode: {e.responseCode} - {e.error}";
                        Debug.Log("GptBrain> Did Receive Raw Error from ChatGPT: " + errorMessage);
                        RawCallback.DidReceiveError(errorMessage);
                    }
                }
            );
    }

    private void DoAddToDialogue(string prompt)
    {
        OpenAi.Api.V1.MessageV1 message = new OpenAi.Api.V1.MessageV1();
        message.role = OpenAi.Api.V1.MessageV1.MessageRole.user;
        message.content = prompt;
        OpenAiChatCompleterV1.Instance.dialogue.Add(message);
    }
}

