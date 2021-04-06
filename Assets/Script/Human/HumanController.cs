using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    private playerItem myItem;


    class playerItem
    {
        private GameObject presentItem = null;
        private Gun myGun;
        private EMP myEMP;

        private Shooter shooter = GameObject.FindObjectOfType<Shooter>();
        private UseEMP useEmp = GameObject.FindObjectOfType<UseEMP>();


        public void CheckItem()
        {
            switch (Item.myItem[Item.arrayIndex-1].name)
            {
                case "Gun":
                    myGun = Item.myItem[Item.arrayIndex - 1].GetComponent<Gun>();
                    Debug.Log("총 습득!");
                    shooter.gun = myGun;
                    break;

                case "EMP":
                    myEMP = Item.myItem[Item.arrayIndex - 1].GetComponent<EMP>();
                    useEmp.emp = myEMP;
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
        if(Input.GetKeyDown(KeyCode.F))
        {
            myItem.CheckItem();
        }
    }
    
}
