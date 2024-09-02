using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Varjo.XR;

public enum GazeDataSource
{
    InputSubsystem,
    GazeAPI
}

public class EyeTracking : MonoBehaviour
{
    [Header("Gaze data")]
    public GazeDataSource gazeDataSource = GazeDataSource.InputSubsystem;

    [Header("Gaze calibration settings")]
    public VarjoEyeTracking.GazeCalibrationMode gazeCalibrationMode = VarjoEyeTracking.GazeCalibrationMode.Fast; //5 dot calibration
    public KeyCode calibrationRequestKey = KeyCode.Space;

    [Header("Gaze output filter settings")]
    public VarjoEyeTracking.GazeOutputFilterType gazeOutputFilterType = VarjoEyeTracking.GazeOutputFilterType.Standard;
    public KeyCode setOutputFilterTypeKey = KeyCode.RightShift;

    [Header("Gaze data output frequency")]
    public VarjoEyeTracking.GazeOutputFrequency frequency;

    [Header("Toggle gaze target visibility")]
    public KeyCode toggleGazeTarget = KeyCode.Return;

    [Header("Debug Gaze")]
    public KeyCode checkGazeAllowed = KeyCode.PageUp;
    public KeyCode checkGazeCalibrated = KeyCode.PageDown;

    [Header("Toggle fixation point indicator visibility")]
    public bool showFixationPoint = true;

    [Header("Visualization Transforms")]
    public Transform fixationPointTransform;
    public Transform leftEyeTransform;
    public Transform rightEyeTransform;

    [Header("XR camera")]
    public Camera xrCamera;

    [Header("Gaze point indicator")]
    public GameObject gazeTarget;

    [Header("Gaze ray radius")]
    public float gazeRadius = 0.01f;

    [Header("Gaze point distance if not hit anything")]
    public float floatingGazeTargetDistance = 5f;

    [Header("Gaze target offset towards viewer")]
    public float targetOffset = 0.2f;

    [Header("Amout of force give to freerotating objects at point where user is looking")]
    public float hitForce = 5f;

    [Header("Gaze data logging")]
    public KeyCode loggingToggleKey = KeyCode.RightControl; //Not in use

    [Header("Default path is Logs under application data path.")]
    public bool useCustomLogPath = false;
    public string customLogPath = "";
    public int nextCase;

    [Header("Print gaze data framerate while logging.")]
    public bool printFramerate = false;

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private Eyes eyes;
    private VarjoEyeTracking.GazeData gazeData;
    private List<VarjoEyeTracking.GazeData> dataSinceLastUpdate;
    private List<VarjoEyeTracking.EyeMeasurements> eyeMeasurementsSinceLastUpdate;
    private Vector3 leftEyePosition;
    private Vector3 rightEyePosition;
    private Quaternion leftEyeRotation;
    private Quaternion rightEyeRotation;
    private Vector3 fixationPoint;
    private Vector3 direction;
    private Vector3 rayOrigin;
    private RaycastHit hit;
    private float distance;
    private StreamWriter writer = null;
    private bool logging = false;

    private static readonly string[] ColumnNames = { "Frame", "CaptureTime", "LogTime", "HMDPosition", "HMDRotation", "GazeStatus", "CombinedGazeForward", "CombinedGazePosition", "InterPupillaryDistanceInMM", "LeftEyeStatus", "LeftEyeForward", "LeftEyePosition", "LeftPupilIrisDiameterRatio", "LeftPupilDiameterInMM", "LeftIrisDiameterInMM", "RightEyeStatus", "RightEyeForward", "RightEyePosition", "RightPupilIrisDiameterRatio", "RightPupilDiameterInMM", "RightIrisDiameterInMM", "FocusDistance", "FocusStability", "Target On" };
    private const string ValidString = "VALID";
    private const string InvalidString = "INVALID";

    int gazeDataCount = 0;
    float gazeTimer = 0f;

    public GameObject controller_object;
    public bool loggingButton;
    private Controller controller;

    public Vector3 gazeforward;

    
    private GameObject left1;
    private GameObject right1;
    private GameObject left2;
    private GameObject right2;
    private GameObject left3;
    private GameObject right3;
    private GameObject fixation;
    private int target_on;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XRNode.CenterEye, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    private void Awake(){ 
        
        left1 = GameObject.Find("L1");
        right1 = GameObject.Find("R1");
        left2 = GameObject.Find("L2");
        right2 = GameObject.Find("R2");
        left3 = GameObject.Find("L3");
        right3 = GameObject.Find("R3");

        fixation = GameObject.Find("Chicken");

    }

    private void Start()
    {
        VarjoEyeTracking.SetGazeOutputFrequency(frequency);
        
        if (VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated())
        {
            gazeTarget.SetActive(true);
        }
        else
        {
            gazeTarget.SetActive(false);
        }

        if (showFixationPoint)
        {
            fixationPointTransform.gameObject.SetActive(true);
        }
        else
        {
            fixationPointTransform.gameObject.SetActive(false);
        }

        //left1 = GameObject.Find("L5");
        //right1 = GameObject.Find("R5");
        //left2 = GameObject.Find("L12");
        //right2 = GameObject.Find("R12");
        //left3 = GameObject.Find("L20");
        //right3 = GameObject.Find("R20");

        //fixation = GameObject.Find("Fixation_cross");

        controller = controller_object.GetComponent<Controller>();
    }

    void Update()
    {
        //if(left1.activeSelf == true){

            //target_on = 1;

        //}else if(right1.activeSelf == true){

            //target_on = 2;

        //}else if(left2.activeSelf == true){

            //target_on = 3;

        //}else if(right2.activeSelf == true){

            //target_on = 4;

        //}else if(left3.activeSelf == true){

            //target_on = 6;

        //}else if(right3.activeSelf == true){

            //target_on = 5;

        //}else{

        target_on = 0;
            //loggingButton = controller.gripButton; 

        //}

        loggingButton = controller.primary2DAxisClick;

        if (logging && printFramerate)
        {
            gazeTimer += Time.deltaTime;
            if (gazeTimer >= 1.0f)
            {
                //Debug.Log("Gaze data rows per second: " + gazeDataCount);
                gazeDataCount = 0;
                gazeTimer = 0f;
            }
        }


        ////////////////////////////////
        // Action Functions Key Codes //
        ////////////////////////////////

        // Request gaze calibration
        if (Input.GetKeyDown(calibrationRequestKey))
        {
            VarjoEyeTracking.RequestGazeCalibration(gazeCalibrationMode);  //5 dot calibration
        }

        // Set output filter type
        if (Input.GetKeyDown(setOutputFilterTypeKey))
        {
            VarjoEyeTracking.SetGazeOutputFilterType(gazeOutputFilterType);
            Debug.Log("Gaze output filter type is now: " + VarjoEyeTracking.GetGazeOutputFilterType());
        }

        // Check if gaze is allowed
        if (Input.GetKeyDown(checkGazeAllowed))
        {
            Debug.Log("Gaze allowed: " + VarjoEyeTracking.IsGazeAllowed());
        }

        // Check if gaze is calibrated
        if (Input.GetKeyDown(checkGazeCalibrated))
        {
            Debug.Log("Gaze calibrated: " + VarjoEyeTracking.IsGazeCalibrated());
        }

        // Toggle gaze target visibility
        if (Input.GetKeyDown(toggleGazeTarget))
        {
            gazeTarget.GetComponentInChildren<MeshRenderer>().enabled = !gazeTarget.GetComponentInChildren<MeshRenderer>().enabled;
        }

        // Get gaze data if gaze is allowed and calibrated
        if (VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated())
        {
            //Get device if not valid
            if (!device.isValid)
            {
                GetDevice();
            }

            // Show gaze target
            gazeTarget.SetActive(true);

            if (gazeDataSource == GazeDataSource.InputSubsystem)
            {

                if (device.TryGetFeatureValue(CommonUsages.eyesData, out eyes))
                {
                    if (eyes.TryGetLeftEyePosition(out leftEyePosition))
                    {
                        leftEyeTransform.localPosition = leftEyePosition;
                    }

                    if (eyes.TryGetLeftEyeRotation(out leftEyeRotation))
                    {
                        leftEyeTransform.localRotation = leftEyeRotation;
                    }

                    if (eyes.TryGetRightEyePosition(out rightEyePosition))
                    {
                        rightEyeTransform.localPosition = rightEyePosition;
                    }

                    if (eyes.TryGetRightEyeRotation(out rightEyeRotation))
                    {
                        rightEyeTransform.localRotation = rightEyeRotation;
                    }

                    if (eyes.TryGetFixationPoint(out fixationPoint))
                    {
                        fixationPointTransform.localPosition = fixationPoint;
                    }
                }

                // Set raycast origin point to VR camera position
                rayOrigin = xrCamera.transform.position;

                // Direction from VR camera towards fixation point
                direction = (fixationPointTransform.position - xrCamera.transform.position).normalized;

            } else
            {
                gazeData = VarjoEyeTracking.GetGaze();

                if (gazeData.status != VarjoEyeTracking.GazeStatus.Invalid)
                {
                    // GazeRay vectors are relative to the HMD pose so they need to be transformed to world space
                    if (gazeData.leftStatus != VarjoEyeTracking.GazeEyeStatus.Invalid)
                    {
                        leftEyeTransform.position = xrCamera.transform.TransformPoint(gazeData.left.origin);
                        leftEyeTransform.rotation = Quaternion.LookRotation(xrCamera.transform.TransformDirection(gazeData.left.forward));
                    }

                    if (gazeData.rightStatus != VarjoEyeTracking.GazeEyeStatus.Invalid)
                    {
                        rightEyeTransform.position = xrCamera.transform.TransformPoint(gazeData.right.origin);
                        rightEyeTransform.rotation = Quaternion.LookRotation(xrCamera.transform.TransformDirection(gazeData.right.forward));
                    }

                    // Set gaze origin as raycast origin
                    rayOrigin = xrCamera.transform.TransformPoint(gazeData.gaze.origin);

                    // Set gaze direction as raycast direction
                    direction = xrCamera.transform.TransformDirection(gazeData.gaze.forward);

                    // Fixation point can be calculated using ray origin, direction and focus distance
                    fixationPointTransform.position = rayOrigin + direction * gazeData.focusDistance;
                }
            }
        }

        // Raycast to world from VR Camera position towards fixation point
        if (Physics.SphereCast(rayOrigin, gazeRadius, direction, out hit))
        {
            // Put target on gaze raycast position with offset towards user
            gazeTarget.transform.position = hit.point - direction * targetOffset;

            // Make gaze target point towards user
            gazeTarget.transform.LookAt(rayOrigin, Vector3.up);

            // Scale gazetarget with distance so it apperas to be always same size
            distance = hit.distance;
            gazeTarget.transform.localScale = Vector3.one * distance;

            // Prefer layers or tags to identify looked objects in your application
            // This is done here using GetComponent for the sake of clarity as an example
            
            //RotateWithGaze rotateWithGaze = hit.collider.gameObject.GetComponent<RotateWithGaze>();
            //if (rotateWithGaze != null)
            //{
                //rotateWithGaze.RayHit();
            //}

            // Alternative way to check if you hit object with tag
            //if (hit.transform.CompareTag("FreeRotating"))
            //{
                //AddForceAtHitPosition();
            //}
        }
        else
        {
            // If gaze ray didn't hit anything, the gaze target is shown at fixed distance
            gazeTarget.transform.position = rayOrigin + direction * floatingGazeTargetDistance;
            gazeTarget.transform.LookAt(rayOrigin, Vector3.up);
            gazeTarget.transform.localScale = Vector3.one * floatingGazeTargetDistance;
        }

        if(loggingButton) 
        {

            if (!logging)
            {
                Debug.Log("Logkey detected, logging");
                StartLogging();
            }

            return;
        }

        if (logging)
        {
            int dataCount = VarjoEyeTracking.GetGazeList(out dataSinceLastUpdate, out eyeMeasurementsSinceLastUpdate);
            if (printFramerate) gazeDataCount += dataCount;
            for (int i = 0; i < dataCount; i++)
            {
                LogGazeData(dataSinceLastUpdate[i], eyeMeasurementsSinceLastUpdate[i]);
            }
        }
    }

    void AddForceAtHitPosition()
    {
        //Get Rigidbody form hit object and add force on hit position
        Rigidbody rb = hit.rigidbody;
        if (rb != null)
        {
            rb.AddForceAtPosition(direction * hitForce, hit.point, ForceMode.Force);
        }
    }


    void LogGazeData(VarjoEyeTracking.GazeData data, VarjoEyeTracking.EyeMeasurements eyeMeasurements)
    {
        string[] logData = new string[24]; 

        // Gaze data frame number
        logData[0] = data.frameNumber.ToString();

        // Gaze data capture time
        logData[1] = data.captureTime.ToString();

        // Log time
        logData[2] = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();

        // HMD
        logData[3] = xrCamera.transform.localPosition.ToString("F3");
        logData[4] = xrCamera.transform.localRotation.ToString("F3");
        //Debug.Log(xrCamera.transform.localPosition.ToString("F3"));
        Debug.Log(xrCamera.transform.localRotation.eulerAngles.ToString("F3"));
        Debug.Log(xrCamera.transform.rotation.eulerAngles.ToString("F3"));


        // Combined gaze
        bool invalid = data.status == VarjoEyeTracking.GazeStatus.Invalid;
        logData[5] = invalid ? InvalidString : ValidString;
        logData[6] = invalid ? "" : data.gaze.forward.ToString("F3");
        gazeforward = data.gaze.forward; 
        //Debug.Log("ET:"+gazeforward.ToString("F3"));
        logData[7] = invalid ? "" : data.gaze.origin.ToString("F3");

        // IPD
        logData[8] = invalid ? "" : eyeMeasurements.interPupillaryDistanceInMM.ToString("F3");

        // Left eye
        bool leftInvalid = data.leftStatus == VarjoEyeTracking.GazeEyeStatus.Invalid;
        logData[9] = leftInvalid ? InvalidString : ValidString;
        logData[10] = leftInvalid ? "" : data.left.forward.ToString("F3");
        logData[11] = leftInvalid ? "" : data.left.origin.ToString("F3");
        logData[12] = leftInvalid ? "" : eyeMeasurements.leftPupilIrisDiameterRatio.ToString("F3");
        logData[13] = leftInvalid ? "" : eyeMeasurements.leftPupilDiameterInMM.ToString("F3");
        logData[14] = leftInvalid ? "" : eyeMeasurements.leftIrisDiameterInMM.ToString("F3");

        // Right eye
        bool rightInvalid = data.rightStatus == VarjoEyeTracking.GazeEyeStatus.Invalid;
        logData[15] = rightInvalid ? InvalidString : ValidString;
        logData[16] = rightInvalid ? "" : data.right.forward.ToString("F3");
        logData[17] = rightInvalid ? "" : data.right.origin.ToString("F3");
        logData[18] = rightInvalid ? "" : eyeMeasurements.rightPupilIrisDiameterRatio.ToString("F3");
        logData[19] = rightInvalid ? "" : eyeMeasurements.rightPupilDiameterInMM.ToString("F3");
        logData[20] = rightInvalid ? "" : eyeMeasurements.rightIrisDiameterInMM.ToString("F3");

        // Focus
        logData[21] = invalid ? "" : data.focusDistance.ToString();
        logData[22] = invalid ? "" : data.focusStability.ToString();

        // Target Onset
        logData[23] = target_on.ToString();

        Log(logData);
    }

    void Log(string[] values)
    {
        if (!logging || writer == null)
            return;

        string line = "";
        for (int i = 0; i < values.Length; ++i)
        {
            values[i] = values[i].Replace("\r", "").Replace("\n", ""); 
            line += values[i] + (i == (values.Length - 1) ? "" : ";");
        }
        writer.WriteLine(line);
    }

    // Initialize Logging
    public void StartLogging()
    {
        if (logging)
        {
            Debug.LogWarning("Logging was on when StartLogging was called. No new log was started.");
            return;
        }

        logging = true;


        string logPath; 

        if(useCustomLogPath == true){

            logPath = Application.dataPath + "/Logs/game/" + customLogPath + "/";

        }else{

            logPath = useCustomLogPath ? customLogPath : Application.dataPath + "/Logs/"; 
        }

        Directory.CreateDirectory(logPath);

        DateTime now = DateTime.Now;
        string fileName; 

        if(useCustomLogPath == true){

            
            fileName = nextCase + "_" + customLogPath + "_" + string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute); 

        }else{

            fileName = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute); 
        }

        string path = logPath + fileName + ".csv";
        writer = new StreamWriter(path);

        Log(ColumnNames);
        Debug.Log("Log file started at: " + path);
    }

    void StopLogging()
    {
        if (!logging)
            return;

        if (writer != null)
        {
            writer.Flush();
            writer.Close();
            writer = null;
        }
        logging = false;
        Debug.Log("Logging ended");
    }

    void OnApplicationQuit()
    {
        StopLogging();
    }
}