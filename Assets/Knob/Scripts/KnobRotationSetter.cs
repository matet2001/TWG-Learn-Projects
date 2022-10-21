using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobRotationSetter : MonoBehaviour
{
    [SerializeField] KnobRotationController rotationController;

    private void Start()
    {
        rotationController.OnRotationValueChange += RotationController_OnRotationValueChange;
    }

    private void RotationController_OnRotationValueChange(float rotationToSet, float unitAmount)
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, rotationToSet));
    }
}
