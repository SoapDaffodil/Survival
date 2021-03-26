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

                if (Item.itemOnHand == null)
                {
                    Item.myItem[Item.arrayIndex - 1].GetComponent<MeshRenderer>().enabled = true;
                    Item.myItem[Item.arrayIndex - 1].transform.position = transform.position;                    
                }
                                                
                   Item.arrayIndex -= 1;                                
                   Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");

            }
        }
    }
}