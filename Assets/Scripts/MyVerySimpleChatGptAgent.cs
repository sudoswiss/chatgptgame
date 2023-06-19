using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyVerySimpleChatGptAgent : MonoBehaviour, MyVerySimpleSceneEventHandler, BrainAdapterCallback, AgentPromptUIDelegate
{
    private BrainAdapter brainAdapter;
    private List<InteractiveObject> withinVicinityGameObjects = new List<InteractiveObject>();
    private List<InteractiveObject> rightNextToGameObjects = new List<InteractiveObject>();
    private List<InteractiveObject> equippedGameObjects = new List<InteractiveObject>();
    private List<Ability> inherentAbilities = new List<Ability>();
    private NavMeshAgent navMeshAgent;
    private AgentPromptUI agentPromptUI;

    public List<GameObject> targets = new List<GameObject>();

    void Awake() {
        brainAdapter = GetComponent<BrainAdapter>();
        brainAdapter.Callback = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.inherentAbilities.Add(new DoNothing());
        this.agentPromptUI = GameObject.FindGameObjectsWithTag(Tags.AgentPromptUI)[0].GetComponent<AgentPromptUI>();
        this.agentPromptUI.Delegate = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        InteractiveObject interactiveObject = ObtainInteractiveObject(other);
        if (interactiveObject != null)
        {
            rightNextToGameObjects.Add(interactiveObject);

            // Remove object from other vicinities
            this.withinVicinityGameObjects.Remove(interactiveObject);
            this.equippedGameObjects.Remove(interactiveObject);

            var context = "Object " + interactiveObject.Identifier + " is right next to you.";
            brainAdapter.RequestInput(context, this.inherentAbilities, this.withinVicinityGameObjects, this.rightNextToGameObjects, this.equippedGameObjects, this);
        }
    }

    private InteractiveObject ObtainInteractiveObject(Collider other)
    {
        InteractiveObject interactiveObject = null;
        switch (other.tag)
        {
            case Tags.YellowCube:
                interactiveObject = other.gameObject.GetComponent<Cube>();
                break;
            case Tags.RedCube:
                interactiveObject = other.gameObject.GetComponent<Cube>();
                break;
            case Tags.BlueCube:
                interactiveObject = other.gameObject.GetComponent<Cube>();
                break;
            case Tags.GreenCube:
                interactiveObject = other.gameObject.GetComponent<Cube>();
                break;
            default:
                break;
        }

        return interactiveObject;
    }

    void OnTriggerExit(Collider other)
    {
        InteractiveObject interactiveObject = ObtainInteractiveObject(other);
        if (interactiveObject != null)
        {
            this.withinVicinityGameObjects.Add(interactiveObject);

            // Remove object from other vicinities
            this.rightNextToGameObjects.Remove(interactiveObject);
            this.equippedGameObjects.Remove(interactiveObject);
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
        Debug.Log("Send prompt: " + prompt);
        this.brainAdapter.DefineTask(prompt);
    }

    public void DidAcknowledgeTask()
    {
        this.agentPromptUI.Hide();
        foreach(GameObject target in targets)
        {
            var interactiveObject = target.GetComponent<Cube>();
            withinVicinityGameObjects.Add(interactiveObject);
        }

        var context = "Some objects are within your vicinity.";
        brainAdapter.RequestInput(context, this.inherentAbilities, this.withinVicinityGameObjects, this.rightNextToGameObjects, this.equippedGameObjects, this);
    }
}

public interface MyVerySimpleSceneEventHandler {
    void DidEnterVicinityOfAgent(InteractiveObject gameObject);
}
