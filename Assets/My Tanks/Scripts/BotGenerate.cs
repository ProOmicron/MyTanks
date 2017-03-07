using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGenerate : MonoBehaviour
{

    [SerializeField]
    private int _bots;
    private int _botsCount;
    private int _pointsCount = 0;
    [SerializeField]
    private GameObject[] _points;

    [SerializeField]
    private GameObject _bot;

    private void Start()
    {
        BotGeneration(_bots);
    }

    void FixedUpdate()
    {
        if (!GameObject.FindWithTag("Enemy"))
        {
            _bots *= 2;
            BotGeneration(_bots);
        }
    }

    private void BotGeneration(int count)
    {
        if (_bot)
        {
            for (int i = 0; i < count; i++)
            {
                if (_pointsCount < _points.Length)
                {
                    Instantiate(_bot, _points[_pointsCount].transform.position, Quaternion.identity);
                    _pointsCount++;
                }
                else
                {
                    _pointsCount = 0;
                    Instantiate(_bot, _points[_pointsCount].transform.position, Quaternion.identity);
                }
            }
        }
    }
}
