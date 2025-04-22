using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceShipSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject layoutGroupContainer;
    [SerializeField] private GameObject shipEntryPrefab;
    [SerializeField] private string sceneWithAccelerometer;
    [SerializeField] private string sceneWithGyroscope;

    [Header("Data")]
    [SerializeField] private SpaceShipData[] availableShips;
    [SerializeField] private SpaceShipData selectedShipData;

    private void Start()
    {
        CreateShipSelectionUI();
    }

    private void CreateShipSelectionUI()
    {
        if (availableShips != null && shipEntryPrefab != null)
        {
            for(int i = 0; i < availableShips.Length; ++i)
            {
                GameObject newEntry = Instantiate(shipEntryPrefab, layoutGroupContainer.transform);
                ConfigureShipEntry(newEntry, availableShips[i]);
            }
        }
    }

    private void ConfigureShipEntry(GameObject entry, SpaceShipData data)
    {
        Image entryImage = entry.GetComponent<Image>();

        if (entryImage != null && data.ShipSprite != null)
        {
            entryImage.sprite = data.ShipSprite;
        }

        Button entryButton = entry.GetComponent<Button>();

        if (entryButton != null)
        {
            entryButton.onClick.AddListener(() => SelectShip(data));
        }
    }

    private void SelectShip(SpaceShipData shipData)
    {
        selectedShipData.ShipSprite = shipData.ShipSprite;
        selectedShipData.MaxHealth = shipData.MaxHealth;
        selectedShipData.Handling = shipData.Handling;
        selectedShipData.ScoreSpeed = shipData.ScoreSpeed;
        selectedShipData.ProjectileDamage = shipData.ProjectileDamage;
    }

    public void StartGameWithAccelerometer()
    {
        SceneGlobalManager.Instance.LoadGameWithResults();
    }

    public void StartGameWithGyroscope()
    {
        SceneManager.LoadScene(sceneWithGyroscope);
    }
}