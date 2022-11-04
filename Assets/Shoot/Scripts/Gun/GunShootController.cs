using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] TargetPathController targetController;

    private Bounds spriteBounds;

    private void Awake()
    {
        spriteBounds = GetComponent<SpriteRenderer>().bounds;
    }
    private void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        Vector3 spriteLengthVectorRotated = CalculateSpriteLengthVectorRotated();
        Vector2 createPosition = transform.position + spriteLengthVectorRotated;

        GameObject newProjectileGameObject = Instantiate(projectilePrefab, createPosition, Quaternion.identity);
        ProjectileController newProjectileController = newProjectileGameObject.GetComponent<ProjectileController>();

        newProjectileController.SetDirection(spriteLengthVectorRotated.normalized);
        newProjectileController.SetSpeed(CalculateProjectileSpeed());

        CollisionManager.Instance.AddToCollidableList(newProjectileController);
    }

    private Vector3 CalculateSpriteLengthVectorRotated()
    {
        Vector3 spriteLengthVector = new Vector3(spriteBounds.extents.x, 0f);
        Vector3 spriteLengthVectorRotated = Quaternion.Euler(transform.rotation.eulerAngles) * spriteLengthVector;
        return spriteLengthVectorRotated;
    }

    private float CalculateProjectileSpeed()
    {
        Vector2 gunTargetVector = new Vector2(transform.position.x, 0f) - new Vector2(targetController.transform.position.x, 0f);
        float projectileSpeed = (gunTargetVector / 2f).magnitude;
        return projectileSpeed;
    }
}
