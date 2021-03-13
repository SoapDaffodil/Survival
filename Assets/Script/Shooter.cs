using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public Gun gun;
    public Transform fireTransform;
    private Vector3 aimPoint;


    // Start is called before the first frame update
    private void Start()
    {
        gun.Setup();
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * gun.fireDistance, Color.green);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GunOnHand();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Item.gun != null)
            {
                UpdateAimPoint();
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gun.CheckBettery();
            Reload();
        }

    }

    void GunOnHand()
    {

        if (Item.arrayIndex == 0)
        {
            Debug.Log("소지하고있는 아이템이 없습니다");
        }

        if (Item.gun == null)
        {
            for (int i = 0; i < Item.arrayIndex; i++)
            {
                if (Item.myItem[i].name == "Gun")
                {
                    Item.gun = Item.myItem[i].gameObject;
                }
            }
            Item.gun.transform.position = transform.position + new Vector3(0.7f, 0f, 0f);
            Item.gun.gameObject.SetActive(true);
            Item.gun.transform.SetParent(transform);
        }
    }


    void Shoot()
    {
        gun.Fire(fireTransform.position, aimPoint - fireTransform.position);
    }

    void Reload()
    {
        gun.Reloade();
    }

    void UpdateAimPoint()
    {
        RaycastHit hit;

        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, gun.fireDistance))
        {
            aimPoint = hit.point;
        }
    }

}
