using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FloatyMove : MonoBehaviour{
    public SteamVR_Action_Single lTriggerPull;
    public SteamVR_Behaviour_Pose lControllerPose;
    public SteamVR_Action_Single rTriggerPull;
    public SteamVR_Behaviour_Pose rControllerPose;
    public SteamVR_Action_Boolean lGripPull;
    public SteamVR_Action_Boolean rGripPull;
    public SteamVR_Action_Boolean menuPush;

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
    
    /* Used to move the player, velocity is opposite of where the player points the controller. */
    Vector3 movePlayer(SteamVR_Behaviour_Pose controllerPose, SteamVR_Action_Single triggerPull, Vector3 speed) {
        Vector3 orientation = -controllerPose.transform.forward; 
        float tmp = (velocity * 0.01f) * triggerPull.axis; // Trigger determines the strength of the thrust!
        if (speed.magnitude < velocity * 10f) {
            speed += orientation * tmp;
        }
        speed *= dampening;
        return speed;
    }

    // Update is called once per frame
    void Update() {
        lSpeed = movePlayer(lControllerPose, lTriggerPull, lSpeed);
        rSpeed = movePlayer(rControllerPose, rTriggerPull, rSpeed);
        Vector3 cameraPos = cameraRig.transform.position;
        if (lGripPull.state || rGripPull.state) {
            lSpeed *= .95f;
            rSpeed *= .95f;
        }
        cameraRig.transform.position = cameraPos + rSpeed + lSpeed;

        if (menuPush.state && !menuPush.lastState) {
            cameraRig.transform.position = Vector3.zero;
            lSpeed = Vector3.zero;
            rSpeed = Vector3.zero;
        }
    }
}
