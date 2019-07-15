using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Data")]
    public float AttackSpeed;
    public float AttackRange;
    public float Damage;
    public GameObject ProjectileObj;
    public Transform RightHandObj;

    [Header("Controls")]
    public bool Active;
    public bool Attacking;
    
    private float _attackTimer;
    private EnemyManager _enemyMan;
    private Animator _animator;
    private EnemyBehaviour _currentEnemy;
    private PlayerProjectile _lastInstantiatedProjectile;
    private AnimationEventCatcher _animCatcher;


    private void Awake()
    {
        _enemyMan = FindObjectOfType<EnemyManager>();
        _animator = GetComponentInChildren<Animator>();
        _animCatcher = _animator.GetComponent<AnimationEventCatcher>();

    }


    public void Start()
    {
        StartCoroutine(AttackRoutine());
    }


    public IEnumerator AttackRoutine()
    {
        while (Active)
        {            

            while (Attacking)
            {
                Attack();
                while(_attackTimer < AttackSpeed)
                {
                    yield return null;
                    _attackTimer += Time.deltaTime;

                }
                _attackTimer = 0;
                yield return null;
            }
            yield return null;
        }


    }

    public void Attack()
    {
        #region Range & Distance Check

        float distance = 1000;
        EnemyBehaviour enemy = null;

        foreach (var item in _enemyMan.SpawnedEnemies)
        {
            float tempDist = Vector3.Distance(item.transform.position, transform.position);

            if (tempDist < AttackRange)
            {
                if (tempDist < distance)
                {
                    distance = tempDist;
                    enemy = item;
                }
            }
        }

        _currentEnemy = enemy;
        if (_currentEnemy != null)
        {
            //_animator.
            transform.LookAt(_currentEnemy.transform);
            _animator.SetTrigger("Attack");
            _animator.SetFloat("AttackSpeed", 1/AttackSpeed);
            _animCatcher.AnimationEvent = () => { FireAttackFromAnimation(); _animCatcher.AnimationEvent = null; };
            //Invoke("FireAttackFromAnimation", 0.5f);
            _currentEnemy.IsBeingAttacked(true);
        }
        #endregion


    }

    public void FireAttackFromAnimation()
    {

        _lastInstantiatedProjectile = Instantiate<GameObject>(ProjectileObj).GetComponent<PlayerProjectile>();

        _lastInstantiatedProjectile.Prepare(RightHandObj.transform.position, _currentEnemy.transform.position, Damage);
                                
    }

    public void AbortAttack()
    {
        _currentEnemy?.IsBeingAttacked(false);
        CancelInvoke();
    }
}
