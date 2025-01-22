using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject maleZombie;
    [SerializeField]
    private GameObject femaleZombie;

    private int SpawnLimit = 5;
    private int Spawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void EnemiesSpawn()
    {
        if (Spawned < SpawnLimit)
        {
            
            Spawned++;
        }
    }
}
