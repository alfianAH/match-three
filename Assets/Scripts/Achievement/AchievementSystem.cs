using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : Observer
{
    public Image achievementBanner;
    public Text achievementText;
    
    // Event
    private TileEvent cookiesEvent, cakeEvent, lollipopEvent;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        
        // Make events
        cookiesEvent = new CookieTileEvent(3);
        cakeEvent = new CakeTileEvent(10);
        lollipopEvent = new LollipopTileEvent(5);

        foreach (var pointOfInterest in FindObjectsOfType<PointOfInterest>())
        {
            pointOfInterest.RegisterObserver(this);
        }
    }

    public override void OnNotify(string value)
    {
        string key;

        if(value.Equals("Cookie event"))
        {
            cookiesEvent.OnMatch();
            if (cookiesEvent.AchievementCompleted())
            {
                key = "Match first cookies";
                NotifyAchievement(key, value);
            }
        }

        if (value.Equals("Cake event"))
        {
            cakeEvent.OnMatch();
            if (cakeEvent.AchievementCompleted())
            {
                key = "Match 10 cake";
                NotifyAchievement(key, value);
            }
        }

        if (value.Equals("Lollipop event"))
        {
            lollipopEvent.OnMatch();
            if (lollipopEvent.AchievementCompleted())
            {
                key = "Match 5 lollipops";
                NotifyAchievement(key, value);
            }
        }
    }

    private void NotifyAchievement(string key, string value)
    {
        // Check if achievement has been obtained
        if (PlayerPrefs.GetInt(value) == 1) return;
        
        PlayerPrefs.SetInt(value, 1);
        achievementText.text = key + " unlocked!";
        
        // Pop up notification
        StartCoroutine(ShowAchievementBanner());
    }

    private void ActivateAchievementBanner(bool active)
    {
        achievementBanner.gameObject.SetActive(active);
    }

    private IEnumerator ShowAchievementBanner()
    {
        ActivateAchievementBanner(true);
        yield return new WaitForSeconds(2f);
        ActivateAchievementBanner(false);
    }
}
