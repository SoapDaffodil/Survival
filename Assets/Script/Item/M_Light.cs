using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_Light : MonoBehaviour
{
    private float minGauge = 15f;
    private float maxGauge = 45f;
    private float chargingTime = 2f;
    private Slider lightSlider;
    public Light m_light;

    private float currenGauge;
    private float chargingSpeed;
    public bool finished;
    private int lightAmount = 0;


    private void Start()
    {
        lightSlider = GameObject.Find("LightSlider").GetComponent<Slider>();
        m_light = GetComponent<Light>();
        chargingSpeed = (maxGauge - minGauge) / chargingTime;
        SetUp();
    }

    public void SetUp()
    {
        finished = false;
        currenGauge = minGauge;
        lightSlider.value = minGauge;
        lightSlider.gameObject.SetActive(false);
        m_light.type = LightType.Point;
        m_light.range = 0f;
    }


    public void Install()
    {
        lightSlider.gameObject.SetActive(true);

        if (finished == true || lightAmount == 0)
        {
            currenGauge = minGauge;
            lightSlider.value = currenGauge;
            lightSlider.gameObject.SetActive(false);

            return;
        }

        if (currenGauge >= maxGauge && !finished)
        {
            currenGauge = maxGauge;
            finished = true;
            m_light.range = 7f;
           
            Item.myItem[Item.arrayIndex - 1].SetActive(true);
            Item.myItem[Item.arrayIndex - 1].transform.SetParent(null);
            Item.myItem[Item.arrayIndex - 1].transform.position = GameObject.Find("Monster").transform.position;
            //Item.EliminateItem();
        }

        else if (Input.GetKey(KeyCode.E) && !finished)
        {
            currenGauge = currenGauge + chargingSpeed * Time.deltaTime;
            lightSlider.value = currenGauge;
        }

        else if (Input.GetKeyUp(KeyCode.E))
        {
            currenGauge = minGauge;
            lightSlider.value = minGauge;

            Debug.Log("current : " + currenGauge);
            Debug.Log("slider value : " + lightSlider.value);
        }

        lightAmount = lightAmount - 1;
    }

    public void CheckLight()
    {
        for (int i = 0; i < Item.arrayIndex; i++)
        {
            if (Item.myItem[i].name == "Light")
            {
                lightAmount += 1;
                Debug.Log("light : " + lightAmount);
            }

            Debug.Log("light 가 없습니다");
        }
    }
}



