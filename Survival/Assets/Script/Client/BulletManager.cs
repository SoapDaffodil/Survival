using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public int id;

    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Crush()
    {        
        GameManager.bullets.Remove(id);
        Destroy(gameObject);
    }
}
