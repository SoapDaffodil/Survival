using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class G_DropItem : MonoBehaviour
{
    void Update()
    {
        Drop();
    }

    public void Drop()
    {       
        if (Input.GetKeyDown(KeyCode.G))
        {                                
                if (Item.arrayIndex == 0)
                {
                    Debug.Log("아이템 창이 비었습니다");
                }
                else
                {
                    
                if (Item.itemInstance == null)
                    {                        
                        Item.myItem.SetActive(true);
                        Item.myItem.transform.position = transform.position;
                     }

                if (Item.itemInstance != null)
                {
                    Item.itemInstance.transform.SetParent(null);
                    Item.itemInstance.transform.position = transform.position;
                    Item.itemInstance = null;
                }

                    
                   Item.arrayIndex = 0;                                
                   Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");

                }
            }
    }
}
