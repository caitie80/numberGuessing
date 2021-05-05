using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class Achievements : MonoBehaviour
{
    public void OpenAchievementPanel()
    {
        Social.ShowAchievementsUI();
    }

    public void UpdateStartingOut()
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_starting_out, 1, null);
    }

    public void UnlockOneGuess()
    {
        Social.ReportProgress(GPGSIds.achievement_one_guess_wonder, 100f, null);
    }

}
