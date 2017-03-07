using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Shot_Controller : MonoBehaviour {

    [SerializeField]
    private float _bulletReload;
    [SerializeField]
    private float _canonForce;
    private float _reload = 1;

    [SerializeField]
    private GameObject[] _bullet;
    [SerializeField]
    private int _bulletCount;
    [SerializeField]
    private GameObject _bulletPos;

    void Start () {
        _bulletPos = GameObject.Find("_bulletPos");
	}
	
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Shot();
        }
        _reload += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_bulletCount < _bullet.Length -1 )
            {
                _bulletCount++;
            }
            else
            {
                _bulletCount = 0;
            }
            
        }

    }
    private void Shot()
    {
        if (_reload > _bulletReload)
        {
            if (_bullet[_bulletCount])
            {
                if (_bulletPos)
                {
                    GameObject tempbullet = Instantiate(_bullet[_bulletCount], _bulletPos.transform.position, _bulletPos.transform.rotation);                    
                    Destroy(tempbullet, 2);
                    _reload = 0;
                }
            }
        }
    }
}
