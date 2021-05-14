using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Dictionary<int, Bullet> bullets = new Dictionary<int, Bullet>();
    private static int nextBullet = 1;

    public int id;
    public Rigidbody rigidBody;
    public int thrownByPlayer;
    public Vector3 initialForce;            //발사벡터
    public float damage = 50f;             //총 데미지
    public bool EMPisInstalled;

    private void Start()
    {
        id = nextBullet;
        nextBullet++;
        bullets.Add(id, this);

        ServerSend.SpawnBullet(this, thrownByPlayer);

       rigidBody.velocity = initialForce;
    }

    private void FixedUpdate()
    {
        ServerSend.BulletPosition(this);
    }

    /// <summary>충돌체크 후 데미지</summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        ServerSend.BulletCrush(this);

        if (collision.gameObject.GetComponent<Player>() != null)
        { 
        if (collision.gameObject.GetComponent<Player>().playerType == PlayerType.CREATURE && EMPisInstalled)
            {                
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
                Debug.Log($"명중 : {collision.gameObject.name}");
            }
            if(collision.gameObject.GetComponent<Player>().playerType == PlayerType.CREATURE && !EMPisInstalled)
            {
                ServerSend.KeyChange(collision.gameObject.GetComponent<Player>().id);
                Debug.Log($"키체인지 명중 : {collision.gameObject.name}");
            }
        }        
        bullets.Remove(id);
        Destroy(this.gameObject);

    }

    /// <summary>초기 발사상태</summary>
    /// <param name="_initialMovementDirection">초기 방향</param>
    /// <param name="_initialForceStrength">초기 세기</param>
    /// <param name="_thrownByPlayer">발사한 사람</param>
    public void Initialize(Vector3 _initialMovementDirection, float _initialForceStrength, int _thrownByPlayer, bool _EMPisInstalled)
    {
        initialForce = _initialMovementDirection * _initialForceStrength;
        thrownByPlayer = _thrownByPlayer;
        EMPisInstalled = _EMPisInstalled;
    }    
}
