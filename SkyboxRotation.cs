using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    [SerializeField, Range(0.001f, 10f)] private float rotationSpeed = 1f;
    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);   
    }
}
