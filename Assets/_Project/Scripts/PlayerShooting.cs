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
    
    private WaitForSeconds _attackTimer;
    private EnemyManager _enemyMan;
    private Animator _animator;
    private EnemyBehaviour _currentEnemy;


    private void Awake()
    {
        _attackTimer = new WaitForSeconds(1/AttackSpeed);
        _enemyMan = FindObjectOfType<EnemyManager>();
        _animator = GetComponentInChildren<Animator>();

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
                yield return _attackTimer;                    
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
            transform.LookAt(_currentEnemy.transform);
            _animator.SetTrigger("Attack");            
            Invoke("FireAttackFromAnimation", 0.5f);
            _currentEnemy.IsBeingAttacked(true);
        }
        #endregion


    }

    public void FireAttackFromAnimation()
    {
        
            BaseProjectile tempProjectile = Instantiate<GameObject>(ProjectileObj).GetComponent<BaseProjectile>();

            tempProjectile.Prepare(RightHandObj.transform.position, _currentEnemy.transform.position, Damage);
                                
    }
}
