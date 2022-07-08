using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] private Animator _animator;

    private Vector3 _targetPosition;
    private float _stopDistance = 0.1f;

    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _moveSpeed = 4f;


    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(_targetPosition, transform.position) > _stopDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
            
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }


}
