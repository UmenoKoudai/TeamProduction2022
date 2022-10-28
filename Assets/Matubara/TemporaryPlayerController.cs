using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.forward * v + Vector3.right * h;
        // カメラのローカル座標系を基準に dir を変換する
        dir = Camera.main.transform.TransformDirection(dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        dir.y = 0;
        // 移動の入力がない時は回転させない。入力がある時はその方向にキャラクターを向ける。
        if (dir != Vector3.zero) this.transform.forward = dir;
        // 水平方向（XZ平面上）の速度を計算する
        dir = dir.normalized * _moveSpeed;
        // 垂直方向の速度を計算する
        float y = _rb.velocity.y;
        _rb.velocity = dir * _moveSpeed + Vector3.up * y;
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Fireを呼び出しました");
            FindObjectOfType<WeaponController>().Fire();
        }
    }
}
