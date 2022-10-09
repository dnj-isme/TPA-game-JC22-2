using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] private float progress = 0;
    [SerializeField] private Slider slider;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation output = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while(!output.isDone)
        {
            Debug.Log(output.progress);
            progress = Mathf.Clamp01(output.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
