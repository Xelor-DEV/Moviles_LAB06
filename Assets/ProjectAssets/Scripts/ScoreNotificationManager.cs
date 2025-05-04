using UnityEngine;
using System;


#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;

public class ScoreNotificationManager : MonoBehaviour
{
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private string scoreChanel = "score_updates";
    [SerializeField] private string recordChanel = "high_scores";

    [Header("Notification Icons")]
    [SerializeField] private string scoreSmall = "icon_score_small";
    [SerializeField] private string scoreLarge = "icon_score_large";
    [SerializeField] private string recordSmall = "icon_highscore_small";
    [SerializeField] private string recordLarge = "icon_highscore_large";

    void Start()
    {
        RequestAuthorization();
        RegisterNewNotificationChannel(scoreChanel, "Score Notifications", Importance.Default, "Game score updates");
        RegisterNewNotificationChannel(recordChanel, "Record Breaking", Importance.High, "New high score notifications");
        SendNotification("Round Finished", "Score: " + scoreData.CurrentScore, DateTime.Now, scoreSmall, scoreLarge, scoreChanel);
        if(scoreData.CheckUpdatedHighScore() == true)
        {
            SendNotification("New High Score!", "Congratulations! New record: " + scoreData.HighScore, DateTime.Now, recordSmall, recordLarge, recordChanel);
        }
    }


    private void RequestAuthorization()
    {
        if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS") == false)
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    private void RegisterNewNotificationChannel(string id, string name, Importance importance, string description)
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel
        {
            Id = id,
            Name = name,
            Importance = importance,
            Description = description
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void SendNotification(string title, string text, DateTime fireTime, string smallIcon, string largeIcon, string channelId)
    {
        AndroidNotification notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = fireTime;
        notification.SmallIcon = smallIcon;
        notification.LargeIcon = largeIcon;

        AndroidNotificationCenter.SendNotification(notification, channelId);
    }
}
#endif