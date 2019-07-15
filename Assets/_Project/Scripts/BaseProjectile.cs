using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseProjectile : MonoBehaviour
{
    [Header("References")]
    public Transform MuzzleFlash;
    

    [Header("Data")]
    public float Speed;

    private Vector3 _target;

    private bool _active;
    private Vector3 _startPos;
    private float _distance = 0;
    protected float _dmg;
    

    public virtual void Awake()
    {
        GetComponentInChildren<ColliderEventCatcher>().TriggerEnter = OnTriggerEnter;
    }

    public void Prepare(Vector3 startPos, Vector3 target, float dmg)
    {
        _dmg = dmg;
        _target = target;
        _startPos = startPos;
        transform.position = startPos;
        _target.y = transform.position.y;

        RaycastHit hit;

        if(Physics.Raycast(_startPos, _target - _startPos, out hit, 10000, 1 << 9)){
            _target = hit.point; 
        }

        UnParentMuzzleFlash();

        _active = true;
    }

    public void UnParentMuzzleFlash()
    {
   
        if(MuzzleFlash != null)
            MuzzleFlash.parent = null;
    
    }

    void Update()
    {
        if (!_active)
            return;
        

        transform.position = Vector3.MoveTowards(transform.position, _target, Speed);
    }

    public virtual void OnTriggerEnter(Collider other)
    {

        gameObject.SetActive(false);

        if (MuzzleFlash != null)
            Destroy(MuzzleFlash.gameObject);

        DOVirtual.DelayedCall(0.7f, () => { Destroy(gameObject); });

    }
}
