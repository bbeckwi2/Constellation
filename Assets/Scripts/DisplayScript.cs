using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DisplayScript : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean isClick;
    public SteamVR_Action_Boolean isTouch;
    public SteamVR_Action_Vector2 padPos;
    public SteamVR_Action_Boolean grabSelection;

    public GameObject textObject;
    private TextDisplay textDisplay;

    public GameObject headObject;
    public GameObject linePrefab;
    private GameObject line;
    private Transform lineTransform;
    private GameObject lastTouched;
    private int missCount = 0;

    public LayerMask layerMask;

    private float alpha = 0.0f;
    public float alphaIncrement = 1f;

    private List<GameObject> selection;

    void Start() {
        line = Instantiate(linePrefab);
        lineTransform = line.transform;
        textDisplay = textObject.GetComponent<TextDisplay>();
        selection = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {
        /* This is for both style and user feedback */
        if (isTouch.state && alpha < 255f) {
            alpha += alphaIncrement;
        } else if (alpha > 0f) {
            alpha -= alphaIncrement;
        }

        /* This handles the laser that shoots out of the controller and how it collides with things */
        if (alpha > 0) {

            RaycastHit hit;

            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 20, layerMask)) {
                showLine(hit.point, hit.distance);
                GameObject o = hit.transform.gameObject;
                if (o.GetComponent<NormalNode>() != null) {
                    if (lastTouched != null && !lastTouched.Equals(o)) {

                    } else {
                        o.GetComponent<NormalNode>().tempColor(this.line.GetComponent<Renderer>().material.color, 1f);
                        lastTouched = o;
                        displayInfo(o.GetComponent<NormalNode>().getInfo());
                    }
                }
            } else {
                showLine(controllerPose.transform.position + transform.forward * 10000f, 10000f);               
                if (lastTouched != null) {
                    lastTouched = null;
                }
                if (textObject.activeSelf) {
                    hideInfo();
                }
            }
            if (textObject.activeSelf) {
                textObject.transform.position = controllerPose.transform.position + Vector3.up * 0.1f;
                textObject.transform.LookAt(headObject.transform);
            }
        } else {
            line.SetActive(false);
            hideInfo();
        }

        if (lastTouched != null && grabSelection.state && !grabSelection.lastState) {
            if (selection.Contains(lastTouched)) {
                selection.Remove(lastTouched);
            } else {
                selection.Add(lastTouched);
            }
        }

        foreach (GameObject o in selection) {
            o.GetComponent<NormalNode>().tempColor(this.line.GetComponent<Renderer>().material.color, 1f);
        }

    }

    private void displayInfo(NodeInfo info) {
        textObject.SetActive(true);
        textDisplay.setTitle(info.name);
        textDisplay.setText(textDisplay.formatTextForMain(info.details));
    }

    private void hideInfo() {
        textObject.SetActive(false);
    }

    private void showLine(Vector3 point, float distance) {
        line.SetActive(true);
        Color c = line.GetComponent<Renderer>().material.color;
        c.a = alpha;
        line.GetComponent<Renderer>().material.color = c;
        lineTransform.position = Vector3.Lerp(controllerPose.transform.position, point, 0.5f);
        lineTransform.LookAt(point);
        lineTransform.localScale = new Vector3(lineTransform.localScale.x, lineTransform.localScale.y, distance);

    }
}
