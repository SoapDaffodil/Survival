using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //인간, 몬스터 공통 기능

    private F_GetItem_OpenDoor keyF;
    private G_DropItem keyG;
    private UseEMP keyE;

    void Start()
    {
        keyF = GameObject.FindObjectOfType<F_GetItem_OpenDoor>();
        keyG = GameObject.FindObjectOfType<G_DropItem>();
        keyE = GameObject.FindObjectOfType<UseEMP>();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            
        }
    }
}
