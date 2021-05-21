using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { CREATURE, HUMAN }

public class Player : MonoBehaviour
{
    public int id;                          //id
    public string username;                 //player 이름
    public PlayerType playerType;           //player 타입(괴물,인간)

    public Animator animator;               //player animation
    public CharacterController controller;  //player의 컨트롤러
    public Transform shootOrigin;           //총알방향
    public float gravity = -9.81f;          //중력가속도
    public float moveSpeed = 10f;            //움직임속도
    public float jumpSpeed = 10f;            //점프속도
    public float throwForce = 600f;         //폭탄 던지기속도
    public float hp;                        //체력
    public float maxHp = 100f;              //최대체력
    public int itemAmount = 0;              //아이템 소요개수
    public int maxItemAmount = 100;         //아이템 최대소요개수


    private bool[] inputs;
    private float yVelocity = 0;

    public float firePower = 600f;            //총 발사 파워

    public bool isCreatureAttack = false;
    private Vector3 viewPoint;

    public void Initialize(int _id, PlayerType _playerType)
    {
        maxHp = 100f;
        hp = maxHp;
        itemAmount = 0;
        maxItemAmount = 100;

        moveSpeed = 1f;
        jumpSpeed = 10f;

        id = _id;
        
        playerType = _playerType;
        hp = maxHp;

        inputs = new bool[8];
    }

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        if (controller.enabled)
        {
            //채력이 0이면 움직이지 못함
            if (hp <= 0f)
            {
                return;
            }
            Vector2 _inputDirection = Vector2.zero;
            if (inputs[0])
            {
                _inputDirection.y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.x -= 1;
            }
            if (inputs[3])
            {
                _inputDirection.x += 1;
            }
            Move(_inputDirection, (inputs[0] || inputs[1] || inputs[2] || inputs[3]), inputs[5], inputs[6], inputs[7]);
            if (inputs[7] && playerType == PlayerType.CREATURE)
            {
                StartCoroutine(Wait(1f));
            }
        }
    }
    IEnumerator Wait(float time)
    {
        controller.enabled = false;
        yield return new WaitForSeconds(time);
        controller.enabled = true;
    }

    private void Update()
    {
        Debug.DrawRay(shootOrigin.position + shootOrigin.forward * 0.7f, viewPoint * 3f, Color.green);
    }
    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    /// <summary>받은 데이터를 통해 플레이어의 움직임을 계산(점프 포함)</summary>
    /// <param name="_inputDirection"></param>
    /// <param name="_move"></param>
    private void Move(Vector2 _inputDirection, bool _walk, bool _run, bool _sit, bool _attack)
    {
        /*Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
        //cross : 벡터외적 y축과 forward방향을 외적하면 오른쪽방향의 벡터가 나옴
        Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));*/

        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4]) //점프키
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;

        _moveDirection.y = yVelocity;
        controller.Move(_moveDirection);

        if (_run)
        {
            moveSpeed = 0.5f;
        }
        else if(_sit)
        {
            moveSpeed = 0.125f;
        }
        else
        {
            moveSpeed = 0.25f;
        }
        ServerSend.MotionSit(this.id, _sit);
        ServerSend.MotionAttack(this.id, _attack);
        ServerSend.PlayerPosition(this, _walk, _run);
        ServerSend.PlayerRotation(this);
        
        animator.SetBool("Sit", _sit);
        animator.SetBool("Attack", _attack);
        animator.SetBool("Walk", _walk);
        animator.SetBool("Run", _run);
    }

    /// <summary>실제 총알을 통해 총알방향의 hit판정</summary>
    /// /// <param name="_viewDirection">총알의 방향</param>
    public void Shoot(Vector3 _viewDirection, bool _EMPInstallFinished)
    {
        //if (_EMPInstallFinished)
        //{
            if (hp <= 0f)
            {
                return;
            }

            NetworkManager.instance.InstantiateBullet(shootOrigin).Initialize(_viewDirection, firePower, id, _EMPInstallFinished);
        /*
        if (Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 25f))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                _hit.collider.GetComponent<Player>().TakeDamage(50f);
            }
        }
        */

        // }
        /*
        else 
        {

            if (Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 25f))
            {
                if (_hit.collider.CompareTag("Player"))
                {
                    Player _player = _hit.collider.GetComponent<Player>();
                    if (_player != this && _player.playerType == PlayerType.CREATURE)
                    {
                        ServerSend.KeyChange(id);
                        Debug.Log($"명중 : {_hit.collider.name}");
                    }
                }
            }
            */
        //}
    }

    IEnumerator MotionHit(float time, Player _player)
    {
        ServerSend.MotionHit(_player.id, true);
        _player.animator.SetBool("HitReaction", true);

        yield return new WaitForSeconds(time);

        ServerSend.MotionHit(_player.id, false);
        _player.animator.SetBool("HitReaction", false);
    }

    /// <summary>괴물 공격</summary>
    /// <param name="_viewDirection"> 괴물 공격 방향 </param>
    public void CreatureAttack(Vector3 _viewDirection)
    {
        viewPoint = _viewDirection;
        if(Physics.Raycast(shootOrigin.position + shootOrigin.forward * 0.7f, _viewDirection, out RaycastHit _hit, 10f))
        {
            if(_hit.collider.GetComponent<Player>() != null)
            {
                Debug.Log($"레이에 맞은 플레이어 : {_hit.collider.gameObject.name}");
                Player hitPlayer = _hit.collider.GetComponent<Player>();
                if (hitPlayer.playerType == PlayerType.HUMAN)
                {
                    StartCoroutine(MotionHit(1f, hitPlayer));
                    isCreatureAttack = true;

                    Debug.Log($"공격 맞음 : {_hit.collider.gameObject.name}");
                    hitPlayer.TakeDamage(50f);
                    hitPlayer.moveSpeed *= 2;
                    hitPlayer.Invoke("SpeedDown", 2f);

                    this.gameObject.GetComponent<Player>().controller.enabled = false;
                    Invoke("EndStun", 2f);
                    //스킬 비활성화
                    ServerSend.CreatureAttackTrue(id, isCreatureAttack);
                }
            }
        }
    }

    /// <summary>투척</summary>
    /// <param name="_viewDirection"></param>
    public void ThrowItem(Vector3 _viewDirection)
    {
        if (hp <= 0f)
        {
            return;
        }

        //if (itemAmount > 0)
       // {
            //itemAmount--;
            NetworkManager.instance.InstantiateProjectile(shootOrigin).Initialize(_viewDirection, throwForce, id);
       // }
    }

    /// <summary>damage 적용 후 클라이언트에 전송</summary>
    public void TakeDamage(float _damage)
    {
        //죽은 적을 맞췄을 때 그냥 반환
        if (hp <= 0f)
        {
            return;
        }

        hp -= _damage;
        //맞춘 적의 hp가 0일 때 해당 object를 정지하고 코루틴을 통해 리스폰실행
        if (hp <= 0f)
        {
            hp = 0f;
            for (int i=1;i<= Server.clients.Count;i++)
            { 
                try
                {
                    Debug.Log($"Server.clients[{i}].player : {Server.clients[i].player.name}");
                    Server.clients[i].player.controller.enabled = false;
                }catch(Exception e)
                {
                    break;
                }
                              
            }
            //controller.enabled = false;
            transform.position = new Vector3(0f, 25f, 0f);
            ServerSend.PlayerPosition(this, false, false);

            ServerSend.MotionDie(id, true);
            animator.SetBool("Die", true);

            //리스폰 불필요 > 그냥 사망 게임종료로 수정해야함
            //StartCoroutine(Respawn());
        }

        ServerSend.PlayerHP(this);
    }

    /// <summary>리스폰</summary>
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        hp = maxHp;
        controller.enabled = true;
        ServerSend.PlayerRespawned(this);
    }

    /// <summary>아이템 소요개수 증가(최대소요개수 체크)</summary>
    /// <returns></returns>
    public bool AttemptPickupItem()
    {
        if (itemAmount >= maxItemAmount)
        {
            return false;
        }

        itemAmount++;
        return true;
    }

    /// <summary>아이템 소요개수 감소(0개인지 체크)</summary>
    /// <returns></returns>
    public bool AttemptThrowItem()
    {
        if (itemAmount <= 0 )
        {
            return false;
        }

        itemAmount--;
        return true;
    }

    public void Cure(float _playerHp)
    {
        hp = _playerHp;

        if(hp + 50f <= maxHp)
        {
            hp += 50f;
        }     

        ServerSend.PlayerHP(this);
    }

    /// <summary>은폐</summary>
    /// <param name="_hidePosition">숨을 포지션</param>
    public void Hide(Vector3 _hidePosition)
    {
        controller.enabled = false;
        controller.transform.position = _hidePosition;
        controller.enabled = true;
    }

    IEnumerator Teleportationing(float time, Vector3 _target)
    {
        ServerSend.MotionTeleportation(id, true);
        animator.SetBool("SkillTeleportation", true);

        yield return new WaitForSeconds(time);
        controller.enabled = false;
        controller.transform.position = _target;
        if (this.GetComponentInChildren<Drone>() != null && this.GetComponentInChildren<Drone>().controller.enabled)
        {
            Drone _drone = this.GetComponentInChildren<Drone>();
            _drone.transform.SetParent(null, true);
            controller.transform.rotation = _drone.transform.rotation;
            Destroy(_drone.gameObject, 1f);
        }
        controller.enabled = true;

        ServerSend.MotionTeleportation(id, false);
        animator.SetBool("SkillTeleportation", false);
    }

    /// <summary>스킬 순간이동</summary>
    /// <param name="_target">target 포지션</param>
    public void Teleportation(Vector3 _target)
    {
        StartCoroutine(Teleportationing(1f, _target));
    }

    public void SpeedUp()
    {
        moveSpeed *= 2;
        Debug.Log("이속 증가 중");
        Invoke("SpeedDown", 5f);                 
    }

    public void SpeedDown()
    {
        moveSpeed /= 2;
        Debug.Log("이속 원상복귀");
    }

    public void EndStun()
    {
       controller.enabled = true;
       Debug.Log("괴물 스턴 종료");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EMP" && playerType == PlayerType.CREATURE && !other.GetComponent<ItemSpawner>().hasItem)
        {
            ServerSend.KeyChange(id);
            /*int _floor = (this.transform.position.y > 8f) ? 2 : 1;
            ItemSpawner.empTrapList.Remove(new ItemSpawner.TrapInfo(_floor, other.GetComponent<ItemSpawner>()));*/
        }
    }
}