using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Gyro Controls")]
    [SerializeField] private float maxTiltAngle = 30f;

    [Header("Dependencies")]
    [SerializeField] private SpaceShipData spaceShipData;
    [SerializeField] private HealthData healthData;
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private SensorData sensorData;

    [Header("Movement Settings")]
    [SerializeField] private float movementSmoothing = 6f;
    [SerializeField] private float verticalLimit = 4f;
    [SerializeField] private float inputResponseCurve = 1.5f;
    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float scoreInterval = 1f;
    private bool isFiring;

    [Header("Gyro Settings")]
    [SerializeField] private float gyroSensitivity = 3f;
    [SerializeField] private float gyroDeadzone = 0.05f;
    [SerializeField] private bool invertGyro = false;
    private float nextFireTime;

    [Header("Pooling")]
    [SerializeField] private ObjectPoolDynamic projectilePool;

    private float targetYPosition;
    private Transform shipTransform;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private Coroutine firingCoroutine;


    public HealthData HealthData
    {
        get
        {
            return healthData;
        }
    }

    private void Awake()
    {
        shipTransform = transform;
        mainCamera = Camera.main;
        healthData.Initialize(spaceShipData.MaxHealth);
        scoreData.ResetCurrentScore();
    }

    private void Start()
    {
        CalculateMovementLimits();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spaceShipData.ShipSprite;
        }

        StartCoroutine(ScoreUpdateRoutine());
    }

    public void StartFiring()
    {
        if (!isFiring)
        {
            isFiring = true;
            firingCoroutine = StartCoroutine(FiringRoutine());
        }
    }

    public void StopFiring()
    {
        isFiring = false;
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    private IEnumerator FiringRoutine()
    {
        while (isFiring)
        {
            FireProjectile();
            yield return new WaitForSeconds(spaceShipData.FireRate);
        }
    }


    private IEnumerator ScoreUpdateRoutine()
    {
        scoreData.CurrentScore = scoreData.CurrentScore + spaceShipData.ScoreSpeed;
        yield return new WaitForSeconds(scoreInterval);
        StartCoroutine(ScoreUpdateRoutine());
    }

    private void FireProjectile()
    {
        GameObject projectile = projectilePool.GetObject(transform.position, projectilePool.ObjPrefab.transform.rotation);
        projectile.GetComponent<Projectile>().Initialize(spaceShipData.ProjectileDamage);
    }

    private void CalculateMovementLimits()
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(shipTransform.position);
        verticalLimit = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, viewportPoint.z)).y;
    }

    private void Update()
    {
        sensorData.UpdateSensorData();
        HandleMovementInput();
        ApplyMovement();
    }

    private void HandleMovementInput()
    {
        float input = GetSensorInput();

        // Mapeo directo del input a la posición (ajusta el 5f según necesites)
        targetYPosition += input * spaceShipData.Handling * Time.deltaTime * 5f;

        // Limitar posición
        targetYPosition = Mathf.Clamp(targetYPosition, -verticalLimit, verticalLimit);
    }

    private float GetSensorInput()
    {

        switch (sensorData.CurrentSensorMode)
        {
            case SensorMode.Accelerometer:
                return sensorData.ScaledAcceleration.y; // Usa el eje Y procesado

            case SensorMode.Gyroscope:
                return sensorData.ScaledEulerRotation.y / 90f; // Giroscopio normalizado

            default:
                return 0f;
        }

    }

    private void ApplyMovement()
    {
        Vector3 newPosition = shipTransform.position;
        newPosition.y = Mathf.Lerp(newPosition.y, targetYPosition, movementSmoothing * Time.deltaTime);
        shipTransform.position = newPosition;
    }

}