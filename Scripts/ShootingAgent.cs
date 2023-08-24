using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ShootingAgent : Agent
{
    [SerializeField]
    private GameObject GunObject;
    [SerializeField]
    private GameObject TargetArea;
    [SerializeField]
    private GameObject GunTip;
    [SerializeField]
    private Camera GunCam;
    [SerializeField]
    private Timer timeScript;
    [SerializeField]
    public bool startedGame = false;

    private GameObject CurrentTarget;
    public float sensitivity = 0.3f;
    private Vector3 UpVectorTarget;
    private Rigidbody AgentRigidBody;
    private float smoothPitchChange = 0f;
    private float smoothYawChange = 0f;
    private float pitchSpeed = 80f;
    private float yawSpeed = 80f;  
    private float MaxPitchAngle = 80;
    private bool shootBool;
    float verticalCameraTransform = 0f;
    public Vector2 turn;
    private float mouseOffsetX =  0f;
    private float mouseOffsetY =  0f;
    int smooth = 1;
    public bool countDown = false;
    private bool stepStart = false;
    private float lastX;
    private float lastY;
    private Vector2 lastTurn;
    private Vector2 currentTargetPos;
    private float lastTargetDistanceX;
    private float lastTargetDistanceY;
    private float episodeCount = 0f;
    private float newTargetDistanceX;
    private float newTargetDistanceY;
    //private Vector2 newTargetDistance;
    private Quaternion rotationNeeded;
    Quaternion rotationWant;

     void Awake () {
        QualitySettings.vSyncCount = 0;  
        Application.targetFrameRate = 30;
    }

    public override void OnActionReceived(ActionBuffers actions) {
        if (countDown) return;
        //Debug.Log(actions.DiscreteActions[0]);
        shootBool = false;

        //Debug.Log("Yo");
        //Debug.Log(actions.DiscreteActions[0]);

        float pitchChange = actions.ContinuousActions[0];
        float yawChange = actions.ContinuousActions[1];

        lastX = turn.x;
        //lastY = turn.y;
        //lastTurn = new Vector2(turn.x, turn.y);

        //lastTargetDistance = (Quaternion.Inverse(rotationWant) * GunCam.transform.rotation).x;
        //lastTargetDistance = newTargetDistance;
        //Debug.Log("Last Transform: " + lastX);

        turn.x += pitchChange;
        turn.y += yawChange;
        turn.x = Mathf.Clamp(turn.x, -80f, 80f);
        turn.y = Mathf.Clamp(turn.y, -90f, 90f);

        GunCam.transform.localRotation = Quaternion.Euler(-turn.x, turn.y, 0);  

        newTargetDistanceX = Mathf.Abs((Quaternion.Inverse(rotationWant) * GunCam.transform.rotation).x);
        newTargetDistanceY = Mathf.Abs((Quaternion.Inverse(rotationWant) * GunCam.transform.rotation).y);
        
        //Debug.Log("New Distance: " + newTargetDistance);
        
        //Debug.Log("New Transform: " + turn.x);

        //lastTargetDistance = Vector2.Distance(lastTurn, currentTargetPos);
        //newTargetDistance = Vector2.Distance(turn, currentTargetPos);
        
        /*
        if (-lastTargetDistanceX < -newTargetDistanceX) {
            AddReward(-0.05f * Time.fixedDeltaTime);
            Debug.Log("BruhX");
            
            Debug.Log("Last Target Distance X: " + lastTargetDistanceX);
            Debug.Log("New Target Distance X: " + newTargetDistanceX);
            Debug.Log("Rotation Wanted: " + Quaternion.Inverse(rotationWant) * GunCam.transform.rotation);
        } else if (-lastTargetDistanceX > -newTargetDistanceX) {
            AddReward(0.05f * Time.deltaTime);
            Debug.Log("yuhX");

            Debug.Log("Last Target Distance X: " + lastTargetDistanceX);
            Debug.Log("New Target Distance X: " + newTargetDistanceX);
            Debug.Log("Rotation Wanted: " + Quaternion.Inverse(rotationWant) * GunCam.transform.rotation);
        }

        

        if (lastTargetDistanceY < newTargetDistanceY) {
            AddReward(-0.05f * Time.fixedDeltaTime);
            Debug.Log("BruhY");

            Debug.Log("Last Target Distance Y: " + lastTargetDistanceY);
            Debug.Log("New Target Distance Y: " + newTargetDistanceY);
        } else if (lastTargetDistanceY > newTargetDistanceY) {
            AddReward(0.05f * Time.fixedDeltaTime);
            Debug.Log("yuhY");

            Debug.Log("Last Target Distance Y: " + lastTargetDistanceY);
            Debug.Log("New Target Distance Y: " + newTargetDistanceY);
        }
        */
        
         
        //transform.Rotate(Vector3.up * yawChange);
        //gun.Rotate(Vector3.up * -pitchChange);

        switch(actions.DiscreteActions[0]) {
            case 1: shootBool = true; break;
            case 0: shootBool = false; break;
            default: break;
        }

        //if (actions.ContinuousActions[2] >= 0) shootBool = true;

        if (shootBool && Time.time >= GunObject.GetComponent<Gun>().nextFire) {
            GunObject.GetComponent<Gun>().nextFire = Time.time + 1f/15f; 
            GunObject.GetComponent<Gun>().Shoot();
        }

        //if (System.Convert.ToBoolean(actions.DiscreteActions[1])) StartCoroutine(GunObject.GetComponent<Gun>().Reload());
    }
    
    public override void OnEpisodeBegin()
    {
        //if (!trainingMode) return;
        
        //Debug.Log("New Episode");
        GunCam.transform.rotation = Quaternion.identity;
        GunObject.GetComponent<Gun>().ResetGame();
        turn.x = 0;
        turn.y = 0;

        episodeCount++;
        //mouseOffsetX = Input.GetAxis("Mouse X");
        //mouseOffsetY = Input.GetAxis("Mouse Y");
        //mouseOffset
        stepStart = true;

        foreach (Transform child in TargetArea.transform) {
            GameObject.Destroy(child.gameObject);
        }

        TargetArea.GetComponent<Spawn>().Respawn();
        CurrentTarget = TargetArea.transform.GetChild(0).gameObject;
        UpVectorTarget = CurrentTarget.GetComponent<Target>().TargetUpVector;
        StartCoroutine(Pause(3));
        //GunCam.transform.rotation = Quaternion.RotateTowards(GunCam.transform.localRotation, Quaternion.identity, smooth * Time.deltaTime);
        //Gun.score = 0;
    }

    public override void Initialize() {
        AgentRigidBody = GetComponent<Rigidbody>();
        //CurrentTarget = 

        //If Not Training Mode, No Max Step, Play Forever
        //if (!trainingMode) MaxStep = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (CurrentTarget == null) {
            CurrentTarget = TargetArea.transform.GetChild(0).gameObject;
            UpVectorTarget = CurrentTarget.GetComponent<Target>().TargetUpVector;
            currentTargetPos = new Vector2(currentTargetPos.x, currentTargetPos.y);
        }

        
        //sensor.AddObservation(GunObject.GetComponent<Gun>().shotsMade);

        //Vector3 toTarget = CurrentTarget.transform.position; //- GunTip.transform.position;
        //sensor.AddObservation(toTarget.normalized);

        //rotationNeeded = Quaternion.FromToRotation(GunCam.transform.position, CurrentTarget.transform.position);
        rotationWant = Quaternion.LookRotation(CurrentTarget.transform.position - GunCam.transform.position, Vector3.up);
        //GunCam.transform.rotation = rotation;

        //Actual Commet: Gets the rotation needed to look at cube
        sensor.AddObservation(rotationWant);

        //Current Rotation
        sensor.AddObservation(GunCam.transform.rotation.normalized);

        //Difference Between the Two Rotations desired and current
        lastTargetDistanceX = Mathf.Abs((Quaternion.Inverse(rotationWant) * GunCam.transform.rotation).x);
        lastTargetDistanceY = Mathf.Abs((Quaternion.Inverse(rotationWant) * GunCam.transform.rotation).y);
        sensor.AddObservation(Quaternion.Inverse(rotationWant) * GunCam.transform.rotation);
        //Debug.Log(GunCam.transform.rotation.normalized);

        RaycastHit hit;
        if (Physics.Raycast(GunObject.GetComponent<Gun>().fpsCamera.transform.position, GunObject.GetComponent<Gun>().fpsCamera.transform.forward, out hit, 100f)) {
            sensor.AddObservation(true);
        } else {
            sensor.AddObservation(false);
        }


        //lastTargetDistance = Vector2.Distance(turn, new Vector2(currentTargetPos.x, currentTargetPos.y));
        
        
        //RIP DOT PRODUCT CODE
        //lastTargetDistance = Vector3.Dot(GunTip.transform.forward.normalized, -UpVectorTarget.normalized);
        //sensor.AddObservation(Vector3.Dot(GunTip.transform.forward.normalized, -UpVectorTarget.normalized));




        //Debug.Log(rotation);
        //Debug.Log(Vector3.Dot(GunTip.transform.forward.normalized, -UpVectorTarget.normalized));
    }

    public override void Heuristic(in ActionBuffers Actions)
    {
        if (countDown) {
            //Debug.Log("Countdown Prevention Name");
            return;
        }

        ActionSegment<float> continuousActions = Actions.ContinuousActions;
        ActionSegment<int> discreteActions = Actions.DiscreteActions;
        
        continuousActions[0] = Input.GetAxis("Mouse Y") - mouseOffsetY;
        continuousActions[1] = Input.GetAxis("Mouse X") - mouseOffsetX;
        discreteActions[0] = 0;
        //discreteActions[1] = 0;
        
        if (GunObject.GetComponent<Gun>().reloading) {
            //Debug.Log("Reloading");
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= GunObject.GetComponent<Gun>().nextFire) {
            discreteActions[0] = 1;
            //Debug.Log("Shoot");
        }

        if (Input.GetButtonDown("Fire2") && GunObject.GetComponent<Gun>().ammoLeft < 30) {
            //discreteActions[1] = 1;
        }
    }
    
    private void FixedUpdate() {
        if (countDown) return;
        Academy.Instance.EnvironmentStep();
        RequestDecision();
        if (((3000 + Academy.Instance.StepCount) - (3000 * episodeCount)) >= 2900 && stepStart) {
            if (GunObject.GetComponent<Gun>().shotsMade <= 10) {
                AddReward(-5f);
                Debug.Log("Not Enough Shots");
                stepStart = false;
            }
        }
    }

    void Start()
    {
        //Set To Negative One for WebGL
        Application.targetFrameRate = 60;
    }

    private IEnumerator Pause(int p) {
        Time.timeScale = 0.5f;
        float pauseEndTime = Time.realtimeSinceStartup + p;
        var emission = GunObject.GetComponent<Gun>().muzzleFlash.emission;    
        emission.enabled = false;
        //Academy.Instance.AutomaticSteppingEnabled = false;
        timeScript.activateTimer(3f);
        countDown = true;

        yield return new WaitForSeconds(3f);

        Time.timeScale = 1f;
        emission.enabled = true;
        //Academy.Instance.AutomaticSteppingEnabled = true;
        countDown = false;
    }

}
