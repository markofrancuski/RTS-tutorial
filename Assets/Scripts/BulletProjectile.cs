using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Transform _bulletHitVFXPrefab;

    public void Setup(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        float moveSpeed = 200f;
        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = _targetPosition;
            _trailRenderer.transform.parent = null;
            Destroy(gameObject);

            Instantiate(_bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
