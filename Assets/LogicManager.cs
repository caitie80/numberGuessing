using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicManager : MonoBehaviour
{
    public GameManager gm;


    private static int gamesToPlay;
    private int numberToGuess;
    private int? guess;
    private System.Random rnd;
    private int numberOfGuesses;

    private bool timerRunning;
    private float timer;

    private string winningMessage;
    private string guessingMessage;

    public InputField guessInput;
    public Text resultText;
    public Button playAgain;
    public Button guessButton;
    public Text numOfGuessesText;
    public Text timerText;
    public Text livesText;

    public Leaderboards leaderboard;
    public Achievements achievements;

    // Start is called before the first frame update
    void Start()
    {
        //gm = new GameManager();
        gamesToPlay = 2;
        rnd = new System.Random();
        
        winningMessage = "Congratulations!";
        guessingMessage = "Guess between 1- 100";

        StartLevel();
        livesText.text = gamesToPlay.ToString();

        leaderboard = new Leaderboards();
        achievements = new Achievements();
    }

    // Update is called once per frame
    void Update()
    {
        if(guess == numberToGuess)
        {
            GameWon();
            guess = null;
        }
        else if(guess > numberToGuess)
        {
            resultText.text = "Lower";
        }
        else if(guess < numberToGuess)
        {
            resultText.text = "Higher";
        }
        
        if(timerRunning)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("0.00");
        }
        livesText.text = gamesToPlay.ToString();
    }

    private void GameWon()
    {
        gm.ShowAnInterstitial();
        timerRunning = false;
        resultText.text = winningMessage;
        playAgain.gameObject.SetActive(true);
        guessButton.gameObject.SetActive(false);
        gamesToPlay = gamesToPlay-1;


        PlayerPrefs.SetFloat("Time", timer);        
        leaderboard.UpdateLeaderboard();
        achievements.UpdateStartingOut();

    }

    public void StartLevel()
    {
        if(gamesToPlay == 0)
        {
            resultText.text = "Out of Lives! watch reward";
        }
        else
        {
            guessButton.gameObject.SetActive(true);
            numberToGuess = rnd.Next(1, 101);

            guess = null;

            guessInput.text = "";
            resultText.text = guessingMessage;

            playAgain.gameObject.SetActive(false);

            numberOfGuesses = 0;
            numOfGuessesText.text = numberOfGuesses.ToString();

            timer = 0.0f;
            timerRunning = true;

            Debug.Log(numberToGuess);
        }
        
    }

    public void MakeGuess()
    {
        if(guessInput.text != null && guessInput.text != "")
        {
            guess = int.Parse(guessInput.text);
            numberOfGuesses++;
            numOfGuessesText.text = numberOfGuesses.ToString();
        }    
    }

    public static void AddGame()
    {
        gamesToPlay = gamesToPlay + 1;
    }
}
