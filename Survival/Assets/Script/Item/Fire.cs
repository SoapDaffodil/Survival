using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    /* 삭제
   public enum State
    {
        On, Off
    }

    public State state { get; private set;}
    private Light fire;

    private void Start()
    {
        fire = gameObject.GetComponent<Light>();
        SetUp();
        Debug.Log("light 셋업 완료");
    }

    public void SetUp()
    {
        state = State.Off;
        fire.range = 0f;
        fire.type = LightType.Point;
        fire.GetComponent<MeshRenderer>().enabled = false;
    }

    public void Power()
    {
        fire.GetComponent<MeshRenderer>().enabled = true;

        if(state == State.Off)
        {
            fire.range = 7f;
            state = State.On;
        }
        else
        {
            fire.range = 0f;
            state = State.Off;
        }
    }
    */
}
