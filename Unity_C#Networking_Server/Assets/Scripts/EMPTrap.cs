using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>EMPTrap</summary>
public class EMPTrap : MonoBehaviour
{
    public static Dictionary<int, EMPTrap> empTraps = new Dictionary<int, EMPTrap>();
    private static int nextEMPTrapId = 1;
    private int byPlayer;

    public int id;

    private void Start()
    {
        id = nextEMPTrapId;
        nextEMPTrapId++;

        ServerSend.SpawnEMPTrap(this, byPlayer);
    }

    public void Initialize(int _byPlayer)
    {
        byPlayer = _byPlayer;
    }
}
