using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public static GameObject[] inventoryBox;
    public static GameObject[] myItem;
    public static GameObject itemOnHand;
    public static int arrayIndex = 0;

    void Start()
    {
        itemOnHand = null;
    }

    public static void EliminateItem()
    {
        Item.arrayIndex -= 1;
        Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");
    }

}
