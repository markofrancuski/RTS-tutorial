using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _shootPointTransform;


    private void Awake()
    {
        if(TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs args)
    {
        _animator.SetTrigger("Shoot");
        Transform bulletProjectileTransform = Instantiate(_bulletProjectilePrefab, _shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = args.TargetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = _shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void MoveAction_OnStopMoving(object sender, System.EventArgs args)
    {
        _animator.SetBool("IsWalking", false);
    }

    private void MoveAction_OnStartMoving(object sender, System.EventArgs args)
    {
        _animator.SetBool("IsWalking", true);
    }
}
