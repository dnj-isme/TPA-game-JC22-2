using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField, Range(2, 10)] private float waitTime = 5;

    private void Awake()
    {
        GetComponent<GameSceneManager>().LoadGameScene(waitTime);
    }
}
