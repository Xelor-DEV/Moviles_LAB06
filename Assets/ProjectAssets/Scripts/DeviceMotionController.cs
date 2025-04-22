using UnityEngine;

public enum SensorMode
{
    Auto,
    Gyroscope,
    Accelerometer
}
public class DeviceMotionController : MonoBehaviour
{
    [Header("Sensor Config")]
    [SerializeField] private SensorData sensorData;
    [SerializeField] private SensorMode inputMode = SensorMode.Auto;
    [SerializeField] private Vector3 accelerometerScale = new Vector3(1, 1.5f, 1);
    [SerializeField] private Vector3 gyroscopeEulerScale = new Vector3(1, 1.5f, 1);

    [Header("Debug Values")]
    [SerializeField] private SensorMode currentMode;
    [SerializeField] private Vector3 debugAcceleration;
    [SerializeField] private Vector3 debugScaledAcceleration;
    [SerializeField] private Quaternion debugGyroRotation;
    [SerializeField] private Vector3 debugScaledEuler;

    [Header("Calibration")]
    [SerializeField] private float gyroDeadzone = 0.1f;
    [SerializeField] private bool invertGyroY = true; // Para ajustar dirección

    private Gyroscope deviceGyroscope;
    private bool gyroSupported;
    private SensorMode lastValidMode;
    private Quaternion gyroBaseRotation = Quaternion.identity;

    private void Start()
    {
        gyroSupported = SystemInfo.supportsGyroscope;
        ValidateSensorMode();
        InitializeSensors();

        if (currentMode == SensorMode.Gyroscope)
        {
            CalibrateGyro();
        }
    }
    private void CalibrateGyro()
    {
        gyroBaseRotation = Quaternion.Inverse(Input.gyro.attitude);
    }

    private void ValidateSensorMode()
    {
        if (inputMode == SensorMode.Auto)
        {
            if (gyroSupported == true)
            {
                currentMode = SensorMode.Gyroscope;
            }
            else
            {
                currentMode = SensorMode.Accelerometer;
            }
        }
        else if (inputMode == SensorMode.Gyroscope)
        {
            if (gyroSupported == true)
            {
                currentMode = SensorMode.Gyroscope;
            }
            else
            {
                Debug.LogWarning("Gyroscope not available. Using accelerometer.");
                inputMode = SensorMode.Accelerometer;
                currentMode = SensorMode.Accelerometer;
            }
        }
        else
        {
            currentMode = SensorMode.Accelerometer;
        }

        if (currentMode == SensorMode.Gyroscope && gyroSupported == false)
        {
            currentMode = SensorMode.Accelerometer;
        }

        lastValidMode = currentMode;
        //sensorData.CurrentSensorMode = currentMode;
    }

    private void InitializeSensors()
    {
        if (gyroSupported == true)
        {
            deviceGyroscope = Input.gyro;
            if (currentMode == SensorMode.Gyroscope)
            {
                deviceGyroscope.enabled = true;
            }
            else
            {
                deviceGyroscope.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (inputMode != lastValidMode || currentMode != lastValidMode)
        {
            ValidateSensorMode();
            InitializeSensors();
        }

        UpdateSensorData();
        UpdateDebugValues();
    }

    private void UpdateSensorData()
    {
        if (currentMode == SensorMode.Gyroscope)
        {
            UpdateGyroData();
        }
        else
        {
            UpdateAccelerometerData();
        }
    }

    private void UpdateGyroData()
    {
        if (!deviceGyroscope.enabled)
        {
            deviceGyroscope.enabled = true;
            return;
        }
        Quaternion rot = gyroBaseRotation * deviceGyroscope.attitude;
        Vector3 euler = rot.eulerAngles;

        euler.x = NormalizeAngle(euler.x);
        euler.y = NormalizeAngle(euler.y);
        euler.z = NormalizeAngle(euler.z);
        Vector3 scaled = Vector3.Scale(euler, gyroscopeEulerScale);

        if (Mathf.Abs(scaled.y) < gyroDeadzone) scaled.y = 0;

        // Invertir eje Y si es necesario
        if (invertGyroY) scaled.y *= -1;

       sensorData.RawRotation = rot;
       sensorData.ScaledEulerRotation = scaled;
       //
       
 
    }

    private void UpdateAccelerometerData()
    {
        Vector3 rawAccel = Input.acceleration;

        // Ajuste para orientación landscape
        switch (Screen.orientation)
        {
            case ScreenOrientation.LandscapeLeft:
                // Mapeo correcto para movimiento vertical natural
                rawAccel = new Vector3(rawAccel.x, rawAccel.z, rawAccel.y);
                break;

            case ScreenOrientation.LandscapeRight:
                rawAccel = new Vector3(-rawAccel.x, rawAccel.z, -rawAccel.y);
                break;

            case ScreenOrientation.PortraitUpsideDown:
                rawAccel = new Vector3(-rawAccel.y, -rawAccel.x, rawAccel.z);
                break;
        }

        sensorData.RawAcceleration = rawAccel;
        sensorData.ScaledAcceleration = Vector3.Scale(rawAccel, accelerometerScale);
    }

    private void UpdateDebugValues()
    {
        debugAcceleration = sensorData.RawAcceleration;
        debugScaledAcceleration = sensorData.ScaledAcceleration;
        debugGyroRotation = sensorData.RawRotation;
        debugScaledEuler = sensorData.ScaledEulerRotation;
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) angle -= 360;
        return angle;
    }
}
