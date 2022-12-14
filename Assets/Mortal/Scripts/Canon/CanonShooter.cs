using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonShooter : MonoBehaviour
{
    [SerializeField] CanonInputManager canonInputManager;
    [SerializeField] CanonValueCalculator canonValueCalculator;
    [SerializeField] CanonRotationController canonRotationController;
    [SerializeField] Transform projectileCollector;

    private int shootMouseButton = 0;
    [SerializeField] float projectileSpeedMultiplier = 0.1f ,projectileBaseSpeed = 0.1f;

    private void Start()
    {
        canonInputManager.OnMouseButtonRelease += CanonInputManager_OnMouseButtonRelease;
    }
    private void Shoot()
    {
        Vector3 ductSpriteVectorRotated = canonRotationController.GetDuctSpriteLengthVectorRotated();
        Vector2 ductEndPosition = canonRotationController.transform.position + ductSpriteVectorRotated;
        Vector2 direction = ductSpriteVectorRotated.normalized;
        float chargeValue = canonValueCalculator.GetChargeValue();
        float projectileSpeed = projectileBaseSpeed + chargeValue * projectileSpeedMultiplier;

        CanonProjectileController newProjectileController = CanonProjectileManager.CreateProjectile(ductEndPosition, projectileSpeed, direction);
        newProjectileController.transform.SetParent(projectileCollector);

        CollisionManager.Instance.AddToCollidableList(newProjectileController);
    }
    private void CanonInputManager_OnMouseButtonRelease(int mouseButton)
    {
        if (mouseButton == shootMouseButton) Shoot();
    }
}
