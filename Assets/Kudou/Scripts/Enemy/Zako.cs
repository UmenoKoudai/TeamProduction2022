using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

/// <summary>一番弱い敵のクラスです</summary>
public class Zako : MonoBehaviour
{
    /// <summary>高台にいるかどうかの判定</summay>
    bool isHighGround = false;
    [Header("高台となるレイヤー名を指定してください"), SerializeField] LayerMask layerMask;
    [Header("地面となるレイヤー名を指定してください"), SerializeField] LayerMask layerMask2;
    /// <summary>NavMeshAgentコンポーネント取得</summary>
    NavMeshAgent _nav;
    /// <summary>ターゲットとなるPlayerの位置</summary>
    Transform _playerPos;
    /// <summary>全Playerの位置格納</summary>
    List<GameObject> _allPlayerPos = new List<GameObject>();
    
    [Header("Playerのタグ名を入れてください"), SerializeField] string _playerTag;

    /// <summary>
    /// 地面に着くときにtrueになります
    /// </summary>
    bool isRotate;
    private void OnEnable()
    {   
        //地面にいるか高台にいるかの判定
        if (!HighGroundJuge())
        {
            isHighGround = false;
            _nav = GetComponent<NavMeshAgent>();
            _nav.enabled = true;
        }
        else
        {
            isHighGround = true;
        }
        
    }
    void Start()
    {
        //シーン内のPlayerタグのオブジェクトを全部取得してリストに変換する
        _allPlayerPos = GameObject.FindGameObjectsWithTag(_playerTag).ToList();
        //一番近いPlayerの位置を取得
        Transform targetPlayer = _allPlayerPos.OrderBy(player => Vector3.Distance(player.transform.position, transform.position)).
                                               First().GetComponent<Transform>();
        _playerPos = targetPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHighGround)
        {
            _nav.SetDestination(_playerPos.position);
        }
        else
        {
            FromHighGroundToGround();
        }
    }

    /// <summary>高台から地面にたどり着くための関数</summary>
    void FromHighGroundToGround()
    {
        if (GroundJuge())　
        {
            isRotate = true;
        }
        else if (HighGroundJuge() && !GroundJuge())
        {
            transform.Translate(transform.forward * 3 * Time.deltaTime);
        }
        else if (!isRotate)
        {
            TurnRight();
        }

        if (isRotate)
        {
            if (transform.rotation.x >= 0) //自身のtransform.rotationが0になるまで実行し続けます
            {
                TurnLeft();
            }
            else
            {
                isHighGround = false;
                _nav = GetComponent<NavMeshAgent>();
                _nav.enabled = true;
            }

        }
    }
    /// <summary>高台に接地しているかどうかの判定</summary>
    /// <returns>trueの時は接地しています</returns>
    bool HighGroundJuge()
    { 
        Ray ray = new Ray(transform.position, -transform.up);
        Debug.DrawRay(ray.origin, ray.direction * 1.3f, Color.red, 5);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.3f, layerMask))
        {
            return true;
        }
        return false;
    }

    /// <summary>地面に着くかどうかの判定</summary>
    /// <returns>trueの時は着きます</returns>
    bool GroundJuge()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 1.3f, Color.gray, 5);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.3f,layerMask2))
        {
            return true;
        }
        return false;
    }

  
    /// <summary>高台から下りる時の回転処理</summary>
    void TurnRight()
    {
        Quaternion rot = Quaternion.AngleAxis(90, transform.right) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 45 * Time.deltaTime);
    }

    /// <summary>高台から下りて地面に着いた時の回転処理 </summary>
    void TurnLeft()
    {
        Quaternion rot = Quaternion.AngleAxis(90, -transform.right) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 40 * Time.deltaTime);
    }
}
