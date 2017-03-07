using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot_Controller : MonoBehaviour
{
    [SerializeField]
    private float _bulletReload;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _canon;
    [SerializeField]
    private GameObject _tower;
    [SerializeField]
    private float _hp = 100;
    private bool _isDeath;
    private NavMeshAgent _agent;
    private float _reload = 1;
    private float dist;

    private float _curTime = 0;
    private float _timeWait = 3;

    private Transform _target;
    private float _dist = 20;
    private float _activAngle = 70f;
    private bool _isTarget;
        
    private Vector3 _move;
    private float _rotate;
    

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    private void FixedUpdate()
    {
        if (dist < _dist)
        {
            if (!CheckBlocked())
            {
                _isTarget = true;
            }

        }
        else _isTarget = false;
        if (_isTarget)
        {
            if (_agent)
            {                
                _agent.SetDestination(_target.position);
                _agent.stoppingDistance = 5f;
                _tower.transform.LookAt(_target.position);
                Shot();
            }            
        }
        GenerationWayPoint();
    }

    void Update()
    {
        if (_isDeath)
        {
            IsDead();
            return;
        }
        dist = Vector3.Distance(transform.position, _target.position);       
        
    }

    private void Shot()
    {        
        if (_reload > _bulletReload)
        {
            GameObject tempbullet = Instantiate(_bullet, _canon.transform.position, _canon.transform.rotation);            
            _reload = 0;
        }
        _reload += Time.deltaTime;
    }

    private void GenerationWayPoint()
    {
        if (!_agent.hasPath && !_isTarget)
        {
            _curTime += Time.deltaTime;
            if (_curTime > _timeWait)
            {
                _curTime = 0;
                Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 30;
                NavMeshHit navMeshHit;
                NavMesh.SamplePosition(gameObject.transform.position + randomPos, out navMeshHit, 40, NavMesh.AllAreas);
                _agent.SetDestination(navMeshHit.position);
            }
        }        
    }
    
    private bool CheckBlocked()
    {
        RaycastHit hit;
        Debug.DrawLine(gameObject.transform.position, _target.position, Color.red);
        if (Physics.Linecast(gameObject.transform.position, _target.position, out hit))
        {
            if (hit.transform == _target)
            {
                return false;
            }
        }
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == "Bullet")
        {            
            ApplyDamage(UnityEngine.Random.Range(20f,40f));
        }        
    }

    public void ApplyDamage(float damage)
    {
        if (_hp > 0)
        {
            _hp -= damage;
        }
        if (_hp <= 0)
        {
            _hp = 0;
            _isDeath = true;
            _isTarget = false;
        }
    }

    private void IsDead()
    {
        _agent.enabled = false;
        foreach (Transform child in transform) // Мега костыль, который решает проблему с разрушением танка.
        {
            child.parent = null;
            if (!child.gameObject.GetComponent<Rigidbody>())
            {
                child.gameObject.AddComponent<Rigidbody>();
            }
            Destroy(child.gameObject, 10);
        }
        foreach (Transform child in transform)
        {
            child.parent = null;
            if (!child.gameObject.GetComponent<Rigidbody>())
            {
                child.gameObject.AddComponent<Rigidbody>();
            }
            Destroy(child.gameObject, 10);
        }
        Destroy(gameObject, 10);
        gameObject.GetComponent<Bot_Controller>().enabled = false;
    }
}