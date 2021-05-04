using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFire : MonoBehaviour
{
    private Fire fire;

    private void Start()
    {
        //fire = GameObject.Find("Fire").GetComponent<Fire>();
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ItemOnHand();
            fire.Power();
            Debug.Log(fire.state);
        }
        */

    }

    void ItemOnHand()
    {
        fire.transform.position = transform.position + new Vector3(-0.7f, 0f, 0f);
    }
}
