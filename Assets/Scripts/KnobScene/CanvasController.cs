using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] Text angleText;

    public void SetAngleText(string textToSet) => angleText.text = textToSet;
}
