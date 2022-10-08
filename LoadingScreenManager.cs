using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class LoadingScreenManager : MonoBehaviour
{
    private float time;
    [SerializeField, Range(2, 10)] private float waitTime = 5;

    private void Awake()
    {
        time = Time.time;
        GetComponent<GameSceneManager>().LoadGameScene(2);
    }
}
