using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageController : MonoBehaviour
{
    public AfterImagePool afterImagePool;
    private SpriteRenderer spriteRenderer;
    private AfterImageDataSO afterImageData;

    public float timeElapsed, alpha;
    private Color color = Color.white;

    public void LoadDefaultDatas(AfterImageDataSO _afterImageData, AfterImagePool _afterImagePool)
    {
        afterImageData = _afterImageData;
        afterImagePool = _afterImagePool;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        DecreaseAlpha();
        CountDown();
    }
    private void DecreaseAlpha()
    {
        alpha = Mathf.Lerp(afterImageData.alphaStart, 0f, timeElapsed / afterImageData.duration);
        color = new Color(color.r, color.g, color.b, alpha);
        spriteRenderer.color = color;
    }
    private void CountDown()
    {
        if (timeElapsed >= afterImageData.duration)
            afterImagePool.AddToPool(this);
        else timeElapsed += Time.deltaTime;
    } 
    public void RefreshDatas(Transform _targetTransform)
    {
        Transform targetTransform = _targetTransform;
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;

        SpriteRenderer _targetSpriteRender = _targetTransform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _targetSpriteRender.sprite;
        spriteRenderer.sortingLayerName = _targetSpriteRender.sortingLayerName;
        spriteRenderer.sortingOrder = _targetSpriteRender.sortingOrder - 1;
        
        color = _targetSpriteRender.color;
        spriteRenderer.color = color;

        timeElapsed = 0f;
    }
}
