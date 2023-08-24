using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    public GameObject LoadingScreen;
    [SerializeField]
    public Slider slider;
    [SerializeField]
    public Text ProgressText;
    
    public static bool TutorialBool = false;
    
    public void BeginTutorial() {
        TutorialBool = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Slay");
        StartCoroutine(LoadScene(1));
    }

    public void LoadMenu() {
        TutorialBool = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(LoadScene(0));
    }


    IEnumerator LoadScene(int sceneIndex) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        LoadingScreen.SetActive(true);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            ProgressText.text = progress * 100f + "%";
            Debug.Log(progress);

            yield return null;
        }
    }
}
