using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyVerySimpleChatGptAgent : MonoBehaviour, MyVerySimpleSceneEventHandler, BrainAdapterCallback, AgentPromptUIDelegate, RawBrainCallback
{
    private BrainAdapter brainAdapter;
    private Brain brain;
    private List<InteractiveObject> withinVicinityGameObjects = new List<InteractiveObject>();
    private List<InteractiveObject> rightNextToGameObjects = new List<InteractiveObject>();
    private List<InteractiveObject> equippedGameObjects = new List<InteractiveObject>();
    private List<Ability> inherentAbilities = new List<Ability>();
    private InteractiveObject targetInteractiveObject;
    private NavMeshAgent navMeshAgent;
    private AgentPromptUI agentPromptUI;

    public GameObject target; 

    void Awake() {
        brainAdapter = GetComponent<BrainAdapter>();
        brainAdapter.Callback = this;
        brain = GetComponent<GptBrain>();
        brain.RawCallback = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.inherentAbilities.Add(new DoNothing());
        this.agentPromptUI = GameObject.FindGameObjectsWithTag(Tags.AgentPromptUI)[0].GetComponent<AgentPromptUI>();
        this.agentPromptUI.Delegate = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        targetInteractiveObject = target.GetComponent<Cube>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        InteractiveObject interactiveObject = null;
        switch(other.tag)
        {
            case Tags.YellowCube:
                interactiveObject = other.gameObject.GetComponent<Cube>();
                break;
            default:
                break;
        }
        if(interactiveObject != null)
        {
            rightNextToGameObjects.Add(interactiveObject);

            // Remove object from other vicinities
            this.withinVicinityGameObjects.Remove(interactiveObject);
            this.equippedGameObjects.Remove(interactiveObject);

            var context = "Object " + interactiveObject.Identifier + " is right next to you.";
            brainAdapter.RequestInput(context, this.inherentAbilities, this.withinVicinityGameObjects, this.rightNextToGameObjects, this.equippedGameObjects, this);
        }
    }

    public void DidEnterVicinityOfAgent(InteractiveObject gameObject) {
        var context = "Object " + gameObject.Identifier + " is within your vicinity.";
        withinVicinityGameObjects.Add(gameObject);
        brainAdapter.RequestInput(context, this.inherentAbilities, this.withinVicinityGameObjects, this.rightNextToGameObjects, this.equippedGameObjects, this);
    }

    public void PerformAction(Ability ability) {
        Debug.Log("Perform action using ability: " + ability);
        if(ability is MoveToAbility)
        {
            var tagOfTarget = ((MoveToAbility)ability).TagOfTarget;
            var target = GameObject.FindGameObjectWithTag(tagOfTarget);
            if(target != null)
            {
                this.navMeshAgent.destination = target.transform.position;
            }
        }
    }

    public void SendPrompt(string prompt)
    {
        this.brain.SendRawPrompt(prompt);
    }

    public void DidReceiveResponse(string rawResponse)
    {
        Debug.Log("Raw response from Brain: " + rawResponse);
        this.agentPromptUI.Hide();
        DidEnterVicinityOfAgent(targetInteractiveObject);
    }

    public void DidReceiveError(string message)
    {
        // TODO error handling
    }
}

public interface MyVerySimpleSceneEventHandler {
    void DidEnterVicinityOfAgent(InteractiveObject gameObject);
}
