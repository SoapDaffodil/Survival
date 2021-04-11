using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class F_GetItem_OpenDoor : MonoBehaviour
{
    Item myItem = Item.instance;

    public bool getKeyDownF = false;

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
    void Update()
    {/*
        if (Input.GetKeyDown(KeyCode.F))
        {
            getKeyDownF = true;
        }*/
    }

    public void OnTriggerStay(Collider other)
    {
        if (getKeyDownF)
        {
            getKeyDownF = false;
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

