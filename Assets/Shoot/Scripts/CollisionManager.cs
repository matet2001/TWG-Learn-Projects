using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{  
    private List<Collidable> collidableList;

    private void Start()
    {
        collidableList.AddRange(FindObjectsOfType<Collidable>());
    }
    private void Update()
    {
        CheckForCollision();
    }
    private void CheckForCollision()
    {
        foreach (Collidable collidable in collidableList)
        {
            float radius = collidable.radius;
            Vector3 position = collidable.transform.position;

            List<Collidable> newCollidableList = collidableList;
            newCollidableList.Remove(collidable);

            foreach (Collidable otherCollidable in newCollidableList)
            {
                float otherRadius = otherCollidable.radius;
                Vector3 otherPosition = otherCollidable.transform.position;

                if(Vector3.Distance(position, otherPosition) < radius + otherRadius)
                {
                    collidable.Collision(otherCollidable.transform);
                    otherCollidable.Collision(collidable.transform);
                }
            }
        }
    }

    public void AddToCollidableList(Collidable collidable) => collidableList.Add(collidable);
    public void RemoveFromCollidableList(Collidable collidable) => collidableList.Remove(collidable);
}
