using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    private List<Collidable> collidableList;
    [SerializeField] TargetController targetController;

    private void Awake()
    {
        Instance = this;
        collidableList = new List<Collidable>();
    }
    private void Update()
    {
        CheckForCollision();
    }
    private void CheckForCollision()
    {
        if (collidableList.Count == 0) return;

        foreach (Collidable collidable in collidableList)
        {
            float radius = collidable.radius;
            Vector3 position = collidable.transform.position;

            List<Collidable> newCollidableList = new List<Collidable>();
            newCollidableList.AddRange(collidableList);
            newCollidableList.Remove(collidable);

            foreach (Collidable otherCollidable in newCollidableList)
            {
                float otherRadius = otherCollidable.radius;
                Vector3 otherPosition = otherCollidable.transform.position;

                float distance = Vector2.SqrMagnitude(otherPosition - position);
                Debug.DrawLine(position, otherPosition, Color.white);
                //Debug.DrawLine(position, position + new Vector3(distance, 0f), Color.red);


                if (distance < radius + otherRadius)
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
