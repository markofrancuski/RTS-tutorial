using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform _ragdollPrefab;
    [SerializeField] private Transform _ragdollRootBone;

    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDead += HealthSystem_OnDead;

    }
    private void OnDestroy()
    {
        _healthSystem.OnDead -= HealthSystem_OnDead;
    }
    private void HealthSystem_OnDead(object sender, System.EventArgs args)
    {
        Transform ragdollTransform = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(_ragdollRootBone);
    }
}
