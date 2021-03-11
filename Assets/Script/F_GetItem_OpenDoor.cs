using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class F_GetItem_OpenDoor : MonoBehaviour
{
    void Start()
    {
        Item.myItem = new GameObject[3];
        Item.arrayIndex = 0;
        Item.inventoryBox = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            Item.inventoryBox[i] = GameObject.Find("ItemImage" + i);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (other.CompareTag("Item"))
            {
                if (Item.arrayIndex == Item.inventoryBox.Length)
                {
                    Debug.Log("아이템 창이 가득 찼습니다");
                }
                else
                {
                    Item.myItem[Item.arrayIndex] = other.gameObject;
                    other.gameObject.SetActive(false);
                    Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = other.GetComponent<Image>().sprite;
                    Item.arrayIndex++;
                }

            }

            if (other.CompareTag("Door"))
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
        }
    }
}
