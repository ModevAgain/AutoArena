using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AI_Robot : EnemyBehaviour
{
    [Header("AI Robot")]
    public RobotTelegraph Telegraph;
    public ParticleSystem DieParticles;
    public GameObject RobotObj;
    public bool AttackInProgress;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void UpdateAIState()
    {
        if (AttackInProgress)
            return; 

        base.UpdateAIState();
    }

    public override void UpdateMovement()
    {
        base.UpdateMovement();
    }

    public override void UpdateAttack()
    {
        if (AttackInProgress)
        {
            return;
        }

        if (Vector3.Distance(_playerTransform.position, transform.position) > Data.AttackRange)
        {
            MoveInAttackRange();
            return;
        }

        if(Physics.Raycast(transform.position, _playerTransform.position - transform.position, out RaycastHit hit, Data.AttackRange, 1 << 9))
        {
            MoveInAttackRange();
            return;
        }

        if (_attackTimer > 1 / Data.AttackSpeed)
        {
            if (_consecutiveAttackCounter >= Data.MaxConsecutiveAttacks)
            {
                _consecutiveAttackCounter = 0;
                _nextState = State.MOVE;
                _currentState = State.IDLE;
                AttackInProgress = false;
                return;
            }

            _attackTimer = 0;

            _rotator = transform.DOLookAt(_playerTransform.position, 0.2f, AxisConstraint.Y).OnComplete(() => _rotator = null);

            AttackInProgress = true;
            //_agent.isStopped = true;
            _agent.enabled = false;
            //Get right distance to player
            _animator.SetBool("StartAttack", true);
            _animator.SetBool("EndAttack", false);
            _animCatcher.AnimationEvent = () => { (Attack as RobotAttack).StartAttack(AttackStartPos.position, _playerTransform.position, this); _animCatcher.AnimationEvent = null; };

            _consecutiveAttackCounter++;
        }
        else
        {
            _animator.SetBool("StartAttack", false);
            _attackTimer += Time.deltaTime;

        }
    }

    public void EndRollAttack()
    {
        AttackInProgress = false;
        _animator.SetBool("StartAttack", false);
        _animator.SetBool("EndAttack", true);
    }

    public override void Die()
    {
        base.Die();

        
        DieParticles.Play();
        DOVirtual.DelayedCall(0.1f, () => 
        {
            RobotObj.SetActive(false);
        });
    }
}
