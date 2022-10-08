using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    private Camera m_Camera;
    private Transform m_Transform;

    [SerializeField, Range(-3f, 3f)]
    private float rotateSpeed = 1;

    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
        m_Transform = m_Camera.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        m_Transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
