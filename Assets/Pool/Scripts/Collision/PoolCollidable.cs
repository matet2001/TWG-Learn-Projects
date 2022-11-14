using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolCollidableCircle : MonoBehaviour
{
    public float radius { get; private set; }

    public void Awake()
    {
        radius = GetComponent<SpriteRenderer>().bounds.extents.x;
    }
}
