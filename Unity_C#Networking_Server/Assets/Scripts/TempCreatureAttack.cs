using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCreatureAttack : MonoBehaviour
{
    Player _player;

    private void Start()
    {
        _player = this.transform.parent.GetComponent<Player>();
    }
    
    IEnumerator Immortal(float _time, Player _player)
    {
        yield return new WaitForSeconds(_time);
        _player.immortal = false;
    }

    public void EndStun()
    {
        _player.controller.enabled = true;
        Debug.Log("괴물 스턴 종료");
    }

    public void OnTriggerStay(Collider other)
    {
        try
        {
            Player player = other.GetComponent<Player>();
            if (player.playerType == PlayerType.HUMAN && !player.immortal && _player.creatureAttack)
            {
                _player.creatureAttack = false;
                Debug.Log($"괴물 공격 성공! : {player.playerType}");
                Debug.Log($"괴물 creatureAttack! : {_player.creatureAttack}");

                player.immortal = true;
                StartCoroutine(Immortal(1f, player));

                player.TakeDamage(50f);
                player.playerMoveSpeed *= 2;
                player.Invoke("SpeedDown", 2f);

                _player.controller.enabled = false;
                Invoke("EndStun", 2f);

                ServerSend.CreatureAttackTrue(_player.id, true);
            }
            else
            {
                Invoke("SetCreatureAttackFalse", 0.2f);
            }
        }
        catch (System.Exception e)
        {
            Invoke("SetCreatureAttackFalse", 0.2f);
            Debug.Log($"괴물 creatureAttack! : {_player.creatureAttack}");
            return;
        }
    }
    private void SetCreatureAttackFalse()
    {
        _player.creatureAttack = false;
    }
}
