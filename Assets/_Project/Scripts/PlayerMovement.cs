using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private PlayerShooting _playerShooting;
    private NavMeshAgent _agent;
    private Vector3 _target;
    private Vector3 _direction;
    private Animator _animator;

    public GameObject Crater;
    private bool _touchedGround;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerShooting = GetComponent<PlayerShooting>();
        _animator = GetComponentInChildren<Animator>();
        _agent.updateRotation = false;
        Crater.SetActive(false);

        StartCoroutine(CustomStartFall());
    }

    private void Start()
    {
        //DOVirtual.DelayedCall(3f, () => Crater.SetActive(false));
    }

    public void Update()
    {
        
        if (_agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized);
        }


        if (Input.GetMouseButtonDown(0))
        {
            MoveVector3(Input.mousePosition);
        }

        if (_agent.velocity != Vector3.zero)
        {

            _playerShooting.Attacking = false;
            _animator.SetBool("Walk", true);

        }
        else
        {
            _animator.SetBool("Walk", false);
            _playerShooting.Attacking = true;
        }

    }


    public void MoveVector3(Vector3 inputPos)
    {

        Ray ray = Camera.main.ScreenPointToRay(inputPos);

        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 1000, 1 << 9))
        {

            _target = hit.point;

            _direction = (_target - transform.position).normalized;
        }

        _direction = (_target - transform.position).normalized;


        NavMeshHit navHit;

        NavMesh.SamplePosition(_target, out navHit, 1000, NavMesh.AllAreas);

        _agent.SetDestination(navHit.position);

    }

    public IEnumerator CustomStartFall()
    {

        while (!_touchedGround)
            yield return null;

        _animator.SetBool("Landed", true);

        FindObjectOfType<CameraManager>().OnFinishedFalling();
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _touchedGround = true;
            GetComponent<Rigidbody>().isKinematic = true;
            Crater.GetComponent<CraterFade>().Fade();
            _agent.enabled = true;
            Crater.SetActive(true);

        }
    }

    
}
