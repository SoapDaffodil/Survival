using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    private Item myItem;

    // Start is called before the first frame update
    void Start()
    {
        myItem = new Item();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            myItem.CheckItem();
        }
    }
    

}
