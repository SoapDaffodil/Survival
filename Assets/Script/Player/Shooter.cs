using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public Gun gun = null;
    public Transform fireTransform;
    private Vector3 aimPoint;

    void Update()
    {
        if(gun != null)
        {
            
            Debug.DrawRay(transform.position, transform.forward * gun.fireDistance, Color.green);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GunOnHand();
                gun.Setup();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                gun.CheckBettery();
                Reload();
            }

            if(Input.GetMouseButtonDown(0))
            {
                UpdateAimPoint();
                Shoot();
            }
        }
        

    }

    void GunOnHand()
    {
        if (gun == null)
        {
            Debug.Log("소지하고있는 아이템이 없습니다");
        }
        else
        {
            gun.transform.position = transform.position + new Vector3(0.7f, 0f, 0f);
            gun.gameObject.SetActive(true);
            gun.transform.SetParent(transform);
        }
    }


    public void Shoot()
    {
        gun.Fire(fireTransform.position, aimPoint - fireTransform.position);
    }

    void Reload()
    {
        gun.Reloade();
    }

    public void UpdateAimPoint()
    {
        RaycastHit hit;

        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, gun.fireDistance))
        {
            aimPoint = hit.point;
        }
    }

}
