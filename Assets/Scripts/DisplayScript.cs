using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DisplayScript : MonoBehaviour
{
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean isClick;
    public SteamVR_Action_Boolean isTouch;
    public SteamVR_Action_Vector2 padPos;

    public GameObject linePrefab;
    private GameObject line;
    private Transform lineTransform;

    private float alpha = 0.0f;
    public float alphaIncrement = 0.01f;

    void Start() {
        line = Instantiate(linePrefab);
        lineTransform = line.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if ((isTouch.state || isTouch.lastState) && alpha < 255f) {
            alpha += alphaIncrement;
        } else if (alpha > 0f) {
            alpha -= alphaIncrement;
        }
        print(alpha);

        if (alpha > 0) {
            RaycastHit hit;
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100)) {
                ShowLine(hit.point, hit.distance);
                GameObject o = hit.transform.gameObject;
                if (o.GetComponent<NormalNode>() != null) {
                    print("Has node!");
                } else {
                    print("Missing node!");
                }
            } else {
                ShowLine(controllerPose.transform.position + transform.forward * 10000f, 10000f);               
            }
        } else {
            line.SetActive(false);
        }
    }

    private void ShowLine(Vector3 point, float distance) {
        line.SetActive(true);
        Color c = line.GetComponent<Renderer>().material.color;
        c.a = alpha;
        line.GetComponent<Renderer>().material.color = c;
        lineTransform.position = Vector3.Lerp(controllerPose.transform.position, point, 0.5f);
        lineTransform.LookAt(point);
        lineTransform.localScale = new Vector3(lineTransform.localScale.x, lineTransform.localScale.y, distance);

    }
}
