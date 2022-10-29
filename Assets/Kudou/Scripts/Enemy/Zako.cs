using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

/// <summary>��Ԏア�G�̃N���X�ł�</summary>
public class Zako : MonoBehaviour
{
    /// <summary>����ɂ��邩�ǂ����̔���</summay>
    bool isHighGround = false;
    [Header("����ƂȂ郌�C���[�����w�肵�Ă�������"), SerializeField] LayerMask layerMask;
    [Header("�n�ʂƂȂ郌�C���[�����w�肵�Ă�������"), SerializeField] LayerMask layerMask2;
    /// <summary>NavMeshAgent�R���|�[�l���g�擾</summary>
    NavMeshAgent _nav;
    /// <summary>�^�[�Q�b�g�ƂȂ�Player�̈ʒu</summary>
    Transform _playerPos;
    /// <summary>�SPlayer�̈ʒu�i�[</summary>
    List<GameObject> _allPlayerPos = new List<GameObject>();
    
    [Header("Player�̃^�O�������Ă�������"), SerializeField] string _playerTag;

    /// <summary>
    /// �n�ʂɒ����Ƃ���true�ɂȂ�܂�
    /// </summary>
    bool isRotate;
    private void OnEnable()
    {   
        //�n�ʂɂ��邩����ɂ��邩�̔���
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
        //�V�[������Player�^�O�̃I�u�W�F�N�g��S���擾���ă��X�g�ɕϊ�����
        _allPlayerPos = GameObject.FindGameObjectsWithTag(_playerTag).ToList();
        //��ԋ߂�Player�̈ʒu���擾
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

    /// <summary>���䂩��n�ʂɂ��ǂ蒅�����߂̊֐�</summary>
    void FromHighGroundToGround()
    {
        if (GroundJuge())�@
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
            if (transform.rotation.x >= 0) //���g��transform.rotation��0�ɂȂ�܂Ŏ��s�������܂�
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
    /// <summary>����ɐڒn���Ă��邩�ǂ����̔���</summary>
    /// <returns>true�̎��͐ڒn���Ă��܂�</returns>
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

    /// <summary>�n�ʂɒ������ǂ����̔���</summary>
    /// <returns>true�̎��͒����܂�</returns>
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

  
    /// <summary>���䂩�牺��鎞�̉�]����</summary>
    void TurnRight()
    {
        Quaternion rot = Quaternion.AngleAxis(90, transform.right) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 45 * Time.deltaTime);
    }

    /// <summary>���䂩�牺��Ēn�ʂɒ��������̉�]���� </summary>
    void TurnLeft()
    {
        Quaternion rot = Quaternion.AngleAxis(90, -transform.right) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 40 * Time.deltaTime);
    }
}
