using System;
using System.Collections;
using System.Collections.Generic;
using Script.UnitBehaviours;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitSystem : MonoBehaviour
{
    [SerializeField]
    private IUnitBehaviour _unitBehaviour;
    [SerializeField]
    private UnitConfigScriptableObject unitConfig;
    [SerializeField] 
    private Animator animator;

    [SerializeField]
    private ParticleSystem _attackParticle;
    [SerializeField]
    private ParticleSystem _dieParticle;

    private GameBehaviour _gameBehaviour;
    
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

    public UnitSystem target;

    private float _timerForNextAttack;

    private bool _dieBool = false;

    public AnimatorEventReceiver eventReceiver;
    
    

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

    private void Awake ()
    {
        eventReceiver.MakeMeleeDamage += HandleMakeMeleeDamage;
    }

    private void OnDestroy()
    {
        eventReceiver.MakeMeleeDamage += HandleMakeMeleeDamage;
    }
    
    public void Fill(GameBehaviour gb)
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
        _gameBehaviour = gb;
        AttackDist = UnitConfig.attackDist;
    }

    private void Update()
    {
        _unitBehaviour.Update();
    }

    public void Move(Vector3 target)
    {
        
        if (Vector3.Distance(transform.position, target) < _attackDist)
        {
            animator.SetBool("Walk",false);
            _unitBehaviour.StateSwitcher(StateAI.Move);
            return;
        }
        else
        {
            animator.SetBool("Walk",true);
            float step =  MoveSpeed * Time.deltaTime;
            var t = RandomPointOnXZCircle(target, _attackDist);
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, t, step);
        }
    }
    
    Vector3 RandomPointOnXZCircle(Vector3 center, float radius) {
        float angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }
    
    public void Attack(UnitSystem target)
    {
        if (target == null)
        {
            _unitBehaviour.StateSwitcher(StateAI.Attack);
            return;
        }
            
        if (target.Health <= 0)
        {
            target.Die();
            _unitBehaviour.StateSwitcher(StateAI.Attack);
            return;
        }

        if (Vector3.Distance(transform.position, target.gameObject.transform.position) > _attackDist)
        {
          _unitBehaviour.StateSwitcher(StateAI.Attack);
          return;
        }
        transform.LookAt(target.gameObject.transform);
        if (_timerForNextAttack > 0)
        {
            _timerForNextAttack  -= Time.deltaTime;
        }
        else if (_timerForNextAttack <=0)
        {
            if (Accuracy>target.Dodge)
            {
               animator.SetTrigger("Attack");
            }
            _timerForNextAttack = AttackSpeed;
        }
    }
    
    public void AttackMagic(UnitSystem target)
    {
        if (target == null)
        {
            _unitBehaviour.StateSwitcher(StateAI.AttackMagic);
            return;
        }
            
        if (target.Health <= 0)
        {
            target.Die();
            _unitBehaviour.StateSwitcher(StateAI.AttackMagic);
            return;
        }

        if (Vector3.Distance(transform.position, target.gameObject.transform.position) > _attackDist)
        {
            _unitBehaviour.StateSwitcher(StateAI.AttackMagic);
            return;
        }
        transform.LookAt(target.gameObject.transform);
        if (_timerForNextAttack > 0)
        {
            _timerForNextAttack  -= Time.deltaTime;
        }
        else if (_timerForNextAttack <=0)
        {
            if (Accuracy>target.Dodge)
            {
                animator.SetTrigger("Attack");
            }
            _timerForNextAttack = AttackSpeed;
        }
    }
    
    public void HandleMakeMeleeDamage(AnimationEvent animationEvent) 
    {
        if (_attackParticle != null)
            _attackParticle.Play();
        
        if(target== null)
            return;

        if (unitConfig.type == TypeUnit.Magic)
        {
            var dam = MagicAttack - target.MagicResistance - target.Defence;
            
            //if (unitConfig.category ==CreatureCategory.Unity)
            //    dam *= Multiplier;
            
            target.Damage(dam,this);  
        }
        else
        {
            var dam = PhysicalAttack - target.PhysicalResistance - target.Defence;
            
         //   if (unitConfig.category ==CreatureCategory.Unity)
           //     dam *= Multiplier;
            
            target.Damage(dam,this);
        }
    }
    
    public void Damage(int dam, UnitSystem unitSystem)
    {
       
        if (_dieBool)
        {
            return;
        }
        
        if (dam <= 0)
        {
            dam = 0;
        }
        if (_unitBehaviour.stateAis == StateAI.Move)
        {
            target = unitSystem;
        }
        Health -= dam;
//        Debug.Log("dam " + dam + " " + this.gameObject.name );
        animator.SetTrigger("Hit");
    }
    
    public void Healing(int heal)
    {
        //Health += heal;
    }
    
    public void SpecialAttack(UnitSystem target)
    {
        _unitBehaviour.SpecialAttack();
       
        _unitBehaviour.StateSwitcher(StateAI.SpecialAttack);
        
    }

    public void Idle()
    {
        if (!_gameBehaviour.bootFin)
        {
            return;
        }
        if (target != null)
        {
            _unitBehaviour.StateSwitcher(StateAI.Idle);
            return;
        }
       
        if (unitConfig.category == CreatureCategory.Unity&&!_gameBehaviour.win)
        {
         //   target = _gameBehaviour.GetMonsterTarget(this.transform);
        }
        if (unitConfig.category == CreatureCategory.Monster&&!_gameBehaviour.win)
        {
         //   target = _gameBehaviour.GetUnitTarget(this.transform);
        }
        _unitBehaviour.target = target;
        _unitBehaviour.StateSwitcher(StateAI.Idle);
    }

    public void Die()
    {
        if (!_dieBool)
        {
            _dieBool = true;
            animator.SetTrigger("Die");
            if (unitConfig.category == CreatureCategory.Unity)
            {
           //     _gameBehaviour.DestroyUnit(this);
            }
            if (unitConfig.category == CreatureCategory.Monster)
            {
          //      _gameBehaviour.DestroyMonster(this);
            }
            
        }
    }
}
