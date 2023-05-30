using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractiveObject {
    List<Ability> WithinVicinityAbilities { get; }
    List<Ability> RightNextToAbilities { get; }
    List<Ability> EquippedAbilities { get; }
}
