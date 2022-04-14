using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Script.UnitBehaviours;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wario : IUnit
{
    private void Awake()
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
            onLogic: (state) => { MoveTowardsUnit(MoveSpeed); }
        );

        fsm.AddState("AttackUnit",
            onLogic: (state) =>
            {
            
                Attack();
            }
        );

        fsm.AddState("SearchNewTargetUnit",
            onLogic: (state) =>
            {
             
                SearchNewTarget();
                
            }
        );
        
        fsm.AddState("WinUnit",
            onLogic: (state) => Win()
        );

        fsm.AddState("DeadUnit",
            onLogic: (state) => Dead()
        );

        fsm.SetStartState("FollowUnit");

        fsm.AddTransition(
            "ExtractIntel",
            "FollowUnit",
            (transition) => DistanceToPlayer() > AttackDist);

        fsm.AddTransition(
            "AttackUnit",
            "FollowUnit",
            (transition) => DistanceToPlayer() > AttackDist);

        fsm.AddTransition(
            "AttackUnit",
            "SearchNewTargetUnit",
            (transition) =>target==null);
        
        fsm.AddTransition(
            "SearchNewTargetUnit",
            "ExtractIntel",
            (transition) =>target!=null);
        
        fsm.AddTransition(
            "FollowUnit",
            "AttackUnit",
            (transition) => DistanceToPlayer() < AttackDist);

        fsm.AddTransition(
            "ExtractIntel",
            "AttackUnit",
            (transition) => DistanceToPlayer() < AttackDist);
        
        fsm.AddTransition(
            "WinUnit",
            "ExtractIntel",
            (transition) =>!_gameBehaviour.win);

        fsm.AddTransitionFromAny(new Transition(
            "",
            "DeadUnit",
            t => (Health <= 0)
        ));
        
        fsm.AddTransitionFromAny(new Transition(
            "",
            "WinUnit",
            t => (_gameBehaviour.win)
        ));
        
        fsm.AddTriggerTransitionFromAny(
            "OnDamage",
            new Transition("", "DeadUnit", t => (Health <= 0))
        );
        
        


        fsm.Init();
    }

    void Update()
    {
        Debug.Log(_gameBehaviour.win);
        fsm.OnLogic();
    }

    float DistanceToPlayer()
    {
        if (target == null)
            GetTarget();
        if (target != null)
        {
            Vector3 player = target.position;
            animator.SetBool("Walk", false);
            return Vector3.Distance(transform.position, player);
        }

        return 0;
    }

    void MoveTowardsUnit(float speed)
    {
        Vector3 player = target.position;
        animator.SetBool("Walk", true);
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player, speed * Time.deltaTime);
    }

    void Attack()
    {
        Vector3 player = target.position;
        transform.LookAt(player);
        animator.SetTrigger("Attack");
        Debug.Log("attack");
    }

    void Dead()
    {
        animator.SetTrigger("Die");
        Debug.Log("dead");
        _gameBehaviour.DestroyUnit(this);
    }

    void SearchNewTarget()
    {
        Debug.Log("find");
        FindClosest();
    }

    void Win()
    {
        animator.SetTrigger("Win");
        Debug.Log("Win");
    }

    void GetTarget()
    {
        if (!_gameBehaviour.bootFin)
            return;

        if (_gameBehaviour.poolMonster.Count.Equals(0) && !_gameBehaviour.win)
        {
            _gameBehaviour.win = true;
            _gameBehaviour.SelectUinit.ShowSelectUnit();
        }
        else
        {
            var searchTarget = _gameBehaviour.poolMonster[Random.Range(0, _gameBehaviour.poolMonster.Count)];
            if (searchTarget != null && searchTarget.GetComponent<IUnit>().Health > 0)
            {
                target = searchTarget.transform;
                targeConfig = searchTarget.GetComponent<IUnit>();
            }
        }
    }

    void FindClosest()
    {
        if (!_gameBehaviour.bootFin)
            return;
        
        if (_gameBehaviour.poolMonster.Count.Equals(0) && !_gameBehaviour.win)
        {
            Debug.Log("kek");
            _gameBehaviour.win = true;
            _gameBehaviour.SelectUinit.ShowSelectUnit();
        }
        
        var nearestDist = float.MaxValue;

        if (_gameBehaviour.poolMonster == null) return;
        
        foreach (var mGameObject in _gameBehaviour.poolMonster)
        {
            if (mGameObject == null || !(Vector3.Distance(this.transform.position, mGameObject.transform.position) <
                                         nearestDist)) continue;
            
            var u = mGameObject.GetComponent<IUnit>();
            
            if (u == null || u.Health <= 0) continue;
            
            nearestDist = Vector3.Distance(this.transform.position, mGameObject.transform.position);
            target = mGameObject.transform;
            targeConfig = u;
        }
    }

    private void HandleMakeMeleeDamage(AnimationEvent obj)
    {
        if (target != null) target.GetComponent<IUnit>().Damage(PhysicalAttack);
    }
}