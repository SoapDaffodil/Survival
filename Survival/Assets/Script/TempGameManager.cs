using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempGameManager : MonoBehaviour
{
    public static float seconds;
    public Text timer;
    public bool isGameFinish;
    public GameObject playUI;
    public GameObject endUI;
    public TempGameManager instance;
    public static bool limit;

    public static void SetSeconds(float _sec)
    {
        seconds = _sec;
    }
    public static void StartTime(float _sec)
    {
        limit = true;
        SetSeconds(300f);
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        isGameFinish = false;
        limit = false;
    }

    private void Update()
    {
        if(!isGameFinish)
        {
            if (limit) {
                seconds -= Time.deltaTime;
                if (seconds <= 0f)
                {
                    isGameFinish = true;
                    limit = false;
                    TimeOver();
                    Cursor.visible = true;
                }
                timer.text = string.Format($"{(int)(seconds / 60)} : {(int)((int)seconds % 60)}");
            }
            for(int i = 1; i <= GameManager.players.Count; i++)
            {
                if(GameManager.players[i].hp <= 0)
                {
                    isGameFinish = true;
                    Cursor.visible = true;
                }
            }
            
        }
        else
        {
            playUI.gameObject.SetActive(false);
            endUI.gameObject.SetActive(true);
            for (int i = 1; i <= GameManager.players.Count; i++)
            {
                GameManager.players[i].GetComponent<AudioSource>().clip = GameManager.players[i].endSound;
                GameManager.players[i].GetComponent<AudioSource>().Play();

                if (GameManager.players[i].playerType == PlayerType.HUMAN && GameManager.players[i].hp <= 0)
                {
                    if (GameManager.players[i].id == Client.instance.myId)
                    {
                        endUI.GetComponent<Image>().sprite = UIManager.instance.endImage[(int)UIManager.EndType.DEFEAT];
                    }
                    else
                    {
                        endUI.GetComponent<Image>().sprite = UIManager.instance.endImage[(int)UIManager.EndType.VICTORY];
                    }
                    break;
                }

                if (GameManager.players[i].playerType == PlayerType.CREATURE && GameManager.players[i].hp <= 0)
                {
                    if (GameManager.players[i].id == Client.instance.myId)
                    {
                        endUI.GetComponent<Image>().sprite = UIManager.instance.endImage[(int)UIManager.EndType.DEFEAT];
                    }
                    else
                    {
                        endUI.GetComponent<Image>().sprite = UIManager.instance.endImage[(int)UIManager.EndType.VICTORY];
                    }
                    break;
                }
            }
        }
    }

    public void TimeOver()
    {
        playUI.SetActive(false);
        endUI.SetActive(true);
        /*
        for (int j = 0; j < UIManager.instance.HPBarUI.Length; j++)
        {
            UIManager.instance.HPBarUI[j].gameObject.SetActive(false);
        }
        for (int j = 0; j < UIManager.instance.skillImageUI.Length; j++)
        {
            UIManager.instance.skillImageUI[j].gameObject.SetActive(false);
            UIManager.instance.itemImageUI[j].gameObject.SetActive(false);
            UIManager.instance.itemCountText[j].gameObject.SetActive(false);
        }
        for (int j = 0; j < UIManager.instance.creatureKey.Length; j++)
        {
            UIManager.instance.creatureKey[j].gameObject.SetActive(false);
            UIManager.instance.creaturekeyBackground[j].gameObject.SetActive(false);
        }
        //GameObject.Find("Aim").gameObject.SetActive(false);
        */

        for (int i = 1; i <= GameManager.players.Count; i++)
        {            
            GameManager.players[i].GetComponent<AudioSource>().clip = GameManager.players[i].endSound;
            GameManager.players[i].GetComponent<AudioSource>().Play();

            if (GameManager.players[i].playerType == PlayerType.HUMAN && GameManager.players[i].hp > 0)
            {
                if (GameManager.players[i].id == Client.instance.myId)
                {
                    endUI.GetComponent<Image>().sprite = UIManager.instance.endImage[(int)UIManager.EndType.VICTORY];
                }
                else
                {
                    endUI.GetComponent<Image>().sprite = UIManager.instance.endImage[(int)UIManager.EndType.DEFEAT];
                }
            }
        }
    }
}
