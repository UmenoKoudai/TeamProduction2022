using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField, Tooltip("弾/レーザーを発射する地点")] Transform _muzzle = default;
    [SerializeField, Tooltip("照準のオブジェクト")] Image _crosshair = default;
    [SerializeField, Tooltip("レーザーの射程距離")] float _shootRange = 50f;
    [SerializeField, Tooltip("照準で捉えられるターゲットのレイヤー")] LayerMask _layerMask;
    [SerializeField, Tooltip("レーザーを描くための Line Renderer")] LineRenderer _line;
    [SerializeField, Tooltip("マガジンサイズ")] int _magazineSize;
    [SerializeField, Tooltip("連射速度")] float _firelate;
    [SerializeField, Tooltip("リロード時間")] float _reloadTime;
    [SerializeField, Tooltip("ショットガンかそれ以外かを切り替える")] bool _shotgun = false;
    [Tooltip("武器のダメージ数")] public float weaponDamage = 10f;
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
        Vector3[] positions = { _muzzle.position, destination };   // レーザーの始点は常に Muzzle にする
        _line.positionCount = positions.Length;   // Line を終点と始点のみに制限する
        _line.SetPositions(positions);
    }
}
