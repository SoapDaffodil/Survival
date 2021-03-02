using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Q_UseItem : MonoBehaviour
{
    public LayerMask whatIsTarget;
    private float distance = 3f;


    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * distance, Color.green);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ItemOnHand();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(Item.itemInstance != null)
            {
                Debug.Log("작동");
                Use();
            }
        }
    }

    void ItemOnHand()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Item.myItem == null)
            {
                Debug.Log("소지하고있는 아이템이 없습니다");
            }
            else if (Item.itemInstance == null)
            {
                Item.itemInstance = Item.myItem;
                Item.itemInstance.transform.position = transform.position + new Vector3(0.7f, 0f, 0f);
                Item.itemInstance.gameObject.SetActive(true);
                Item.itemInstance.transform.SetParent(transform);
            }
            else
            {
                Debug.Log("이미 아이템을 들고 있습니다");
            }

        }
    }

    //아이템사용 상호작용코드
    void Use()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, whatIsTarget))
        {
            GameObject hitTarget = hit.collider.gameObject;
            
            //상호작용 내용
            Debug.Log(Item.itemInstance.name + "과" + hitTarget.name + " 이 사용되었습니다");

            Destroy(hitTarget);
            Destroy(Item.itemInstance);
            
            Item.inventoryBox[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");
            Item.arrayIndex = 0;
        }
    }
}
