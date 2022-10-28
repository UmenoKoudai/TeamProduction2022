using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField, Tooltip("�e/���[�U�[�𔭎˂���n�_")] Transform _muzzle = default;
    [SerializeField, Tooltip("�Ə��̃I�u�W�F�N�g")] Image _crosshair = default;
    [SerializeField, Tooltip("���[�U�[�̎˒�����")] float _shootRange = 50f;
    [SerializeField, Tooltip("�Ə��ő�������^�[�Q�b�g�̃��C���[")] LayerMask _layerMask;
    [SerializeField, Tooltip("���[�U�[��`�����߂� Line Renderer")] LineRenderer _line;
    [SerializeField, Tooltip("�}�K�W���T�C�Y")] int _magazineSize;
    [SerializeField, Tooltip("�A�ˑ��x")] float _firelate;
    [SerializeField, Tooltip("�����[�h����")] float _reloadTime;
    [SerializeField, Tooltip("�V���b�g�K��������ȊO����؂�ւ���")] bool _shotgun = false;
    [Tooltip("����̃_���[�W��")] public float weaponDamage = 10f;
    Vector3 _hitPosition;
    Collider _hitcollider;
    ParticleSystem _particleSystem;
    private void Start()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(_crosshair.rectTransform.position);
        RaycastHit hit = default;
        _hitPosition = _muzzle.transform.position + _muzzle.transform.forward * _shootRange;
        if (Physics.Raycast(ray, out hit, _shootRange, _layerMask))
        {
            _crosshair.color = Color.black;
            _hitPosition = hit.point;
            _hitcollider = hit.collider;
        }
        else
        {
            _crosshair.color = Color.white;
        }
        //Debug.Log($"{_hitPosition.x} {_hitPosition.z}");
    }
    private void FixedUpdate()
    {
        DrawLaser(_muzzle.position);
    }
    public void Fire()
    {
        if (_shotgun)
        {

        }
        else
        {
            if (_hitcollider)
            {
                DrawLaser(_hitPosition);
                Debug.Log($"{_hitPosition}");
            }
            else
            {
                DrawLaser(_muzzle.position);
            }
        }
        _particleSystem.Play();
    }
    void DrawLaser(Vector3 destination)
    {
        Vector3[] positions = { _muzzle.position, destination };   // ���[�U�[�̎n�_�͏�� Muzzle �ɂ���
        _line.positionCount = positions.Length;   // Line ���I�_�Ǝn�_�݂̂ɐ�������
        _line.SetPositions(positions);
    }
}
