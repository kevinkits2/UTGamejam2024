using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreatureEvents {
 
    public static event Action<CreatureState, Transform> OnCreatureStateChange;
    public static void ChangeCreatureState(CreatureState state, Transform transform) 
        => OnCreatureStateChange?.Invoke(state, transform);
}
