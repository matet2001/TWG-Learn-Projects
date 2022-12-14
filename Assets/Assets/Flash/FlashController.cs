using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    [SerializeField] SpriteRenderer targetSpriteRenderer;
    private SpriteRenderer spriteRenderer;

    private enum FlashStates { Nothing, Flash, FlashingRaise, FlashingReduce, Charge }
    [SerializeField] FlashStates flashState = FlashStates.Nothing;

    private float duration, timeElapsed;
    private float alpha, flashingSpeed;

    private void Start()
    {
        SetUpSpriteRenderer();
    }
    private void SetUpSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = targetSpriteRenderer.sortingLayerName;
        spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder + 1;
        spriteRenderer.sprite = targetSpriteRenderer.sprite;
        spriteRenderer.color = Color.clear;
        spriteRenderer.enabled = false;
    }
    private void Update()
    {
        Timer();
        HandleFlashing();
        HandleCharge();
    }
    private void Timer()
    {
        if (flashState != FlashStates.Flash) return;
        
        if (timeElapsed < duration) timeElapsed += Time.deltaTime;
        else StopFlashState();
    }
    public void StopFlashState()
    {
        spriteRenderer.color = Color.clear;
        spriteRenderer.enabled = false;
        
        flashState = FlashStates.Nothing;
        timeElapsed = 0f;
    }
    public void Flash(Color _flashColor, float _duration)
    {
        if (flashState != FlashStates.Nothing) return;

        spriteRenderer.enabled = true;
        spriteRenderer.color = _flashColor;
        duration = _duration;
        
        flashState = FlashStates.Flash;
    }
    public void StartFlashing(Color _flashingColor, float _flashingSpeed)
    {
        if (flashState != FlashStates.Nothing) return;

        spriteRenderer.enabled = true;
        spriteRenderer.color = new Color(_flashingColor.r, _flashingColor.g, _flashingColor.b, 0f);

        flashingSpeed = 1f / _flashingSpeed;     

        flashState = FlashStates.FlashingRaise;      
    }
    private void HandleFlashing()
    {
        if (flashState != FlashStates.FlashingRaise && flashState != FlashStates.FlashingReduce) return;

        if (flashState == FlashStates.FlashingRaise)
        {
            alpha += Time.deltaTime * flashingSpeed;
            if (alpha >= 1f) flashState = FlashStates.FlashingReduce;
        } 
        if (flashState == FlashStates.FlashingReduce)
        {
            alpha -= Time.deltaTime * flashingSpeed;
            if (alpha <= 0f) flashState = FlashStates.FlashingRaise;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }
    public void StartCharge(Color _chargeColor, float _duration)
    {
        if (flashState != FlashStates.Nothing) return;

        spriteRenderer.enabled = true;
        spriteRenderer.color = new Color(_chargeColor.r, _chargeColor.g, _chargeColor.b, 0f);
        duration = _duration;

        flashState = FlashStates.Charge;
    }
    private void HandleCharge()
    {
        if (flashState != FlashStates.Charge) return;

        alpha = Mathf.Lerp(0f, 1f, timeElapsed / duration);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        if (timeElapsed < duration) timeElapsed += Time.deltaTime;
    }
    public bool IsFlashing() => flashState == FlashStates.FlashingRaise || flashState == FlashStates.FlashingReduce;
}
