using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
   

 

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame

   



    /* 삭제
    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(Item.itemOnHand.name == "Light")
            {
                ItemOnHand();
            }
        } 
       else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Item.itemOnHand.name == "Drone")
            {
                ItemOnHand();
            }
        }
    }

    void ItemOnHand()
    {
        if(Item.itemOnHand != null)
        {
            Item.itemOnHand.transform.position = transform.position + new Vector3(0.7f, 0f, 0f);
            Item.itemOnHand.gameObject.SetActive(true);
            Item.itemOnHand.transform.SetParent(transform);
        }
    }
    */
}
