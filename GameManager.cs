using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public List<GameObject> rings;
    public GameObject OutcomeText;
    public GameObject CountDownText;
    public float remainingTime;
    public string NextSceneName;
    private bool isGameActive;

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
    }
       
    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            Timer();
        }
    }
    public void FlyThroughRing(GameObject ring)
    {
        if (isGameActive)
        {
            rings.Remove(ring);
            if (rings.Count == 0)
            {
                EndGame(true);
            }
        }

    }

    public void EndGame(bool win)
    {
        isGameActive = false;
        if (win)
        {
            OutcomeText.GetComponent<TextMeshProUGUI>().text = "LEVEL COMPLETE - YOU WIN";
            StartCoroutine(delayed_load(NextSceneName, 5));
        }
        else
        {
            OutcomeText.GetComponent<TextMeshProUGUI>().text = "OUT OF TIME - YOU LOSE";
            StartCoroutine(delayed_load(SceneManager.GetActiveScene().name, 5));
        }
    }

    IEnumerator delayed_load(string sceneName, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }

    void Timer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            EndGame(false);
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        CountDownText.GetComponent<TextMeshProUGUI>().text = string.Format("Time Remaining: {0:00}:{1:00}", minutes, seconds);
    }
}
