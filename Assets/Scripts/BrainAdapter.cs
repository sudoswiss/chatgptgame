using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BrainAdapter : MonoBehaviour, BrainCallback
{
    private Brain brain;

    public BrainAdapterCallback Callback { get; set; }

    void Awake() {
        brain = GetComponent<GptBrain>();
        this.brain.Callback = this;
    }

    public void RequestInput(
        string context,
        List<Ability> inherentAbilities,
        List<InteractiveObject> withinVicinityGameObjects,
        List<InteractiveObject> rightNextToGameObjects,
        List<InteractiveObject> equippedGameObjects,
        BrainAdapterCallback callback) {

        var index = 1;
        var options = new List<string>();
        var abilities = new List<Ability>();

        foreach (Ability ability in inherentAbilities)
        {
            options.Add("[" + index + "] " + ability.ToString());
            abilities.Add(ability);
            index++;
        }
        foreach (InteractiveObject interactiveObject in withinVicinityGameObjects) {
            foreach(Ability ability in interactiveObject.WithinVicinityAbilities) {
                options.Add("[" + index + "] " + ability.ToString());
                abilities.Add(ability);
                index++;
            }
        }
        foreach(InteractiveObject interactiveObject in rightNextToGameObjects) {
            foreach(Ability ability in interactiveObject.RightNextToAbilities) {
                options.Add("[" + index + "] " + ability.ToString());
                abilities.Add(ability);
                index++;
            }
        }
        foreach(InteractiveObject interactiveObject in equippedGameObjects) {
            foreach(Ability ability in interactiveObject.EquippedAbilities) {
                options.Add("[" + index + "] " + ability.ToString());
                abilities.Add(ability);
                index++;
            }
        }

        var contextPreamble = "Again, imagine you are in a hypothetical virtual world. I want you to answer only with the numbers in the square brackets that are presented to you. Given this, ";
        var promptPreamble = "What action do you want to take? Answer only with the number inside the square brackets.";
        var optionsString = "";
        foreach(string option in options) {
            optionsString += option;
        }
        var completePrompt = contextPreamble + " " + context + " " + promptPreamble + "\n" + optionsString;
        Debug.Log("Adapter> Send prompt: " + completePrompt);
        brain.SendPrompt(new PromptData(completePrompt, abilities, options));
    }

    public void DidReceiveResponse(ResponseData responseData) {
        Debug.Log("Adapter> Did receive response: " + responseData.Response);
        Debug.Log("Adapter> Possible options: ");
        foreach(string option in responseData.PossibleOptions)
        {
            Debug.Log(">>>Adapter> " + option);
        }
        
        // e.g. selectedOption = [1] Turn left
        var selectedOption = responseData.PossibleOptions.First(option => option.Contains(responseData.Response) || responseData.Response.Contains(option));
        Debug.Log("Adapter> Selected option" + selectedOption);

        // e.g. optionWithoutIndex = Turn left
        var optionWithoutIndex = ExtractOption(selectedOption);
        var selectedAbility = responseData.PossibleAbilities.First(ability =>
            ability.ToString() == optionWithoutIndex || ability.ToString().Contains(optionWithoutIndex) ||
            optionWithoutIndex.Contains(ability.ToString())
        );
        Debug.Log("Adapter> Selected ability: " + selectedAbility);

        if(this.Callback != null && selectedAbility != null) {
            this.Callback.PerformAction(selectedAbility);
        }
    }

    private string ExtractOption(string optionWithIndex) {
        // Options are always formatted in the following way: [Index] Option text
        var indexOfClosingBracket = optionWithIndex.IndexOf(']');
        var optionWithoutIndex = optionWithIndex.Substring(indexOfClosingBracket + 1);
        return optionWithoutIndex;
    }
}

public interface BrainAdapterCallback {
    void PerformAction(Ability ability);
}
