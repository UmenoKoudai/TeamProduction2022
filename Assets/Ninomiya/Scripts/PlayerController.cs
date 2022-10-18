using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     Rigidbody _rb;
    [SerializeField] float _movespeed; //Player�̈ړ����x
    [SerializeField] float _jumppower;//Player�̃W�����v��
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
        if (Input.GetKeyDown(KeyCode.Space) && _canjump) //�W�����v
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
        // �J�����̃��[�J�����W�n����� dir ��ϊ�����
        dir = Camera.main.transform.TransformDirection(dir);
        // �J�����͎΂߉��Ɍ����Ă���̂ŁAY ���̒l�� 0 �ɂ��āuXZ ���ʏ�̃x�N�g���v�ɂ���
        dir.y = 0;
        // �ړ��̓��͂��Ȃ����͉�]�����Ȃ��B���͂����鎞�͂��̕����ɃL�����N�^�[��������B
        if (dir != Vector3.zero) this.transform.forward = dir;
        _rb.velocity = dir.normalized * _movespeed + _rb.velocity.y * Vector3.up;
    }
    private void OnTriggerEnter(Collider other)�@//�ݒu����
    {
        _canjump = true;
        Debug.Log("true�ł�");
    }
    private void OnTriggerExit(Collider other) //�ݒu����
    {
        _canjump = false;
        Debug.Log("false�ł�");
    }
}
