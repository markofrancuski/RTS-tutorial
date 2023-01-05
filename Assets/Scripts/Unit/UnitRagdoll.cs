using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform _ragdollRootBone;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, _ragdollRootBone);

        ApplyExplosionToRagdoll(originalRootBone, 300f, transform.position, 10f);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransforms(clone, cloneChild);
            }
        }
    }
    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if(TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
