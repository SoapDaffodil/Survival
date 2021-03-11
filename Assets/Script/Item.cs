using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static GameObject[] inventoryBox;
    public static GameObject[] myItem;
    public static GameObject itemInstance;
    public static GameObject gun;
    public static int arrayIndex;

    void Start()
    {
        itemInstance = null;
    }


}
