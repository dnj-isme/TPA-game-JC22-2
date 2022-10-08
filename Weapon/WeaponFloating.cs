using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFloating : MonoBehaviour
{
    private Vector3 posOffset;
    private Vector3 tempPosition;

    private void Start()
    {
        tempPosition = Vector3.zero;
        posOffset = transform.position;
    }

    private void Update()
    {
        tempPosition = posOffset;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 0.2f) * 0.5f;
        transform.position = tempPosition;
        transform.Rotate(60 * Time.deltaTime * Vector3.forward);
    }
}
