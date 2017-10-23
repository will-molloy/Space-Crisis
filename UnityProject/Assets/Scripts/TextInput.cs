using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextInput : MonoBehaviour {

    public InputField mainInputField;
    public Text FinalScoreText;
    public Button SubmitButton;

    public Text levelComplete;

    ScoreManager sm;

    //persisting fields
    private string TeamName;
    private string HighScore;
    private int levelType;

    void Start()
    {
        sm = ScoreManager.Instance();
        Button btn = SubmitButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {

        HighScore = FinalScoreText.text.ToString();

        if (mainInputField.text.ToString() == "") {
            //default value
            TeamName = "The Astronauts";
        } else
        {
            TeamName = mainInputField.text.ToString();
        }
        Debug.Log(Application.loadedLevelName);
        if (Application.loadedLevelName == "ExitScene")
        {
            levelType = 0;
        } else
        {
            levelType = 1;
        }

        //Debug.Log("Team: " + TeamName);
        //Debug.Log("Score: " + HighScore);
        //Debug.Log("level " + levelType);

        sm.SetScore(TeamName, levelType, HighScore);

        //for the user to know that their score has been stored in the board
        levelComplete.text = TeamName + "'s Score is " + HighScore;

    }



}
