using System.Collections;
using UnityEngine;

namespace U_RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup CanvasGroup;

        private void Start()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            //StartCoroutine(FadeOutIn());
        }
        
         IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f); print("FadeOut");
            yield return FadeIn(1f); print("FadeIn");
        }
        public IEnumerator FadeOut(float time)
        {
            while (CanvasGroup.alpha < 1)
            {
                CanvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (CanvasGroup.alpha > 0)
            {
                CanvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
