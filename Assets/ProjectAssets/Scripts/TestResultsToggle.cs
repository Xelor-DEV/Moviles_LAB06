using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
public class TestResultsToggle : MonoBehaviour
{
    IEnumerator Start()
    {
        Debug.Log("=== INICIO PRUEBA ===");

        // Esperar a que se complete la carga
        yield return new WaitForSeconds(1f);

        // Verificar escenas cargadas
        Scene gameScene = SceneManager.GetSceneByName("MainGameAccelerometer"); // Usa tu nombre real
        Scene resultsScene = SceneManager.GetSceneByName("Results"); // Usa tu nombre real

        // 1. Verificar que ambas escenas están cargadas
        if (!gameScene.IsValid() || !resultsScene.IsValid())
        {
            //Debug.LogError("FALLO: No se cargaron ambas escenas");
            LogLoadedScenes();
            yield break;
        }

        // 2. Verificar que la escena activa es la correcta (debe ser el juego)
        if (SceneManager.GetActiveScene() != gameScene)
        {
            
            LogLoadedScenes();
            yield break;
        }

        // 3. Verificar visibilidad de Results
        GameObject[] resultsObjects = resultsScene.GetRootGameObjects();
        bool anyVisible = resultsObjects.Any(obj => obj.activeInHierarchy);

        if (anyVisible)
        {
           
            yield break;
        }

        Debug.Log("ÉXITO: Configuración inicial correcta");

        // 4. Probar mostrar resultados
        SceneGlobalManager.Instance.ShowResults();
        yield return null; // Esperar un frame

        anyVisible = resultsObjects.Any(obj => obj.activeInHierarchy);
        if (!anyVisible)
        {
         
            yield break;
        }

        Debug.Log("=== PRUEBA EXITOSA ===");
    }

    void LogLoadedScenes()
    {
        Debug.Log("Escenas cargadas:");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            Debug.Log($"- {scene.name} {(scene == SceneManager.GetActiveScene() ? "(ACTIVA)" : "")}");
        }
    }
}
