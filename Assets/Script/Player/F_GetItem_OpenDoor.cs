using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class F_GetItem_OpenDoor : MonoBehaviour
{
    Item myItem = Item.instance;
    void Start()
    {

        /*
        Item.myItem = new GameObject[3];
        Item.arrayIndex = 0;
        Item.inventoryBox = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            Item.inventoryBox[i] = GameObject.Find("ItemImage" + i);
        }
        */
    }

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (other.CompareTag("Item"))
            {

                if (false)
                {
                    Debug.Log("아이템 창이 가득 찼습니다");
                }
                else
                {
                    myItem.currentItem = other.gameObject;
                    other.gameObject.SetActive(false);

                    myItem.CheckItem(other.gameObject.GetComponent<MonoBehaviour>());

                    
                    /*
                    Item.myItem[Item.arrayIndex] = other.gameObject;

                    Debug.Log("줍는 아이템 : " + Item.myItem[Item.arrayIndex]);

                    other.gameObject.SetActive(false);

                    if(other.GetComponent<Image>() != null)
                    {
                        Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = other.GetComponent<Image>().sprite;
                    }
                    else
                    {
                        Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");
                    }

                    Item.arrayIndex++;
                    Debug.Log("array index : " + Item.arrayIndex);
                    */
                }

            }

            else if (other.CompareTag("Door"))
            {

                if (other.gameObject.transform.rotation.y == 0f)
                {
                    other.gameObject.transform.RotateAround(other.gameObject.transform.GetChild(0).position, Vector3.up, -90f);
                }

                else
                {
                    other.gameObject.transform.RotateAround(other.gameObject.transform.GetChild(0).position, Vector3.up, 90f);
                }

            }

            else if (other.CompareTag("Hide"))
            {
                Debug.Log("은폐!");
                gameObject.transform.position = other.gameObject.transform.position;
            }
        }

        
    }



}

