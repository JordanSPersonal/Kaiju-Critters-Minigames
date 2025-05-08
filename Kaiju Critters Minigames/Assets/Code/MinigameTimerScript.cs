using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Mathematics;

public class MinigameTimerScript : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    public enum CurrentPhase
    {
        GetReady,
        Play,
        Reset
    }
    public Difficulty difficulty;
    public CurrentPhase phase;

    public List<float> timers;
    public float timerCurrent, getReadyTime, playTime, resetTime;

    public int successfulInputs, totalButtons;
    public GameObject buttonSpawnPos; 

    public float buttonOffset = 0.35f;
    public List<GameObject> buttonPrefabs;
    public List<ButtonBehaviourScript> activeButtons;

    public List<string> headlines;
    public TextMeshProUGUI headline, difficultyText;
    public Slider timerSlider;
    
    void Start()
    {
        timers = SetupTimers();
        Debug.Log(timers.Count);
        timerCurrent = timers[0]; 
        headline.text = headlines[0];
        timerSlider = GetComponentInChildren<Slider>();
        timerSlider.maxValue = timerCurrent;
        SetupButtons();
        SetupText();
    }
    void Update()
    {
        timerCurrent -= Time.deltaTime;
        timerSlider.value = timerCurrent;
        if (timerCurrent < 0f && phase == CurrentPhase.GetReady)
        {
            phase = CurrentPhase.Play;
            timerCurrent = timers[1];
            timerSlider.maxValue = timerCurrent;
            headline.text = headlines[1];
        }
        else if(timerCurrent < 0f && phase == CurrentPhase.Play)
        {
            timerCurrent = timers[2];
            timerSlider.maxValue = timerCurrent;
            phase = CurrentPhase.Reset;
            if (successfulInputs == totalButtons)
            {
                headline.text = headlines[2];
                headline.color = Color.green;
            }
            else if(successfulInputs >= totalButtons / 2)
            {
                headline.text = headlines[3];
                headline.color = Color.yellow;
            }
            else
            {
                headline.text = headlines[4];
                headline.color = Color.red;
            }
        }
        else if (timerCurrent < 0  && phase == CurrentPhase.Reset)
        {
            SceneManager.LoadScene(0);
        }
    }
    public void AButtonPress(InputAction.CallbackContext context)
    {
        if(context.performed && phase == CurrentPhase.Play)
        {
            if (activeButtons[0].buttonType == ButtonBehaviourScript.ButtonType.AButton)
            {
                Debug.Log("Correct Button!");
                activeButtons[0].ChangeSprite(1);
                activeButtons.RemoveAt(0);
                successfulInputs++;
            }
            else
            {
                Debug.Log("Wrong Button :(");
                activeButtons[0].ChangeSprite(2);
                activeButtons.RemoveAt(0);
            }
        }
    }
    public void BButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed && phase == CurrentPhase.Play)
        {
            if (activeButtons[0].buttonType == ButtonBehaviourScript.ButtonType.BButton)
            {
                Debug.Log("Correct Button!");
                activeButtons[0].ChangeSprite(1);
                activeButtons.RemoveAt(0);
                successfulInputs++;
            }
            else
            {
                Debug.Log("Wrong Button :(");
                activeButtons[0].ChangeSprite(2);
                activeButtons.RemoveAt(0);
            }
        }
    }
    public void XButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed && phase == CurrentPhase.Play)
        {
            if (activeButtons[0].buttonType == ButtonBehaviourScript.ButtonType.XButton)
            {
                Debug.Log("Correct Button!");
                activeButtons[0].ChangeSprite(1);
                activeButtons.RemoveAt(0);
                successfulInputs++;
            }
            else
            {
                Debug.Log("Wrong Button :(");
                activeButtons[0].ChangeSprite(2);
                activeButtons.RemoveAt(0);
            }
        }
    }
    public void YButtonPress(InputAction.CallbackContext context)
    {
        Debug.Log("Y Button Press");
        if (context.performed && phase == CurrentPhase.Play)
        {
            if (activeButtons[0].buttonType == ButtonBehaviourScript.ButtonType.YButton)
            {
                Debug.Log("Correct Button!");
                activeButtons[0].ChangeSprite(1);
                activeButtons.RemoveAt(0);
                successfulInputs++;
            }
            else
            {
                Debug.Log("Wrong Button :(");
                activeButtons[0].ChangeSprite(2);
                activeButtons.RemoveAt(0);
            }
        }
    }
    public List<float> SetupTimers()
    {
        List<float> ret = new List<float>();
        switch (difficulty)
        {        
            //First is get ready time
            //Second timer to add is overall time to do input
            //Third timer added is time to reset scene after minigame completion
            case Difficulty.Easy:
                ret.Add(getReadyTime);
                ret.Add(playTime);
                ret.Add(resetTime);
                return ret;
            case Difficulty.Medium:
                ret.Add(getReadyTime);
                ret.Add(playTime * 0.75f);
                ret.Add(resetTime);
                return ret;
            case Difficulty.Hard:
                ret.Add(getReadyTime);
                ret.Add(playTime * 0.5f);
                ret.Add(resetTime);
                return ret;
            default:
                ret.Add(getReadyTime);
                ret.Add(playTime);
                ret.Add(resetTime);
                return ret;
        }
    }
    public void SetupButtons()
    {
        switch (difficulty)
        {
            //Number of buttons easy = 3, medium = 5, hard = 7
            case Difficulty.Easy:
                totalButtons = 3;
                for(int i = 0; i < totalButtons; i++)
                {
                    int random = UnityEngine.Random.Range(0, buttonPrefabs.Count);
                    ButtonBehaviourScript newButton = Instantiate(buttonPrefabs[random], buttonSpawnPos.transform.position, Quaternion.identity, buttonSpawnPos.transform).GetComponent<ButtonBehaviourScript>();
                    activeButtons.Add(newButton);
                }
                activeButtons[0].gameObject.transform.localPosition += new Vector3(-buttonOffset, 0f, 0f);
                activeButtons[2].gameObject.transform.localPosition += new Vector3(buttonOffset, 0f, 0f);
                return;
            case Difficulty.Medium:
                totalButtons = 5;
                for (int i = 0; i < totalButtons; i++)
                {
                    int random = UnityEngine.Random.Range(0, buttonPrefabs.Count);
                    ButtonBehaviourScript newButton = Instantiate(buttonPrefabs[random], buttonSpawnPos.transform.position, Quaternion.identity, buttonSpawnPos.transform).GetComponent<ButtonBehaviourScript>();
                    activeButtons.Add(newButton);
                }
                activeButtons[0].gameObject.transform.localPosition += new Vector3(-buttonOffset * 2f, 0f, 0f);
                activeButtons[1].gameObject.transform.localPosition += new Vector3(-buttonOffset, 0f, 0f);
                activeButtons[3].gameObject.transform.localPosition += new Vector3(buttonOffset, 0f, 0f);
                activeButtons[4].gameObject.transform.localPosition += new Vector3(buttonOffset * 2f, 0f, 0f);
                return;
            case Difficulty.Hard:
                totalButtons = 7;
                for (int i = 0; i < totalButtons; i++)
                {
                    int random = UnityEngine.Random.Range(0, buttonPrefabs.Count);
                    ButtonBehaviourScript newButton = Instantiate(buttonPrefabs[random], buttonSpawnPos.transform.position, Quaternion.identity, buttonSpawnPos.transform).GetComponent<ButtonBehaviourScript>();
                    activeButtons.Add(newButton);
                }
                activeButtons[0].gameObject.transform.localPosition += new Vector3(-buttonOffset * 3f, 0f, 0f);
                activeButtons[1].gameObject.transform.localPosition += new Vector3(-buttonOffset * 2f, 0f, 0f);
                activeButtons[2].gameObject.transform.localPosition += new Vector3(-buttonOffset, 0f, 0f);
                activeButtons[4].gameObject.transform.localPosition += new Vector3(buttonOffset, 0f, 0f);
                activeButtons[5].gameObject.transform.localPosition += new Vector3(buttonOffset * 2f, 0f, 0f);
                activeButtons[6].gameObject.transform.localPosition += new Vector3(buttonOffset * 3f, 0f, 0f);
                return;
        }
    }    
    public void SetupText()
    {
        switch (difficulty)
        {
            //Describes difficulty
            case Difficulty.Easy:
                difficultyText.text = "Difficulty: Easy";
                difficultyText.color = Color.green;
                return;
            case Difficulty.Medium:
                difficultyText.text = "Difficulty: Medium";
                difficultyText.color = Color.yellow;
                return;
            case Difficulty.Hard:
                difficultyText.text = "Difficulty: Hard";
                difficultyText.color = Color.red;
                return;
        }
    }
}
