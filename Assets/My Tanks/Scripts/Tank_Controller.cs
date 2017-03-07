using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank_Controller : MonoBehaviour {

    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _rotateSpeedTower;    
    private Text _healthText;    
    private Text _gameOverText;

    private float _move;
    private float _rotate;
    private float _rotateTower;
    private float _TowerAngle;
    private float _hp = 200;

    //*Украл с другой сцены.
    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
    private float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
    
    private Rigidbody _rigidbody;

    [SerializeField]
    private GameObject _tankBody;
    [SerializeField]
    private GameObject _tower;
    

    void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _healthText = GameObject.Find("HealthUI").GetComponent<Text>();
        _gameOverText = GameObject.Find("GameOverUI").GetComponent<Text>();
        _healthText.text = "HEALTH: " + _hp;
        m_MovementAudio = GetComponent<AudioSource>();
        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch; //кэшируем пинч аудиосурса.
        
    }
	
	void FixedUpdate ()
    {
        Move();
        RotateTank();
        RotateTower();        
	}

    private void Move()
    {
        Vector3 movement = transform.forward * _move * _moveSpeed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }

    private void RotateTank()
    {
        float turn = _rotate * _rotateSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
    }

    private void RotateTower()
    {
        _tower.transform.Rotate(Vector3.up, _rotateTower * _rotateSpeedTower * Time.deltaTime, Space.World);
    }

    private void RotateTowerZero()
    {
        _TowerAngle = _tower.transform.rotation.y - _tankBody.transform.rotation.y;
        Debug.Log(_TowerAngle);    
        if (_TowerAngle > 0)
        {
            _tower.transform.Rotate(Vector3.up, -1 * _rotateSpeedTower * Time.deltaTime, Space.World);
        }      
        else
        {
            _tower.transform.Rotate(Vector3.up, _rotateSpeedTower * Time.deltaTime, Space.World);
        }  
    } 

    private void Update()
    {
        _move = Input.GetAxis("Vertical1");
        _rotate = Input.GetAxis("Horizontal1");
        _rotateTower = Input.GetAxis("Horizontal2");
        EngineAudio();
        if (Input.GetKey(KeyCode.DownArrow))
        {
            RotateTowerZero();
        }
    }

    

    private void EngineAudio()
    {
        
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(_move) < 0.1f && Mathf.Abs(_rotate) < 0.1f)
        {            
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ApplyDamage(UnityEngine.Random.Range(20f, 40f));
        }
        _healthText.text = "HEALTH: " + (int)_hp;
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
            _gameOverText.text = "GAME OVER";
            foreach (Transform child in GetComponentInChildren<Transform>())
            {
                child.parent = null;
                Destroy(child.gameObject, 10);
                if (!child.gameObject.GetComponent<Rigidbody>())
                {
                    child.gameObject.AddComponent<Rigidbody>();
                }
            }

            Time.timeScale = 0;
            Destroy(gameObject, 30);
        }
    }

}
