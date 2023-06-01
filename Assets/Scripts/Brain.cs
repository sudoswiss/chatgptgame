using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PromptData {
    public string Prompt { get; private set; }

    public List<Ability> PossibleAbilities { get; private set; }

    public List<string> PossibleOptions { get; private set; }

    public PromptData(string prompt, List<Ability> abilities, List<string> options) {
        this.Prompt = prompt;
        this.PossibleAbilities = abilities;
        this.PossibleOptions = options;
    }
}

public struct ResponseData {
    public string Response { get; private set; }

    public List<Ability> PossibleAbilities { get; private set; }

    public List<string> PossibleOptions { get; private set; }

    public ResponseData(string response, List<Ability> abilities, List<string> options) {
        this.Response = response;
        this.PossibleAbilities = abilities;
        this.PossibleOptions = options;
    }
}

public interface Brain
{
    BrainCallback Callback { get; set; }

    RawBrainCallback RawCallback { get; set; }

    void SendPrompt(PromptData promptData);

    void SendRawPrompt(string prompt);
}

public interface BrainCallback {
    void DidReceiveResponse(ResponseData responseData);
}

public interface RawBrainCallback
{
    void DidReceiveResponse(string rawResponse);
}
