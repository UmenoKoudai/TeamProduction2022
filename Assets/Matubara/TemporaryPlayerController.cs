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
        // �J�����̃��[�J�����W�n����� dir ��ϊ�����
        dir = Camera.main.transform.TransformDirection(dir);
        // �J�����͎΂߉��Ɍ����Ă���̂ŁAY ���̒l�� 0 �ɂ��āuXZ ���ʏ�̃x�N�g���v�ɂ���
        dir.y = 0;
        // �ړ��̓��͂��Ȃ����͉�]�����Ȃ��B���͂����鎞�͂��̕����ɃL�����N�^�[��������B
        if (dir != Vector3.zero) this.transform.forward = dir;
        // ���������iXZ���ʏ�j�̑��x���v�Z����
        dir = dir.normalized * _moveSpeed;
        // ���������̑��x���v�Z����
        float y = _rb.velocity.y;
        _rb.velocity = dir * _moveSpeed + Vector3.up * y;
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Fire���Ăяo���܂���");
            FindObjectOfType<WeaponController>().Fire();
        }
    }
}
