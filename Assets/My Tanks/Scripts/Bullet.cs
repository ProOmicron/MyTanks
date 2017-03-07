using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField]
    private float _rebound = 0.8f;
    private bool _event = false;
    [SerializeField]
    private ParticleSystem m_ExplosionParticles;    
    [SerializeField]
    private float _canonForce;
    [SerializeField]
    private Vector3 _angle;
    private Vector3 _angleTemp;

    [SerializeField]
    private GameObject _bulletPos;

    private void Start()
    {
        _angleTemp = gameObject.transform.forward + _angle;
        gameObject.GetComponent<Rigidbody>().AddForce(_angleTemp * _canonForce);        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            if (!_event)
            {
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject,2);
                }                
                m_ExplosionParticles.GetComponent<AudioSource>().Play();
                m_ExplosionParticles.transform.parent = null;
                m_ExplosionParticles.Play();
                Destroy(gameObject, _rebound);
                _event = true;
            }
        }
        
        
    }

}
