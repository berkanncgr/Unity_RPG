using System.Collections;
using UnityEngine;

namespace U_RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float Time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += UnityEngine.Time.deltaTime / Time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float Time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= UnityEngine.Time.deltaTime / Time;
                yield return null;
            }
        }
    }
}