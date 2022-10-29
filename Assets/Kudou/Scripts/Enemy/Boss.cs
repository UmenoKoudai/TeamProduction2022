using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using DG.Tweening;
public class Boss : MonoBehaviour
{
    bool jump;
    bool Action;
    Rigidbody _rb;
    float time;
    /// <summary>NavMeshAgentコンポーネント取得</summary>
    NavMeshAgent _nav;
    /// <summary>ターゲットとなるPlayerの位置</summary>
    Transform _playerPos;
    /// <summary>全Playerの位置格納</summary>
    List<GameObject> _allPlayerPos = new List<GameObject>();
    [Header("上に向かって飛ぶ速度"), SerializeField] float _upSpeed = 1f;
    [Header("急降下攻撃　下に向かって行く速度"),SerializeField] float _downSpeed = 1.5f;
    [Header("Playerからどのくらい離れた距離まで急降下するか"),SerializeField] float _downDistance = 5f;
    [Header("ジャンプ力"), SerializeField] float _jumpPower = 5f;
    [Header("ジャンプした時の重力"), SerializeField] float _gravity;
    [Header("Playerのタグ名を入れてください"), SerializeField] string _playerTag;
    void Start()
    {
        //シーン内のPlayerタグのオブジェクトを全部取得してリストに変換する
        _allPlayerPos = GameObject.FindGameObjectsWithTag(_playerTag).ToList();
        TargetPlayer();
        _nav = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
       Action = true;
    }

    // Update is called once per frame
    void Update()
    {
        //_nav.SetDestination(_playerPos.position);
        if (Action)
        {
            Active();
            if(GroundJuge())
            {
                Action = false;
            }
        }
        else
        {
            time += Time.deltaTime;
            if (time >= 10)
            {
                _nav.SetDestination(_playerPos.position);
                High();
            }
            else
            {
                RotateAround();
            }
        }
    }

    /// <summary>標的となるPlayerの位置を取得する関数</summary>
    void TargetPlayer()
    {
        //一番近いPlayerの位置を取得
        Transform targetPlayer = _allPlayerPos.OrderBy(player => Vector3.Distance(player.transform.position, transform.position)).
                                               First().GetComponent<Transform>();
        _playerPos = targetPlayer;
    }

    void High()
    {
        float d = Vector2.Distance(new Vector2(_playerPos.position.x, _playerPos.position.z), new Vector2(transform.position.x, transform.position.z));
        if (_nav.baseOffset >= 1)
        {
            if (d >= _downDistance)
            {
                _nav.baseOffset -= _downSpeed * Time.deltaTime;
            }
            else if (d >= _nav.stoppingDistance + 2)
            {
                _nav.baseOffset -= 0.1f * Time.deltaTime;
            }
        }
    }

    void Jump()
    {
        if (jump)
        {
            _nav.baseOffset += _jumpPower * Time.deltaTime;
        }
        else
        {
            _nav.baseOffset -= _gravity * Time.deltaTime;
        }

        if(_nav.baseOffset >= 3)
        {
            jump = false;
        }
        else if(_nav.baseOffset <= 0.5)
        {
            jump = true;
        }
        
    }
    void RotateAround()
    {
        transform.RotateAround(new Vector3(0,0,0), Vector3.up, 30 * Time.deltaTime);
    }
    void Up()
    { 
        _nav.baseOffset += _upSpeed * Time.deltaTime;
    }

    bool GroundJuge()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.gray, 5);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f))
        {
            return true;
        }
        return false;
    }

    void Active()
    {
        _nav.baseOffset -= _downSpeed * Time.deltaTime;
    }
}
