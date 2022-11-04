using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectileManager : MonoBehaviour
{
    public static event Action<float> OnWindValueChange;
    public static float wind { private set; get; }
    public static float gravity { private set; get; } = 1f;

    public static void RandomizeWind()
    {
        wind = UnityEngine.Random.Range(-1f, 1f);
        OnWindValueChange?.Invoke(wind);
    }
    public static CanonProjectileController CreateProjectile(Vector2 position, float speed, Vector2 direction)
    {
        GameObject projectilePrefab = Resources.Load("PfCanonProjectile") as GameObject;

        GameObject newProjectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        CanonProjectileController newProjectileController = newProjectile.GetComponent<CanonProjectileController>();
        newProjectileController.SetSpeed(speed);
        newProjectileController.SetDirection(direction);
        return newProjectileController;
    } 
}
