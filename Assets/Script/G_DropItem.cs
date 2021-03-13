using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class G_DropItem : MonoBehaviour
{

    private void Start()
    {
        Item.myItem = new GameObject[3];
    }
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
                    Item.myItem[Item.arrayIndex - 1].SetActive(true);
                    Item.myItem[Item.arrayIndex - 1].transform.position = transform.position;                    
                }
<<<<<<< HEAD
=======
                else
                {
                    
                if (Item.itemInstance == null)
                    {                        
                        Item.myItem[Item.arrayIndex-1].SetActive(true);
                        Item.myItem[Item.arrayIndex-1].transform.position = transform.position;

                    Debug.Log("버림 : " + Item.myItem[Item.arrayIndex - 1].name + "   index : " + (Item.arrayIndex-1) );
                     }
>>>>>>> 52826d8f7255cf9032802a180b071499a8dcea54

                if (Item.itemInstance != null)
                {
                    Debug.Log("아이템 버릴 수 없음");
<<<<<<< HEAD

                }

=======
                    
                }

                    
                   Item.arrayIndex -= 1;                                
                   Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");
>>>>>>> 52826d8f7255cf9032802a180b071499a8dcea54

                Item.arrayIndex -= 1;
                Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");

            }
        }
    }
}