using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public enum TypeUnit
{
    Melee,
    Range,
    Magic,
    Heal,
}

public enum CreatureCategory
{
    Unity,
    Monster,
}

public enum ClassUnit
{
    None,
    Bow,
    Wizard,
    SwordShield,
    DoubleSword,
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitConfig", order = 2)]
public class UnitConfigScriptableObject : ScriptableObject
{
    public string id;
    public CreatureCategory category;
    public TypeUnit type;
    public ClassUnit ClassUnit;
    public Texture icon;
    public int health;
  //  public int defence;
    public int physicalAttack;
    public int magicAttack;
   // public int magicResistance;
   // public int physicalResistance;
    public int moveSpeed;
    public int attackSpeed;
    //public int dodge;
  //  public int accuracy;
  public float attackDist;

}
