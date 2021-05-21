using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRandomSpawn : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform[] itemSpawnTransform;
    private static List<int> randomCount = new List<int>();
    // Start is called before the first frame update
    void Awake()
    {   //gun, drone, emp, lighttrap, battery
        int[] itemCount = { 6, 0, 18, 24, 18 };
        for (int i=0;i<itemCount.Length;i++)
        {
            for (int j = 0; j < itemCount[i]; j++)
            {
                randomCount.Add(i);
            }
        }
        itemSpawnTransform = GetComponentsInChildren<Transform>();
        if (randomCount.Count != itemSpawnTransform.Length)
        {
            Debug.Log($"아이템 위치의 수와 아이템의 수가 맞지 않습니다");
            return;
        }
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = randomCount.Count; i > 0; i--)
        {
            int itemNumber = Random.Range(0, i - 1);
            GameObject temp = Instantiate(itemPrefabs[randomCount[itemNumber]], itemSpawnTransform[i-1].position, itemSpawnTransform[i-1].rotation);
            temp.GetComponent<ItemSpawner>().Initialize();
            randomCount[itemNumber] = randomCount[i - 1];
            itemSpawnTransform[itemNumber] = itemSpawnTransform[i - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
