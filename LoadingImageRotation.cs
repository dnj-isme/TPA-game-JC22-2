using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingImageRotation : MonoBehaviour
{
    [SerializeField, Range(300, 1000)] private float speed;
    private RectTransform rt;
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.Rotate(Time.deltaTime * speed * Vector3.forward);
    }
}
