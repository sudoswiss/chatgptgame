using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ability {
    string ToString();
}

public struct MoveToAbility: Ability {
    public string TagOfTarget { get; private set; }

    public MoveToAbility(string tag) {
        this.TagOfTarget = tag;
    }

    public override string ToString () {
        return "Move to " + this.TagOfTarget;
    }

    public interface MoveToAbilityDelegate
    {
        void MoveToTarget(string tag);
    }
}

public struct TurnLeft: Ability {
    public override string ToString () {
        return "Turn left";
    }
}

public struct DoNothing: Ability {
    public override string ToString () {
        return "Do nothing";
    }
}


