using System.Collections;
using U_RPG.Saving;
using UnityEngine;

namespace U_RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string DefaultSaveFile = "save";

        [SerializeField] float FadeInTime = 0.2f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        // Load last saved scene when game starts.
        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(DefaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(FadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        // Load last Save
        public void Load()
        {
            GetComponent<SavingSystem>().Load(DefaultSaveFile);
        }

        // Save current
        public void Save()
        {
            GetComponent<SavingSystem>().Save(DefaultSaveFile);
        }

        // Delete save file.
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(DefaultSaveFile);
        }
    }
}