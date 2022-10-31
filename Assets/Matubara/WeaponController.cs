using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField, Tooltip("")] Transform _muzzle = default;
    [SerializeField, Tooltip("")] Image _crosshair = default;
    [SerializeField, Tooltip("")] float _shootRange = 50f;
    [SerializeField, Tooltip("")] LayerMask _layerMask;
    [SerializeField, Tooltip("")] LineRenderer _line;
    [SerializeField, Tooltip("")] int _magazineSize;
    [SerializeField, Tooltip("")] float _firelate;
    [SerializeField, Tooltip("")] float _reloadTime;
    [SerializeField, Tooltip("")] bool _shotgun;
    [Tooltip("")] public float WeaponDamege = 10f;
    Vector3 _hitPosition;
    Collider _hitcollider;
    ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
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
        Vector3[] positions = {_muzzle.position, destination};
        _line.positionCount = positions.Length;
        _line.SetPositions(positions);
    }
}
