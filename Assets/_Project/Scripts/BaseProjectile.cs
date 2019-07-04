using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [Header("Data")]

    public float Speed;

    private Vector3 _target;

    private bool _active;
    private Vector3 _startPos;
    private float _distance = 0;
    

    private void Awake()
    {
        GetComponentInChildren<ColliderEventCatcher>().TriggerEnter = OnTriggerEnter;
    }

    public void Prepare(Vector3 startPos, EnemyBehaviour target)
    {
        _target = target.transform.position;
        _startPos = startPos;
        transform.position = startPos;
        _target.y = transform.position.y;

        RaycastHit hit;

        if(Physics.Raycast(_startPos, _target - _startPos, out hit, 10000, 1 << 10)){
            _target = hit.point; 
        }

        _active = true;
    }


    
    void Update()
    {
        if (!_active)
            return;
        

        transform.position = Vector3.MoveTowards(transform.position, _target, Speed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyBehaviour>().Die();
            Destroy(this.gameObject);
        }
        else if(other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
