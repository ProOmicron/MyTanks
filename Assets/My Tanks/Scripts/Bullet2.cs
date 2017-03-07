using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : MonoBehaviour {

    private float _rebound = 0.8f;
    private bool _event = false;
    private bool _eventChild = false;
    [SerializeField]
    private ParticleSystem m_ExplosionParticles;
    [SerializeField]
    private float _canonForce;
    [SerializeField]
    private Vector3 _angle;
    private Vector3 _angleTemp;
    [SerializeField]
    private GameObject _bulletPos;

    [SerializeField]
    private float _waitForGenerate;
    private float _time;
    [SerializeField]
    private GameObject _childBullets;    

    private void Start()
    {
        _angleTemp = gameObject.transform.forward + _angle;
        gameObject.GetComponent<Rigidbody>().AddForce(_angleTemp * _canonForce);        
    }

    private void Update()
    {
        GenerateChild();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_event)
        {
            m_ExplosionParticles.GetComponent<AudioSource>().Play();
            m_ExplosionParticles.transform.parent = null;
            m_ExplosionParticles.Play();
            Destroy(gameObject, _rebound);
            _event = true;
        }
    }

    private void GenerateChild()
    {
        if (!_eventChild)
        {
            
            if (_time > _waitForGenerate)
            {
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject, 2);
                }
                Vector3 childBulletPos = gameObject.transform.position;
                Quaternion childBulletRot = gameObject.transform.rotation;
                for (int i = 0; i < 10; i++)
                {
                    Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 2;
                    GameObject tempBullet = Instantiate(_childBullets, childBulletPos + randomPos, childBulletRot);
                    tempBullet.transform.parent = null;
                }
                m_ExplosionParticles.transform.parent = null;
                m_ExplosionParticles.Play();
                _eventChild = true;
                Destroy(gameObject);
            }
            _time += Time.deltaTime;
        }
        
    }
}
