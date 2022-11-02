using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using DG.Tweening;
public class Boss : MonoBehaviour
{
    bool isSave = true;
    bool isUp = false;
    float d;
    float high;
    [Tooltip("�ŏ����鍂��"), SerializeField] float startHigh = 10;
    [Tooltip("Player����ǂꂭ�炢���ꂽ�ꏊ�Œ��n���邩")] float arrivePoint = 1.5f;
    Rigidbody _rb;
    /// <summary>NavMeshAgent�R���|�[�l���g�擾</summary>
    //NavMeshAgent _nav;
    [Header("�}�~���U���@���Ɍ������čs�����x"),SerializeField] float _downSpeed = 1.5f;
    //[Header("Player����ǂ̂��炢���ꂽ�����܂ŋ}�~�����邩"),SerializeField] float _downDistance = 5f;
    //[Header("�W�����v��"), SerializeField] float _jumpPower = 5f;
    //[Header("�W�����v�������̏d��"), SerializeField] float _gravity;
    [Header("Player�̃^�O�������Ă�������"), SerializeField] string _playerTag;
    /// <summary>�^�[�Q�b�g�ƂȂ�Player�̈ʒu</summary>
    Transform _playerPos;
    /// <summary>�SPlayer�̈ʒu�i�[</summary>
    List<GameObject> _allPlayerPos = new List<GameObject>();
    //[Header("��Ɍ������Ĕ�ԑ��x"), SerializeField] float _upSpeed = 1f;
    Vector3 Pos;
    Vector3 target;
    Vector3 point;
    Quaternion rotate2;
    [SerializeField] float _rotateSpeed = 10;
    void Start()
    {
        Vector3 startPos = transform.position;
        startPos.y += startHigh;
        transform.position = startPos;
        //�V�[������Player�^�O�̃I�u�W�F�N�g��S���擾���ă��X�g�ɕϊ�����
        _allPlayerPos = GameObject.FindGameObjectsWithTag(_playerTag).ToList();
        TargetPlayer();
        _rb = GetComponent<Rigidbody>();
        //_nav = GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //_nav.SetDestination(_playerPos.position);
        DownAttack();
    }

    /// <summary>�W�I�ƂȂ�Player�̈ʒu���擾����֐�</summary>
    void TargetPlayer()
    {
        //��ԋ߂�Player�̈ʒu���擾
        Transform targetPlayer = _allPlayerPos.OrderBy(player => Vector3.Distance(player.transform.position, transform.position)).
                                               First().GetComponent<Transform>();
        _playerPos = targetPlayer;
    }


    void DownAttack()
    {
        if(isSave)
        {
            target = new Vector3(_playerPos.position.x, _playerPos.position.y + 5, _playerPos.position.z);
            isSave = false;
        }
        
        Vector3 direction = target - transform.position;
        Vector3 direction2 = direction.normalized;
        
        if (!isUp)
        {
            transform.rotation = Quaternion.LookRotation(_playerPos.position);
            _rb.velocity = direction2 * _downSpeed;
            
            if(direction.magnitude <= 1)
            {
                rotate2 = transform.rotation * Quaternion.Euler(90,0,0);
                point = Quaternion.AngleAxis(90, -transform.right) * -direction2;
                StartCoroutine(Attack());
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate2, _rotateSpeed);
            Debug.Log(Pos);
            Vector3 velo = target - transform.position;
            if (velo.magnitude < 50)
            {
                _rb.velocity = point * _downSpeed;
            }
            else
            {
                _rb.velocity = Vector3.zero;
            }
        }
    }
    IEnumerator Attack()
    {
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(2f);
        isUp = true;
    }
    void Down()
    {
        Vector3 direction = (_playerPos.position * arrivePoint - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(_playerPos.position);
        if (Mathf.Abs(direction.x) < 10 && Mathf.Abs(direction.y) < 10)
        {
            _rb.velocity = direction * _downSpeed;
        }
    }

    /*void High()
    {
        float d2 = Vector2.Distance(new Vector2(_playerPos.position.x, _playerPos.position.z), new Vector2(transform.position.x, transform.position.z));
        if (isSave)
        {
            d = d2 - 5;
            high = _nav.baseOffset - 0.5f;
            _downSpeed = (high / ( d / (_nav.speed )));
            
            isSave = false;
        }


        _nav.SetDestination(_playerPos.position);
        Debug.Log(_nav.remainingDistance);
        
        if (_nav.baseOffset >= 0.5 && d2 >= 5)
        {
           
            _nav.baseOffset -= _downSpeed * Time.deltaTime;
        }
        else
        {
            _nav.SetDestination(transform.position);
        }
    }*/



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

   
}
