using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public Transform fireTransform;
    private Vector3 aimPoint;
    public Gun gun;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }



    }

    void GunOnHand()
    {
        if (gun == null)
        {          
            //Debug.Log($"아이템 창에 있는 총 :  {Item.gun}");
            gun = new Gun();


            gun.transform.position = transform.position + new Vector3(0.7f, 0f, 0f);
            gun.gameObject.SetActive(true);
            gun.transform.SetParent(transform);
        }
        else
        {
            Debug.Log("이미 총을 들고 있습니다");
        }
    }


    public void Shoot()
    {
        if(gun != null)
        {
            //gun.Fire(fireTransform.position, aimPoint - fireTransform.position);
        }
       
    }

    void Reload()
    {
        if (gun != null)
        {
            gun.Reloade();
        }
        
    }

    public void UpdateAimPoint()
    {
        RaycastHit hit;

        Ray ray = new Ray(transform.position, transform.forward);
        /*
        if (Physics.Raycast(ray, out hit, gun.fireDistance))
        {
            aimPoint = hit.point;
        }*/
    }

}
