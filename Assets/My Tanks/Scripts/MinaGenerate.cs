using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinaGenerate : MonoBehaviour {

    [SerializeField]
    private int _bombCount;
    [SerializeField]
    private GameObject _bomb;

    private void Start()
    {
        BotGeneration(_bombCount);
    }

    private void BotGeneration(int count)
    {
        if (_bomb)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 50;
                randomPos.y = 0;
                Instantiate(_bomb, randomPos, Quaternion.identity);
            }
        }
    }
}
