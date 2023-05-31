using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, InteractiveObject
{
    public string Identifier { get
        {
            return this.tag;
        }
    }
    public List<Ability> WithinVicinityAbilities { get; private set; }
    public List<Ability> RightNextToAbilities { get; private set; }
    public List<Ability> EquippedAbilities { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        this.WithinVicinityAbilities = new List<Ability>();
        this.RightNextToAbilities = new List<Ability>();
        this.EquippedAbilities = new List<Ability>();
        this.WithinVicinityAbilities.Add(new MoveToAbility(this.gameObject.tag));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
