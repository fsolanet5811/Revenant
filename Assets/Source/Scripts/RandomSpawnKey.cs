using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnKey : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject engineRoomKey;
    public GameObject bossKey;
    public Transform storageSpawn1, storageSpawn2, storageSpawn3, storageSpawn4;
    public Transform bossSpawn1, bossSpawn2;
    private int engineKeySpawn, bossKeySpawn;
    void Start()
    {
        engineKeySpawn = Random.Range(1,5);
        bossKeySpawn = Random.Range(1, 3);

        if(engineRoomKey != null)
        {
            switch (engineKeySpawn)
            {
                case 1:
                    engineRoomKey.transform.position = storageSpawn1.position;
                    break;
                case 2:
                    engineRoomKey.transform.position = storageSpawn2.position;
                    break;
                case 3:
                    engineRoomKey.transform.position = storageSpawn3.position;
                    break;
                default:
                    engineRoomKey.transform.position = storageSpawn4.position;
                    break;
            }
        }

        if(bossKey != null)
        {
            switch (bossKeySpawn)
            {
                case 1:
                    bossKey.transform.position = bossSpawn1.position;
                    break;
                default:
                    bossKey.transform.position = bossSpawn2.position;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
