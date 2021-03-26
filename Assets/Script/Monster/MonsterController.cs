using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private playerItem myItem;

    class playerItem
    {
        private GameObject presentItem = null;
        private M_Light myLight;
        private M_Drone myDrone;

        private UseLight useLight = GameObject.FindObjectOfType<UseLight>();
        public void CheckItem()
        {
            switch (Item.myItem[Item.arrayIndex - 1].name)
            {
                case "Light":
                    myLight = Item.myItem[Item.arrayIndex - 1].GetComponent<M_Light>();
                    useLight.light = myLight;
                    break;

                case "Drone":
                    myDrone = Item.myItem[Item.arrayIndex - 1].GetComponent<M_Drone>();
                   // useEmp.emp = myEMP;
                    break;
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        myItem = new playerItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            myItem.CheckItem();
            Debug.Log("checkItem()");
        }
    }
   



    /*
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
