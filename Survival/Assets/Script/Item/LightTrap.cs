using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightTrap : MonoBehaviour
{
    public bool isDetectiveMode = false;
    public int trapId = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(isDetectiveMode)
        {
            if(other.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
            {               
                for(int i = 0; i < ItemSpawner.lightTrapList.Count; i++)
                {
                    if(trapId == ItemSpawner.lightTrapList[i].trap.spawnerId)
                    {
                        Debug.Log($"인간이 {i}번 LightTrap에서 감지 되었습니다");
                    }
                }
                
            }
        }
    }
    /* 게이지바 통해서 설치 안할거면 삭제
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
    */
}



