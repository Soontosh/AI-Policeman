using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    public GameObject[] popUps;
    [SerializeField]
    GameObject clickContinue;
    private int currentIndex = 0;
    public bool tutorial = false;

    private IEnumerator Pause(int p) {
        Time.timeScale = 0;
        yield return new WaitForSeconds(p);
        Time.timeScale = 1;
    }

    void Update()
    {
        Debug.Log(LevelLoader.TutorialBool);
        Debug.Log("Current Index: " + currentIndex);
        if (LevelLoader.TutorialBool) {
            for (int i = 0; i < popUps.Length; i++) {
                if (i == currentIndex && !GetComponent<ShootingAgent>().countDown) {
                    popUps[i].SetActive(true);
                } else {
                    popUps[i].SetActive(false);
                }
                /*
                if (i == currentIndex) {
                    popUps[currentIndex].gameObject.SetActive(true);
                    //currentPopUp = popUps[currentIndex];
                    //currentPopUp.SetActive(true);
                    //Debug.Log("Active: " + popUps[currentIndex]);
                    //Debug.Log("Pop Ups:" + popUps);
                } else {
                    //if (popUps[currentIndex] != currentPopUp) {
                    //popUps[currentIndex].gameObject.SetActive(false);
                    //Debug.Log("Not Active: " + popUps[currentIndex]);
                    //}
                }
                */
            }

            if(currentIndex != 1) {
                if (currentIndex >= 2) {
                    Time.timeScale = 0;
                    clickContinue.SetActive(true);
                }

                if (Input.GetButtonDown("Fire1")) {
                    currentIndex++;

                    if (currentIndex == popUps.Length) {
                        BroadcastMessage("LoadMenu");
                    }
                    //currentPopUp.SetActive(false);
                }
            }
        }
    }

    public void TargetDown() {
        if (LevelLoader.TutorialBool) {
            if (currentIndex == 1) {
                currentIndex++;
                //currentPopUp.SetActive(false);
            }
        }
        
    }
}
