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

    public ItemSpawner item_number1;
    public List<ItemSpawner> item_number2 = new List<ItemSpawner>();

    
    /*
    public void CheckItem(GameObject item)
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
    */
     
    public void EliminateItem()
    {
        Item.arrayIndex -= 1;
        Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");
    }
}
