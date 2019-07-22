using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;
using System.Linq;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("General References")]
    protected NavMeshAgent _agent;
    protected Transform _playerTransform;
    public Transform AttackStartPos;
    public Image HealthBar;

    [Header("Data")]
    public EnemyData Data;
    public EnemyBaseAttack Attack;
    public float Health = 100;

    [Header("etc")]
    public MeshRenderer AttackedMarker;
    public Collider MainCollider;
    protected EnemyManager _enemyMan;
    protected Animator _animator;
    protected AnimationEventCatcher _animCatcher;
    protected Rigidbody _rigid;

    [Header("DEBUG")]
    #region AI Behaviour

    #region StateMachine

    [SerializeField]
    protected State _currentState = State.IDLE;
    protected State _nextState = State.DEFAULT;
    public enum State
    {
        IDLE,
        MOVE,
        ATTACK,
        DEFAULT
    }

    private void Update()
    {
        UpdateAIState();
    }

    public virtual void UpdateAIState()
    {
        if (_currentState == State.IDLE)
        {
            UpdateIdle();
        }
        else if(_currentState == State.MOVE)
        {
            UpdateMovement();
        }
        else if(_currentState == State.ATTACK)
        {
            UpdateAttack();
        }
    }


    #endregion

    #region Idle
    
    private float _updateTicker;

    public virtual void UpdateIdle()
    {

        if (_rotator != null)
            return;

        _animator.SetBool("StartAttack", false);

        if (_updateTicker > Data.IdleTime)
        {

            _updateTicker = 0;

            if(_nextState == State.ATTACK || _nextState == State.DEFAULT)
            {
                _currentState = State.ATTACK;
            }
            else if(_nextState == State.MOVE)
            {
                MoveToRandomLocation();
            }
            _nextState = State.DEFAULT;
        }
        else _updateTicker += Time.deltaTime;
    }

    #endregion

    #region Movement



    [SerializeField]
    protected Vector3 _currentMoveTarget;
    protected bool _applyCurrentMoveTarget;
    protected Tween _rotator;

    public enum MovementType
    {
        Walk,
        Jump,
        Fly
    }

    
    public virtual void UpdateMovement()
    {
        if (!_agent.enabled)
            _agent.enabled = true;

        if(_agent.velocity == Vector3.zero)
        {
            if(_applyCurrentMoveTarget)
            {
                _agent.SetDestination(_currentMoveTarget);
                _agent.isStopped = false;

                _animator.SetBool("Walk", true);
                _applyCurrentMoveTarget = false;
            }
            else
            {   if(_currentMoveTarget == transform.position)
                {
                     _currentState = State.IDLE; //Is this right?!
                    _animator.SetBool("Walk", false);
                    _rotator = transform.DOLookAt(_playerTransform.position, 0.2f, AxisConstraint.Y).OnComplete(()=> _rotator = null);
                }
            }
        }
        else
        {
            if (_agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized);
            }
        }
    }

    public void MoveToRandomLocation()
    { 
        MoveTo(GetRandomLocationOnNavmesh());          //Adjust Y-Pos                 
    }

    public void MoveInAttackRange()
    {
        Vector3 moveTarget;
        while (true)
        {
            moveTarget = GetRandomLocationOnNavmesh();
            if (Vector3.Distance(moveTarget, _playerTransform.position) < Vector3.Distance(transform.position, _playerTransform.position))
            {
                MoveTo(moveTarget);
                break;
            }
        }
    }

    public void ApplyPushBack(Vector3 dir)
    {
        //Debug.Log(dir);
        dir.y = 0;
        //_rigid.AddForce(dir.normalized * Data.PushBackMultiplier, ForceMode.Impulse);
        //_rigid.velocity = dir * Data.PushBackMultiplier;
    }

    #endregion

    public void MoveTo(Vector3 target)
    {
        _currentMoveTarget = target;
        _applyCurrentMoveTarget = true;
        _currentState = State.MOVE;
    }

    #region Attack

    
    [SerializeField]
    protected float _attackTimer;
    [SerializeField]
    protected bool _isAttacking;
    [SerializeField]
    protected int _consecutiveAttackCounter;

    public virtual void UpdateAttack()
    {
        if (_attackTimer > 1 / Data.AttackSpeed)
        {
            if(Vector3.Distance(_playerTransform.position, transform.position) > Data.AttackRange)
            {
                MoveInAttackRange();
                return;
            }

            _attackTimer = 0;
            _consecutiveAttackCounter++;

            _rotator = transform.DOLookAt(_playerTransform.position, 0.2f, AxisConstraint.Y).OnComplete(() => _rotator = null); ;
            _animCatcher.AnimationEvent = () => { Attack.StartAttack(AttackStartPos.position, _playerTransform.position); _animCatcher.AnimationEvent = null; };
            _animator.SetBool("StartAttack", true);

            if (_consecutiveAttackCounter >= Data.MaxConsecutiveAttacks)
            {
                //_animator.SetBool("StartAttack", false);
                _consecutiveAttackCounter = 0;
                _nextState = State.MOVE;
                _currentState = State.IDLE;
                return;
            }
        }
        else
        {
            _animator.SetBool("StartAttack", false);
            _attackTimer += Time.deltaTime;

        }
    }

    

    #endregion

    #endregion


    protected virtual void Awake()
    {
        _enemyMan = FindObjectOfType<EnemyManager>();
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _animCatcher = GetComponentInChildren<AnimationEventCatcher>();
        _rigid = GetComponent<Rigidbody>();
        Health = Data.MaxHealth;
    }

    public void IsBeingAttacked(bool isBeingAttacked)
    {
        AttackedMarker.enabled = isBeingAttacked;
    }

    public void GetDamage(float dmg, Vector3 impactDirection)
    {
        ApplyPushBack(impactDirection);

        Health = Mathf.Clamp(Health - dmg,0,10000);
       

        HealthBar.DOFillAmount(Health / 100, 0.15f);

        if (Health <= 0)
            Die();
    }

    public virtual void Die()
    {
        _currentState = State.DEFAULT;
        _animCatcher.AnimationEvent = null;
        _enemyMan.UnregisterEnemy(this);
        _agent.velocity = Vector3.zero;
        _agent.enabled = false;                
        IsBeingAttacked(false);
        MainCollider.enabled = false;
        Invoke("DestroyThis", 2);
    }

    private void DestroyThis()
    {
       // Destroy(this.gameObject);
    }





    //Maybe put them in helper class
    #region Helpers

    private Vector3 GetRandomLocationOnNavmesh()
    {
        Vector3 randomDir = Random.insideUnitSphere * Data.MoveRange;

        randomDir += transform.position;

        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, Data.MoveRange, 1))
        {
            return hit.position;

        }
        else return Vector3.positiveInfinity;

    }

    #endregion  

}
