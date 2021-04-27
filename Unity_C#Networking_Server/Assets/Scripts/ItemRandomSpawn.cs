using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRandomSpawn : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform[] itemSpawnTransform;
    private static int[] randomCount = { 0, 1, 2, 3, 4, 0, 1, 2, 3, 4 };
    // Start is called before the first frame update
    void Awake()
    {
        if (randomCount.Length != itemSpawnTransform.Length)
        {
            Debug.Log($"아이템 위치의 수와 아이템의 수가 맞지 않습니다");
            return;
        }
        for(int i = randomCount.Length; i > 0; i--)
        {
            int itemNumber = Random.Range(0, i - 1);
            Instantiate(itemPrefabs[randomCount[itemNumber]], itemSpawnTransform[itemNumber].position, itemSpawnTransform[itemNumber].rotation).GetComponent<ItemSpawner>().Initialize();
            randomCount[itemNumber] = randomCount[i - 1];
            itemSpawnTransform[itemNumber] = itemSpawnTransform[i - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
