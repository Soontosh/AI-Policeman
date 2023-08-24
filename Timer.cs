using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private bool timerOn = false;
    private float timeLeft = 0f;

    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private GameObject startingPanel;
    [SerializeField] private GameObject timerPanel;

    void Update()
    {
        if (timerOn) {
            if (timeLeft > 0) {
                if (Time.unscaledDeltaTime < 1) timeLeft -= Time.unscaledDeltaTime;
                //Debug.Log(Time.unscaledDeltaTime);
                updateTimer(timeLeft);
            } else {
                timerOn = false;
                Debug.Log("Time Is Done!");
                timerPanel.SetActive(false);
                timeText.gameObject.SetActive(false);
                startingPanel.SetActive(true);
            }
        }
    }

    public void updateTimer(float currentTime) {
        currentTime += 1;
        currentTime = Mathf.FloorToInt(currentTime);
        timeText.text = currentTime.ToString();
    }

    public void activateTimer(float startingTime) {
        timeText.gameObject.SetActive(true);
        timerPanel.gameObject.SetActive(true);
        timeLeft = startingTime;
        timerOn = true;
        startingPanel.SetActive(false);
    }
}
