using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultsBoardManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;

    [Header("Data References")]
    [SerializeField] private ScoreData scoreData;

    [Header("Scene Settings")]
    [SerializeField] private string mainSceneName = "MainGame";

    void Start()
    {
        UpdateScoreDisplays();
    }

    private void UpdateScoreDisplays()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = $"Score: {scoreData.CurrentScore}";
        }

        if (highScoreText != null)
        {
            highScoreText.text = $"HighScore: {scoreData.HighScore}";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}