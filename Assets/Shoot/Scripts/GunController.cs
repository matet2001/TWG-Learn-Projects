using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] CollisionManager collisionManager;
    private Bounds spriteBounds;

    private Vector2 startVector;

    private void Awake()
    {
        spriteBounds = GetComponent<SpriteRenderer>().bounds;
    }
    private void Start()
    {
        EventBasedRotationInputManager rotationInputManager = GetComponent<EventBasedRotationInputManager>();
        rotationInputManager.OnAngleChange += RotationInputManager_OnAngleChange;
        rotationInputManager.OnDragStart += RotationInputManager_OnDragStart;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) Shoot();
    }
    private void SetRotation(float value)
    {
        Vector2 currentVector = Quaternion.Euler(value, 0f, 0f) * Vector2.up;

        float signedDifference = Vector2.SignedAngle(startVector, currentVector);
        transform.rotation.SetFromToRotation(transform.rotation.eulerAngles, new Vector3(0f, 0f, signedDifference));
    }
    private void Shoot()
    {
        Vector3 spriteLengthVector = new Vector3(spriteBounds.extents.x, 0f);
        Vector3 spriteLengthVectorRotated = Quaternion.Euler(transform.rotation.eulerAngles) * spriteLengthVector;

        Vector2 createPosition = transform.position + spriteLengthVectorRotated;
        GameObject newProjectileGameObject = Instantiate(projectilePrefab, createPosition, Quaternion.identity);
        ProjectileController newProjectileController = newProjectileGameObject.GetComponent<ProjectileController>();
        newProjectileController.SetDirection(spriteLengthVectorRotated.normalized);

        collisionManager.AddToCollidableList(newProjectileController);
    }
    private void RotationInputManager_OnDragStart(float value)
    {
        startVector = Quaternion.Euler(value, 0f, 0f) * Vector2.up;
    }
    private void RotationInputManager_OnAngleChange(float value)
    {
        SetRotation(value);
    }
}
