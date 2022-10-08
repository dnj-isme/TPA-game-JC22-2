using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using static Utility.UtilityScript;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private SceneType sceneType;

    private static int mainMenuIndex = -1;
    private static int loadingIndex = -1;
    private static int gameIndex = -1;
    private static int gameOverIndex = -1;

    private void Awake() 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        switch (sceneType)
        {
            case SceneType.MainMenu:
                mainMenuIndex = SceneManager.GetActiveScene().buildIndex;
                break;
            case SceneType.Loading:
                loadingIndex = SceneManager.GetActiveScene().buildIndex;
                break;
            case SceneType.Game:
                gameIndex = SceneManager.GetActiveScene().buildIndex;
                break;
            default:
                break;
        }
    }

    public void PlaySound(AudioSource src)
    {
        src.Play();
    }

    public void PlayFadeAnimation(GameObject obj)
    {
        obj.SetActive(true);
        Animator animator = obj.GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
    }

    public void LoadMainMenuScene(float sec)
    {
        StartCoroutine(DoAction(sec, () => {
            if (mainMenuIndex == -1) mainMenuIndex = gameIndex - 3;
            SceneManager.LoadScene(mainMenuIndex);
        }));
    }

    public void LoadLoadingScene(float sec)
    {
        StartCoroutine(DoAction(sec, () =>
        {
            if (loadingIndex == -1) loadingIndex = mainMenuIndex + 1;
            SceneManager.LoadScene(loadingIndex);
        }));
    }

    public void LoadGameScene(float sec)
    {
        StartCoroutine(DoAction(sec, () =>
        {
            if(gameIndex == -1) gameIndex = loadingIndex + 1;
            SceneManager.LoadScene(gameIndex);
        }));
    }
    public void LoadGameOverScene(float sec)
    {
        StartCoroutine(DoAction(sec, () =>
        {
            if (gameOverIndex == -1) gameOverIndex = gameIndex + 1;
            SceneManager.LoadScene(gameOverIndex);
        }));
    }
}
