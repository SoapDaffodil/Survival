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

<<<<<<< HEAD


=======
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            myItem.CheckItem();
        }
    }
    
>>>>>>> ed4ea78d44db50ea7759824ea6ade76dbf0b81cc
}
