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

    public GameObject customNodeFab;

    public float middleButtonRad = 0.4f;

    public GameObject ConstellationSpawner;

    public LayerMask noTouch;
    public LayerMask layerMask;

    private float alpha = 0.0f;
    public float alphaIncrement = 1f;

    public float outerLimit = 0.1f;
    public float innerLimit = 0.01f;
    public float middleLimit = 0.05f;

    private Color originalColor;

    public Color deleteColor = new Color(1f, 0f, 0f);

    private float rad = 0.05f;

    private float circleDeg = 0.0f;

    private Dictionary<GameObject, GameObject> selection;

    void Start() {
        line = Instantiate(linePrefab);
        lineTransform = line.transform;
        textDisplay = textObject.GetComponent<TextDisplay>();
        selection = new Dictionary<GameObject, GameObject>();
        originalColor = line.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update() {

        foreach (GameObject o in selection.Keys) {
            o.GetComponent<NormalNode>().tempColor(originalColor, 1f);
        }

        /* This is for both style and user feedback */
        if ((getTouchType() == TButtonType.middle || getTouchType() == TButtonType.left) && alpha < 255f) {
            alpha += alphaIncrement;
        } else if (alpha > 0f) {
            alpha -= alphaIncrement;
        }

        if (getTouchType() == TButtonType.left) {
            line.GetComponent<Renderer>().material.color = deleteColor;
        } else {
            line.GetComponent<Renderer>().material.color = originalColor;
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
                        o.GetComponent<NormalNode>().tempColor(line.GetComponent<Renderer>().material.color, 1f);
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

        if (getTouchType() == TButtonType.bottom) {
            if (rad < outerLimit) 
                rad = Mathf.Min(rad + 0.001f, outerLimit);
        } else if (getTouchType() == TButtonType.top) {
            if (rad > innerLimit) 
                rad = Mathf.Max(rad - 0.001f, innerLimit);
        } else {
            if (rad > middleLimit) {
                rad = Mathf.Max(rad - 0.001f, middleLimit);
            } else if (rad < middleLimit) {
                rad = Mathf.Min(rad + 0.001f, middleLimit);
            }
        }

        if (isClick.state && !isClick.lastState ) {
            if (lastTouched != null && getButtonPress() == TButtonType.middle) {
                if (selection.ContainsKey(lastTouched)) {
                    selection[lastTouched].GetComponent<NormalNode>().remove();
                    selection.Remove(lastTouched);
                } else {
                    GameObject copy = Instantiate(lastTouched);
                    NormalNode nCopy = copy.GetComponent<NormalNode>();
                    nCopy.setColor(lastTouched.GetComponent<NormalNode>().getColor());
                    nCopy.size = 0.02f;
                    nCopy.nodeGrowthRate = 0.001f;
                    copy.layer = noTouch;
                    nCopy.init(lastTouched.GetComponent<NormalNode>().getInfo());
                    copy.transform.parent = controllerPose.transform;
                    selection.Add(lastTouched, copy);
                }
            }

            if (lastTouched != null && getButtonPress() == TButtonType.left) {
                lastTouched.GetComponent<NormalNode>().remove();
                foreach (GameObject o in selection.Values) {
                    o.GetComponent<NormalNode>().remove();
                }
                selection.Clear();
            }

            if (getButtonPress() == TButtonType.bottom) {
                foreach (GameObject o in selection.Values) {
                    o.GetComponent<NormalNode>().remove();
                }
                selection.Clear();
            }

            if (getButtonPress() == TButtonType.top) {
                spawnConstellation();
            }
        }

        print(getTouchType());
        animateSelected();
        circleDeg = (circleDeg + 1f) % 360f;
    }

    private TButtonType getButtonPress() {
        if (!this.isClick.state) {
            return TButtonType.none;
        }
        return getTouchType();
    }

    private void spawnConstellation() {

        if (selection.Count <= 0) {
            return;
        }

        NodeInfo n;
        n.name = "Custom Node";
        n.details = "Filters: ";
        n.type = NodeType.custom;
        n.genreType = GenreType.None;
        List<NodeInfo> info = new List<NodeInfo>();

        Vector3 tColor = Vector3.zero;

        foreach (GameObject o in selection.Values) {
            NormalNode nN = o.GetComponent<NormalNode>(); 
            n.details += nN.getInfo().name + ", ";
            info.Add(nN.getInfo());
            Color c = nN.getColor();
            tColor += new Vector3(c.r, c.g, c.b);
            nN.remove();
        }

        selection.Clear();

        tColor /= info.Count;
        GameObject constellation = Instantiate(ConstellationSpawner);
        constellation.transform.position = controllerPose.transform.position;
        ConstellationManager cM = constellation.GetComponent<ConstellationManager>();
        cM.init(n, customNodeFab);
        cM.mainNode.setColor(new Color(tColor.x, tColor.y, tColor.z));

        foreach (NodeInfo i in info) {
            cM.addNode(i);
        }
    }

    private TButtonType getTouchType() {
        if (!this.isTouch.state) {
            return TButtonType.none;
        }

        if (Vector2.Distance(Vector2.zero, padPos.axis) < middleButtonRad) {
            return TButtonType.middle;
        }

        float s = Mathf.Sin(-0.785398f);
        float c = Mathf.Cos(-0.785398f);
        Vector2 rotPoint = new Vector2(padPos.axis.x * c - padPos.axis.y * s, padPos.axis.x * s + padPos.axis.y * c);
        if (rotPoint.x > 0f && rotPoint.y > 0f) {
            return TButtonType.top;
        } else if (rotPoint.x < 0f && rotPoint.y < 0f) {
            return TButtonType.bottom;
        } else if (rotPoint.x > 0f && rotPoint.y < 0f) {
            return TButtonType.right;
        } else {
            return TButtonType.left;
        }
    }

    private void animateSelected() {
        if (selection.Count <= 0) return;
        float eOffSet = 360f / (float) selection.Count;
        float tOffSet = 0f;
        foreach(GameObject o in selection.Values){
            float ang = Mathf.Deg2Rad * ((circleDeg + tOffSet));
            Vector3 pos = new Vector3(Mathf.Sin(ang) * rad, 0.03f, Mathf.Cos(ang) * rad);
            o.transform.localPosition = pos;
            tOffSet += eOffSet;
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
