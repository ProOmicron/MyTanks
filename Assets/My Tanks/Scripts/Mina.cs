using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : MonoBehaviour {

    public ParticleSystem m_ExplosionParticles;
    private bool _event = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!_event)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Bullet")
            {
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject, 2);
                }                
                m_ExplosionParticles.GetComponent<AudioSource>().Play();                
                m_ExplosionParticles.transform.parent = null;
                m_ExplosionParticles.Play();
                Destroy(gameObject, 0.1f);
                _event = true;
            }
        }           
    }
}
