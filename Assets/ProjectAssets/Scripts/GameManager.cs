using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HealthData playerHealthData;
    [SerializeField] private string gameOverScene = "Results";

    private void OnEnable()
    {
        playerHealthData.onDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        playerHealthData.onDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        SceneManager.LoadScene(gameOverScene);
    }
}