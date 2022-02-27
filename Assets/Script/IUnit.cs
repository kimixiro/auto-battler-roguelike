using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IUnit : MonoBehaviour
{
    protected StateMachine fsm;

    public Transform target;
    protected IUnit targeConfig;
    
    [SerializeField]
    protected UnitConfigScriptableObject unitConfig;
    [SerializeField] 
    protected Animator animator;

    [SerializeField]
    protected ParticleSystem _attackParticle;
    [SerializeField]
    protected ParticleSystem _dieParticle;

    protected GameBehaviour _gameBehaviour;
    [SerializeField]
    protected AnimatorEventReceiver eventReceiver;
    
    [SerializeField] 
    private int _health;
    private int _defence;
    private int _physicalAttack;
    private int _magicAttack;
    private int _magicResistance;
    private int _physicalResistance;
    private int _moveSpeed;
    private int _attackSpeed;
    private int _dodge;
    private int _accuracy;
    private float _attackDist;
    private int _multiplier;
    
    #region Public Property
    public int Health
    {
        get => _health;
        set => _health = value;
    }

    public int Defence
    {
        get => _defence;
        set => _defence = value;
    }

    public int PhysicalAttack
    {
        get => _physicalAttack;
        set => _physicalAttack = value;
    }

    public int MagicAttack
    {
        get => _magicAttack;
        set => _magicAttack = value;
    }

    public int MagicResistance
    {
        get => _magicResistance;
        set => _magicResistance = value;
    }

    public int PhysicalResistance
    {
        get => _physicalResistance;
        set => _physicalResistance = value;
    }

    public int MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    public int Dodge
    {
        get => _dodge;
        set => _dodge = value;
    }

    public int Accuracy
    {
        get => _accuracy;
        set => _accuracy = value;
    }

    public UnitConfigScriptableObject UnitConfig
    {
        get => unitConfig;
        set => unitConfig = value;
    }

    public int AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = value;
    }

    public float AttackDist
    {
        get => _attackDist;
        set => _attackDist = value;
    }

    public int Multiplier
    {
        get => _multiplier;
        set => _multiplier = value;
    }

    #endregion

    public void Fill(GameBehaviour gameBehaviour)
    {
        Health = UnitConfig.health;
        // Defence = UnitConfig.defence;
        PhysicalAttack = UnitConfig.physicalAttack;
        MagicAttack = UnitConfig.magicAttack;
        //  MagicResistance = UnitConfig.magicResistance;
        //  PhysicalResistance = UnitConfig.physicalResistance;
        MoveSpeed = UnitConfig.moveSpeed;
        AttackSpeed = UnitConfig.attackSpeed;
        //  Dodge = UnitConfig.dodge;
        //  Accuracy = UnitConfig.accuracy;
        _gameBehaviour = gameBehaviour;
        AttackDist = UnitConfig.attackDist;
    }

    public void Damage(int dam)
    {
        if (dam <= 0)
        {
            dam = 0;
        }
        Health -= dam;
        
        fsm.Trigger("OnDamage");
        animator.SetTrigger("Hit"); 
    }
}
