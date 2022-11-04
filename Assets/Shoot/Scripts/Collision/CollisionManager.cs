using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    private Camera mainCamera;
    private List<CollidableProjectile> collidableList;
    [SerializeField] TargetCollidable targetCollidable;

    private void Awake()
    {
        Instance = this;
        collidableList = new List<CollidableProjectile>();
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        ManageCollidables();
    }
    private void ManageCollidables()
    {
        if (collidableList.Count <= 0) return;

        foreach (CollidableProjectile projectile in collidableList)
        {
            if(CheckForCollision(projectile)) return;
            if(DestroyOutOfCameraProjectiles(projectile)) return;
        }
    }
    private bool CheckForCollision(CollidableProjectile projectile)
    {
        float radius = projectile.radius;
        Vector3 position = projectile.transform.position;

        float targetRadius = targetCollidable.radius;
        Vector3 targetPosition = targetCollidable.transform.position;
        Vector2 distanceVector = targetPosition - position;
        float distance = distanceVector.magnitude;

        if (distance <= radius + targetRadius)
        {
            projectile.Collision(targetCollidable.transform);
            targetCollidable.Collision(projectile.transform);
            return true;
        }

        return false;
    }
    private bool DestroyOutOfCameraProjectiles(CollidableProjectile projectile)
    {
        Vector3 position = projectile.transform.position;
        Vector3 collidableViewportPosition = mainCamera.WorldToViewportPoint(position);
        if (collidableViewportPosition.x < 0 || collidableViewportPosition.x > 1 || collidableViewportPosition.y < 0 || collidableViewportPosition.y > 1)
        {
            projectile.DestroyProjectile();
            return true;
        }
        return false;
    }
    public void AddToCollidableList(CollidableProjectile collidable) => collidableList.Add(collidable);
    public void RemoveFromCollidableList(CollidableProjectile collidable) => collidableList.Remove(collidable);
}
