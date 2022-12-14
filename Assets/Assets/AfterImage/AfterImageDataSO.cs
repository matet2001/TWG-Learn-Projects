using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AfterImageData", menuName = "ScriptableObjects/Effects/AfterImageData", order = 1)]
public class AfterImageDataSO : ScriptableObject
{
    public float distanceBetweenImages = 0.5f;
    public float duration = 10f;
    public float alphaStart = 0.7f;
}
