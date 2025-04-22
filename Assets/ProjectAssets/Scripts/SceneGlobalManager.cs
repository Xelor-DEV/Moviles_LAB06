using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class SceneGlobalManager : MonoBehaviour
{
    public static SceneGlobalManager Instance { get; private set; }

    [Header("Scene Names")]
    public string splashScreenScene = "SplashScreen";
    public string menuScene = "MainMenu";
    public string characterSelectScene = "CharacterSelection";
    public string gameScene = "MainGameAccelerometer";
    public string resultsScene = "Results";

    private bool resultsLoaded = false;

    private void Awake()
    {
        // Singleton pattern
        if (gameObject.scene.IsValid())
        {
            Debug.Log($"Manager en escena: {gameObject.scene.name}");
        }
        else
        {
            Debug.LogError("SceneGlobalManager no está en una escena válida");
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Si estamos en SplashScreen, cargar el menú
            if (SceneManager.GetActiveScene().name == splashScreenScene)
            {
                StartCoroutine(LoadMenuFromSplash());
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("=== ESCENAS ACTUALES ===");
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                Debug.Log($"{i}: {scene.name} {(scene.isLoaded ? "Cargada" : "No cargada")} {(scene == SceneManager.GetActiveScene() ? "(ACTIVA)" : "")}");
            }
        }
    }
    private IEnumerator LoadMenuFromSplash()
    {
        // Cargar menú de forma aditiva
        yield return SceneManager.LoadSceneAsync(menuScene, LoadSceneMode.Additive);

        // Descargar splash
        yield return SceneManager.UnloadSceneAsync(splashScreenScene);

        // Establecer menú como activo
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(menuScene));
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadSceneAndCleanup(menuScene));
    }

    public void LoadCharacterSelect()
    {
        StartCoroutine(LoadSceneAndCleanup(characterSelectScene));
    }

    public void LoadGameWithResults()
    {
        StartCoroutine(LoadGameAndResults());
    }

    private IEnumerator LoadGameAndResults()
    {
        // 1. Limpieza completa
        yield return CleanupAllScenes();

        // 2. Cargar juego como escena principal
        yield return SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Single);

        // 3. Verificar si Results ya está cargada antes de cargarla
        if (!SceneManager.GetSceneByName(resultsScene).IsValid())
        {
            yield return SceneManager.LoadSceneAsync(resultsScene, LoadSceneMode.Additive);

            // Ocultar Results inmediatamente
            Scene resultsSceneObj = SceneManager.GetSceneByName(resultsScene);
            foreach (GameObject obj in resultsSceneObj.GetRootGameObjects())
            {
                obj.SetActive(false);
            }
        }

        Debug.Log("Juego y Results cargados sin duplicados");
    }

    private IEnumerator CleanupAllScenes()
    {
        // Esperar al final del frame para asegurar consistencia
        yield return new WaitForEndOfFrame();

        // Crear lista segura para descarga
        List<string> scenesToUnload = new List<string>();

        // Recolectar nombres de escenas a descargar
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene != gameObject.scene && scene.isLoaded && scene.IsValid())
            {
                scenesToUnload.Add(scene.name);
            }
        }

        // Descargar usando nombres en lugar de referencias directas
        foreach (string sceneName in scenesToUnload)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid() && scene.isLoaded)
            {
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
                while (unloadOp != null && !unloadOp.isDone)
                {
                    yield return null;
                }
            }
            yield return null; // Pequeña pausa entre descargas
        }

        // Limpieza final
        Resources.UnloadUnusedAssets();
        yield return null;
    }
    private IEnumerator LoadSceneAndCleanup(string sceneName)
    {
        // Limpiar escenas anteriores
        yield return CleanupScenes();

        // Cargar nueva escena
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    private IEnumerator CleanupScenes()
    {
        // Descargar todas las escenas excepto la persistente
        for (int i = SceneManager.sceneCount - 1; i >= 0; i--)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene != gameObject.scene && scene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(scene);
            }
        }

        resultsLoaded = false;
        Resources.UnloadUnusedAssets();
    }

    public void ShowResults()
    {
        if (!resultsLoaded) return;

        Scene resultsSceneObj = SceneManager.GetSceneByName(resultsScene);
        if (!resultsSceneObj.IsValid()) return;

        // Activar solo el Canvas de Results
        GameObject[] rootObjects = resultsSceneObj.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            if (obj.CompareTag("ResultsCanvas")) // Asigna este tag al Canvas de Results
            {
                obj.SetActive(true);
            }
        }

        Debug.Log("Results mostrado correctamente");
    }

    public void HideResults()
    {
        if (resultsLoaded)
        {
            SetResultsVisibility(false);
        }
    }

    private void SetResultsVisibility(bool visible)
    {
        Scene resultsSceneObj = SceneManager.GetSceneByName(resultsScene);
        if (!resultsSceneObj.IsValid()) return;

        foreach (GameObject obj in resultsSceneObj.GetRootGameObjects())
        {
            obj.SetActive(visible);
        }
    }


    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


}
