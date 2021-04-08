using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    #region temp
    public static GameObject[] inventoryBox;
    public static GameObject[] myItem;
    public static int arrayIndex = 0;
    #endregion

    public static Gun gun;
    public static Drone drone;
    public static List<EMP> empList;
    public static List<M_Light> lightTrapList;
    public GameObject itemOnHand;
    public GameObject currentItem;
    public delegate void GrabItem();

    public enum playerType { PLAYER, MONSTER}

    void Start()
    {
        itemOnHand = null;
        currentItem = null;

        empList = new List<EMP>();
    }

    /*
    public void GrabItem(int number)
    {
        switch(Client.instance.playerType)
        {
            case playerType.PLAYER:
                델리게이트이름[2*number-2];
                break;
            case playerType.MONSTER:
                델리게이트이름[2*number-1];
                break;
        }
    }
   */

    public void CheckItem(MonoBehaviour item)
    {
        switch (item.name)
        {
            case "Gun":
                gun = item.GetComponent<Gun>();
                Debug.Log($"내가 얻은 아이템은 {gun.name} 입니다");
                break;

            case "EMP":
                empList.Add(item.GetComponent<EMP>());
                Debug.Log($"내가 얻은 아이템은 emp 입니다");
                break;

            case "Drone":
                drone = item.GetComponent<Drone>();
                Debug.Log($"내가 얻은 아이템은 {drone.name} 입니다");
                break;

            case "LightTrap":
                Debug.Log($"내가 얻은 아이템은 light 입니다");
                lightTrapList.Add(item.GetComponent<M_Light>());
                break;
        }
    }

    public void EliminateItem(MonoBehaviour item)
    {
        int index = empList.FindIndex(x => x.name == item.name);
        if (index != -1)
        {
            empList.RemoveAt(index);
        }
        else
        {
            index = lightTrapList.FindIndex(x => x.name == item.name);
            if (index != -1)
            {
                lightTrapList.RemoveAt(index);
            }
            else
            {
                if (gun.name == item.name)
                {
                    gun = null;
                }
                else if (drone.name == item.name)
                {
                    drone = null;
                }
                else
                {
                    Debug.Log($"This is not Item");
                }
            }
        }
    }
     
    public void EliminateItem()
    {
        Item.arrayIndex -= 1;
        Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");
    }
}
