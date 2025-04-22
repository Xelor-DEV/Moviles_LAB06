using UnityEngine;

[CreateAssetMenu(fileName = "Sensor Data", menuName = "ScriptableObjects/Settings/Sensor Data", order = 1)]
public class SensorData : ScriptableObject
{
    [Header("Sensor Mode")]
    [SerializeField] private SensorMode currentSensorMode = SensorMode.Gyroscope;


    [Header("Accelerometer Settings")]
    [SerializeField] private float accelerometerSensitivity = 12f;
    [SerializeField] private float accelerometerDeadzone = 0.15f;
    [SerializeField] private bool invertAccelerometer = false;  // Nuevo: para invertir controles
    [SerializeField] private ScreenOrientation accelerometerOrientation = ScreenOrientation.LandscapeLeft;
    [Header("Gyroscope Settings")]
    [SerializeField] private float gyroSensitivity = 2f;
    [SerializeField] private float gyroDeadzone = 0.15f;
    [SerializeField] private bool invertGyro = false;

    [Header("Runtime Data")]
    [SerializeField] private Vector3 rawAcceleration;
    [SerializeField] private Vector3 scaledAcceleration;
    [SerializeField] private Quaternion rawRotation;
    [SerializeField] private Vector3 scaledEulerRotation;

    // Propiedades públicas vinculadas a los campos serializados
    public Vector3 RawAcceleration
    {
        get => rawAcceleration;
        set => rawAcceleration = value;
    }

    public Vector3 ScaledAcceleration
    {
        get => scaledAcceleration;
        set => scaledAcceleration = value;
    }

    public Quaternion RawRotation
    {
        get => rawRotation;
        set => rawRotation = value;
    }

    public Vector3 ScaledEulerRotation
    {
        get => scaledEulerRotation;
        set => scaledEulerRotation = value;
    }

    public SensorMode CurrentSensorMode
    {
        get => currentSensorMode;
        set => currentSensorMode = value;
    }

    public void UpdateSensorData()
    {
        // Actualizar datos del acelerómetro
        RawAcceleration = Input.acceleration;
        ScaledAcceleration = ProcessAccelerometer(RawAcceleration);

        // Actualizar datos del giroscopio si está disponible
        if (SystemInfo.supportsGyroscope)
        {
            RawRotation = Input.gyro.attitude;
            ScaledEulerRotation = ProcessGyroscope(RawRotation.eulerAngles);
        }
    }

    private Vector3 ProcessAccelerometer(Vector3 input)
    {
        Vector3 orientedInput = input;
        switch (accelerometerOrientation)
        {
            case ScreenOrientation.LandscapeLeft:
                orientedInput = new Vector3(input.y, -input.x, input.z);
                break;
            case ScreenOrientation.LandscapeRight:
                orientedInput = new Vector3(-input.y, input.x, input.z);
                break;
            case ScreenOrientation.Portrait:
                orientedInput = new Vector3(input.x, input.y, input.z);
                break;
            case ScreenOrientation.PortraitUpsideDown:
                orientedInput = new Vector3(-input.x, -input.y, input.z);
                break;
        }

        // Aplicar sensibilidad e inversión
        Vector3 processed = orientedInput * accelerometerSensitivity;
        if (invertAccelerometer) processed.y *= -1;

        // Aplicar deadzone solo al eje Y (movimiento vertical)
        if (Mathf.Abs(processed.y) < accelerometerDeadzone) processed.y = 0;

        return processed;


    }

    private Vector3 ProcessGyroscope(Vector3 eulerAngles)
    {
        Vector3 normalizedAngles = new Vector3(
            NormalizeAngle(eulerAngles.x),
            NormalizeAngle(eulerAngles.y),
            NormalizeAngle(eulerAngles.z)
        );

        float yRotation = normalizedAngles.y * gyroSensitivity;
        if (invertGyro) yRotation *= -1;
        if (Mathf.Abs(yRotation) < gyroDeadzone) yRotation = 0;

        return new Vector3(0, yRotation, 0);
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) angle -= 360;
        return angle;
    }
}