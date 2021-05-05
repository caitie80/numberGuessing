using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboards : MonoBehaviour
{
    public void OpenLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void UpdateLeaderboard()
    {
        if (PlayerPrefs.GetFloat("Time") > 0)
        {
            int timeInSeconds = Convert.ToInt32(PlayerPrefs.GetFloat("Time", 1));
            Social.ReportScore(timeInSeconds, GPGSIds.leaderboard_quickest_times, null);
        }
    }
}
