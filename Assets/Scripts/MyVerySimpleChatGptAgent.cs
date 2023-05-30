using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyVerySimpleChatGptAgent : MonoBehaviour, MyVerySimpleSceneEventHandler, BrainAdapterCallback
{
    private BrainAdapter brainAdapter;
    private List<InteractiveObject> withinVicinityGameObjects = new List<InteractiveObject>();
    private List<InteractiveObject> rightNextToGameObjects = new List<InteractiveObject>();
    private List<InteractiveObject> equippedGameObjects = new List<InteractiveObject>();
    private InteractiveObject targetInteractiveObject;
    private bool didDetectTarget = false;
    private NavMeshAgent navMeshAgent;

    public GameObject target; 

    void Awake() {
        brainAdapter = GetComponent<BrainAdapter>();
        brainAdapter.Callback = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targetInteractiveObject = target.GetComponent<Cube>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!didDetectTarget) {
            didDetectTarget = true;
            DidEnterVicinityOfAgent(targetInteractiveObject);
        }
    }

    public void DidEnterVicinityOfAgent(InteractiveObject gameObject) {
        withinVicinityGameObjects.Add(gameObject);
        brainAdapter.RequestInput(this.withinVicinityGameObjects, this.rightNextToGameObjects, this.equippedGameObjects, this);
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
}

public interface MyVerySimpleSceneEventHandler {
    void DidEnterVicinityOfAgent(InteractiveObject gameObject);
}
