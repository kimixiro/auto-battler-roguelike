using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameConfig", order = 3)]
public class GameConfig : ScriptableObject
{
    public int maxSlotUnit;
    public int maxSlotMonster;

    public List<string> startPool;
}
