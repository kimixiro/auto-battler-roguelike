using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LvlScriptableObject", order = 1)]
public class LvlScriptableObject : ScriptableObject
{
    public int id;
    public List<string> enemyPrefab;
    
    
}
