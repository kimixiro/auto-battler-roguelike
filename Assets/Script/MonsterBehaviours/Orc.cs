using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Script.UnitBehaviours;
using UnityEngine;
using Random = UnityEngine.Random;

public class Orc : IUnit
{
      private void Awake ()
    {
        eventReceiver.MakeMeleeDamage += HandleMakeMeleeDamage;
    }

    private void OnDestroy()
    {
        eventReceiver.MakeMeleeDamage += HandleMakeMeleeDamage;
    }
    
   void Start()
        {
            fsm = new StateMachine();
            
            StateMachine extractIntel = new StateMachine(needsExitTime: false);
            fsm.AddState("ExtractIntel", extractIntel);
            
            extractIntel.SetStartState("ExtractIntel");

            fsm.AddState("FollowUnit",
                onLogic: (state) =>
                {
                    MoveTowardsUnit(MoveSpeed);
                }
            );
            
            fsm.AddState("AttackUnit",
                onLogic: (state) =>
                {
                    if (targeConfig.Health <= 0)
                    {
                        fsm.RequestStateChange("SearchNewTargetUnit");
                    }
                    Attack();
                }
                );
            
            fsm.AddState("SearchNewTargetUnit",
                onLogic: (state) => SearchNewTarget()
            );
            
            fsm.AddState("DeadUnit",
                onLogic: (state) => Dead()
            );

            fsm.SetStartState("FollowUnit");

            fsm.AddTransition(
                "ExtractIntel",
                "FollowUnit",
                (transition) => DistanceToPlayer() > 3);
            
            fsm.AddTransition(
                "AttackUnit",
                "FollowUnit",
                (transition) => DistanceToPlayer() > 3);
            
            fsm.AddTransition(
                "SearchNewTargetUnit",
                "AttackUnit",
                (transition) => DistanceToPlayer() < 3);

            fsm.AddTransition(
                "FollowUnit",
                "AttackUnit",
                (transition) => DistanceToPlayer() < 3);

            fsm.AddTransition(
                "ExtractIntel",
                "AttackUnit",
                (transition) => DistanceToPlayer() < 3);

            fsm.AddTransitionFromAny( new Transition(
                "",    
                "DeadUnit",
                t => (Health <= 0)
            ));
            
            
            fsm.AddTriggerTransitionFromAny(
                "OnDamage",
                new Transition("", "DeadUnit", t => (Health <= 0))
            );

           

            fsm.Init();
        }

        void Update()
        {
            fsm.OnLogic();
        }

    float DistanceToPlayer()
    {
        if (target == null)
            GetTarget();
        Vector3 player = target.position;
        animator.SetBool("Walk",false);
        return Vector3.Distance(transform.position, player);
    }

    void MoveTowardsUnit(float speed)
    {
        Vector3 player = target.position;
        animator.SetBool("Walk",true);    
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player, speed * Time.deltaTime);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("attack");
    }
    
    void Dead()
    {
        animator.SetTrigger("Die");
        Debug.Log("dead");
        _gameBehaviour.DestroyMonster(this);
    }

    void SearchNewTarget()
    {
        Debug.Log("find");
        GetTarget();
    }
    
    void GetTarget()
    {
        if(!_gameBehaviour.bootFin)
            return;
        
        if (_gameBehaviour.poolUnit.Count.Equals(0)&&!_gameBehaviour.win)
        {
            _gameBehaviour.win = true;
            _gameBehaviour.Defeat.ShowDefeat();
        }
        else
        {
            var searchTarget = _gameBehaviour.poolUnit[Random.Range(0, _gameBehaviour.poolUnit.Count)];
            if (searchTarget.GetComponent<IUnit>().Health > 0)
            {
                target = searchTarget.transform;
                targeConfig = searchTarget.GetComponent<IUnit>();
            }
        }
    }
    
    private void HandleMakeMeleeDamage(AnimationEvent obj)
    {
        target.GetComponent<IUnit>().Damage(PhysicalAttack);
    }
}
