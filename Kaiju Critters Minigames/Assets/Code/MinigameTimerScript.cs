using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Mathematics;
using UnityEditor.SearchService;

public class MinigameTimerScript : MonoBehaviour
{
    public enum Difficulty
    {
        Normal,
        Hard,
        VeryHard
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
    public Image fillImage;
    
    void Start()
    {
        timers = SetupTimers();
        Debug.Log(timers.Count);
        timerCurrent = timers[0]; 
        headline.text = headlines[0];
        timerSlider = GetComponentInChildren<Slider>();
        timerSlider.maxValue = timerCurrent;
        fillImage = GameObject.FindGameObjectWithTag("Fill").GetComponent<Image>();
        SetupButtons();
        SetupText();
    }
    void Update()
    {
        timerCurrent -= Time.deltaTime;
        timerSlider.value = timerCurrent;
        if(timerSlider.value < timerSlider.maxValue / 4f)
        {
            fillImage.color = Color.red;

        }
        else if (timerSlider.value < timerSlider.maxValue / 2f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.green;
        }
        if (timerCurrent < 0f && phase == CurrentPhase.GetReady)
        {
            phase = CurrentPhase.Play;
            timerCurrent = timers[1];
            timerSlider.maxValue = timerCurrent;
            headline.text = headlines[1];
        }
        else if (timerCurrent < 0f && phase == CurrentPhase.Play)
        {
            timerCurrent = timers[2];
            timerSlider.maxValue = timerCurrent;
            phase = CurrentPhase.Reset;
            if (successfulInputs == totalButtons)
            {
                headline.text = headlines[2];
                headline.color = Color.green;
            }
            else if (successfulInputs >= totalButtons / 2)
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
        else if (timerCurrent < 0 && phase == CurrentPhase.Reset)
        {
            SceneManager.LoadScene(0);
        }
    }
    public void SwitchScene(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int newScene = SceneManager.GetActiveScene().buildIndex + 1;
            if (newScene > SceneManager.sceneCount + 1)
            {
                newScene = 0;
            }
            SceneManager.LoadScene(newScene);
        }
    }
    public void ResetScene(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void AButtonPress(InputAction.CallbackContext context)
    {
        if(context.performed && phase == CurrentPhase.Play)
        {
            Debug.Log("A Button Press");
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
            Debug.Log("B Button Press");
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
            Debug.Log("X Button Press");
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
        Debug.Log("Y Button Press 1");
        if (context.performed && phase == CurrentPhase.Play)
        {
            Debug.Log("Y Button Press 2");
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
            case Difficulty.Normal:
                ret.Add(getReadyTime);
                ret.Add(playTime);
                ret.Add(resetTime);
                return ret;
            case Difficulty.Hard:
                ret.Add(getReadyTime);
                ret.Add(playTime);
                ret.Add(resetTime);
                return ret;
            case Difficulty.VeryHard:
                ret.Add(getReadyTime);
                ret.Add(playTime);
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
            //Number of buttons Normal = 3, Hard = 5, VeryHard = 7
            case Difficulty.Normal:
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
            case Difficulty.Hard:
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
            case Difficulty.VeryHard:
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
            case Difficulty.Normal:
                difficultyText.text = "Difficulty: Normal";
                difficultyText.color = Color.green;
                return;
            case Difficulty.Hard:
                difficultyText.text = "Difficulty: Hard";
                difficultyText.color = Color.yellow;
                return;
            case Difficulty.VeryHard:
                difficultyText.text = "Difficulty: Very Hard";
                difficultyText.color = Color.red;
                return;
        }
    }
}
