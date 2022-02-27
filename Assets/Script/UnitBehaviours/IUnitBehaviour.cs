using System.Collections.Generic;
using UnityEngine;

namespace Script.UnitBehaviours
{
    public enum StateAI
    {
        Idle,
        Move,
        Attack,
        AttackMagic,
        SpecialAttack,
        Healing,
        Die
    }
    

    public abstract class IUnitBehaviour : MonoBehaviour
    {
        public UnitSystem unitSystem;
        
        public StateAI stateAis;
        public StateAI stateComplit;

        public UnitSystem target;
        public abstract void SpecialAttack();
        public abstract void Update();
        public abstract void StateSwitcher(StateAI stateAI);
    }
}