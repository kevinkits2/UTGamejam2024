using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreatureEvents {
 
    public static event Action<CreatureState, Transform> OnCreatureStateChange;
    public static void ChangeCreatureState(CreatureState state, Transform transform) 
        => OnCreatureStateChange?.Invoke(state, transform);

    public static event Action<Vector3, CreatureState> OnCreatureDeath;
    public static void CreatureDeath(Vector3 pos, CreatureState state) => OnCreatureDeath.Invoke(pos, state);

    public static event Action<int> OnGeneratePoints;
    public static void GeneratePoints(int points) => OnGeneratePoints?.Invoke(points);
}
