using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class UtilityScript : MonoBehaviour
    {
        public delegate void Action();

        public static IEnumerator DoAction(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }
    }

    public enum SceneType
    {
        MainMenu, Loading, Game, GameOver
    }
}
