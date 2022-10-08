using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIRotator : MonoBehaviour
{
    private GameObject cameraTransform;

    private void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()

    {
        transform.LookAt(cameraTransform.transform);
    }
}
