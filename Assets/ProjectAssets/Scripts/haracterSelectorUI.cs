using UnityEngine;
using UnityEngine.UI;

public class haracterSelectorUI : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private void Start()
    {
        // Asigna este método al botón Play
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        // Llama al método que carga Game + Results
        SceneGlobalManager.Instance.LoadGameWithResults();

        // Opcional: Desactivar el botón para evitar múltiples clics
        playButton.interactable = false;

        Debug.Log("Iniciando juego con escena Results cargada");
    }
}
