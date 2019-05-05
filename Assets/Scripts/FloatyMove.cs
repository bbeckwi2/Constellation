using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FloatyMove : MonoBehaviour{
    public SteamVR_Action_Single lTriggerPull;
    public SteamVR_Behaviour_Pose lControllerPose;
    public SteamVR_Action_Single rTriggerPull;
    public SteamVR_Behaviour_Pose rControllerPose;
    public GameObject cameraRig;
    public float dampening = 0.99f;
    public float velocity = 0.01f;
    public float maxVelocity = 1f;

    private Vector3 lSpeed;
    private Vector3 rSpeed;

    // Start is called before the first frame update
    void Start() {
        lSpeed = new Vector3();
        rSpeed = new Vector3();
    }

    Vector3 movePlayer(SteamVR_Behaviour_Pose controllerPose, SteamVR_Action_Single triggerPull, Vector3 speed) {
        Vector3 orientation = -controllerPose.transform.forward; // (controllerPose.transform.forward - new Vector3(180f, 180f, 180f)).normalized;
        float tmp = (velocity * 0.01f) * triggerPull.axis;
        if (speed.magnitude < velocity * 10f) {
            speed += orientation * tmp;
        }
        speed *= dampening;
        /*
        if (triggerPull.axis > 0) {
            speed[0] = Mathf.Clamp(speed[0], -Mathf.Abs(maxVelocity * orientation[0]), Mathf.Abs(maxVelocity * orientation[0]));
            speed[1] = Mathf.Clamp(speed[1], -Mathf.Abs(maxVelocity * orientation[1]), Mathf.Abs(maxVelocity * orientation[1]));
            speed[2] = Mathf.Clamp(speed[2], -Mathf.Abs(maxVelocity * orientation[2]), Mathf.Abs(maxVelocity * orientation[2]));
        }
        */
        return speed;
    }

    // Update is called once per frame
    void Update() {
        lSpeed = movePlayer(lControllerPose, lTriggerPull, lSpeed);
        rSpeed = movePlayer(rControllerPose, rTriggerPull, rSpeed);
        //print("lSpeed: " + lSpeed);
        //print("rSpeed: " + rSpeed);
        Vector3 cameraPos = cameraRig.transform.position;
        cameraRig.transform.position = cameraPos + rSpeed + lSpeed;
    }
}
