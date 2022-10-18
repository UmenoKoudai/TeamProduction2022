using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     Rigidbody _rb;
    [SerializeField] float _movespeed; //Playerの移動速度
    [SerializeField] float _jumppower;//Playerのジャンプ力
    bool _canjump = true;
    [SerializeField] int _hp;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float y = _rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space) && _canjump) //ジャンプ
        {
            y = _jumppower;
            _rb.velocity = Vector3.up * _jumppower;
        }
    }
    private void FixedUpdate()
    {
        float _h = Input.GetAxisRaw("Horizontal");
        float _v = Input.GetAxisRaw("Vertical");
        Vector3 dir = Vector3.forward * _v + Vector3.right * _h;
        // カメラのローカル座標系を基準に dir を変換する
        dir = Camera.main.transform.TransformDirection(dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        dir.y = 0;
        // 移動の入力がない時は回転させない。入力がある時はその方向にキャラクターを向ける。
        if (dir != Vector3.zero) this.transform.forward = dir;
        _rb.velocity = dir.normalized * _movespeed + _rb.velocity.y * Vector3.up;
    }
    private void OnTriggerEnter(Collider other)　//設置判定
    {
        _canjump = true;
        Debug.Log("trueです");
    }
    private void OnTriggerExit(Collider other) //設置判定
    {
        _canjump = false;
        Debug.Log("falseです");
    }
}
