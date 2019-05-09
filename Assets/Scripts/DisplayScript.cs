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
    public GameObject movieNodeFab;

    private DataContainer dataContainer;
    public GameObject dataObject;

    
    public float middleButtonRad = 0.4f;

    public GameObject ConstellationSpawner;

    public LayerMask noTouch;
    public LayerMask layerMask;

    private GameObject cSelected;

    private float alpha = 0.0f;
    public float alphaIncrement = 1f;

    public float outerLimit = 0.1f;
    public float innerLimit = 0.01f;
    public float middleLimit = 0.05f;

    private Color originalColor;

    public Color deleteColor = new Color(1f, 0f, 0f);
    public Color pinColor = new Color(0f, 1f, 0f);

    private float rad = 0.05f;

    private float circleDeg = 0.0f;

    private Dictionary<GameObject, GameObject> selection;

    void Start() {
        line = Instantiate(linePrefab);
        lineTransform = line.transform;
        textDisplay = textObject.GetComponent<TextDisplay>();
        selection = new Dictionary<GameObject, GameObject>();
        originalColor = line.GetComponent<Renderer>().material.color;
        dataContainer = new DataContainer();
        dataContainer.init();
    }

    // Update is called once per frame
    void Update() {
        //dataContainer = dataObject.GetComponent<DataContainer>();
        foreach (GameObject o in selection.Keys) {
            o.GetComponent<NormalNode>().tempColor(originalColor, 1f);
        }

        if (cSelected != null) {
            cSelected.GetComponent<NormalNode>().tempColor(pinColor, 1f);
            displayInfo(cSelected.GetComponent<NormalNode>().getInfo());
        }

        /* This is for both style and user feedback */
        if ((getTouchType() == TButtonType.middle || getTouchType() == TButtonType.left || getTouchType() == TButtonType.right) && alpha < 255f) {
            alpha += alphaIncrement;
        } else if (alpha > 0f) {
            alpha -= alphaIncrement;
        }

        // Sets the color of the laser according to the type
        if (getTouchType() == TButtonType.left) {
            line.GetComponent<Renderer>().material.color = deleteColor;
        } else if (getTouchType() == TButtonType.right) {
            line.GetComponent<Renderer>().material.color = pinColor;
        } else {
            line.GetComponent<Renderer>().material.color = originalColor;
        }

        textObject.transform.position = controllerPose.transform.position + Vector3.up * 0.1f;
        textObject.transform.LookAt(headObject.transform);

        /* This handles the laser that shoots out of the controller and how it collides with things */
        if (alpha > 0) {
            RaycastHit hit;

            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 50, layerMask)) {
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
                    if (cSelected == null)
                        hideInfo();
                }
            }
        } else {
            line.SetActive(false);
            if (cSelected == null) {
                hideInfo();
            }
        }

        // Animates the little nodes in and out
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

            // Selection add case
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

            // Deletion case
            if (lastTouched != null && getButtonPress() == TButtonType.left) {
                lastTouched.GetComponent<NormalNode>().remove();
                foreach (GameObject o in selection.Values) {
                    o.GetComponent<NormalNode>().remove();
                }
                selection.Clear();
            }

            // Pin case
            if (getButtonPress() == TButtonType.right) {
                if (lastTouched != null) {
                    if (cSelected == lastTouched) {
                        cSelected = null;
                        hideInfo();
                    } else {
                        cSelected = lastTouched;
                        cSelected.GetComponent<NormalNode>().tempColor(pinColor, 1f);
                    }
                } else {
                    hideInfo();
                    cSelected = null;
                }
            }


            // Clear selection case
            if (getButtonPress() == TButtonType.bottom) {
                foreach (GameObject o in selection.Values) {
                    o.GetComponent<NormalNode>().remove();
                }
                selection.Clear();
            }

            // Spawn case
            if (getButtonPress() == TButtonType.top) {
                spawnConstellation();
            }
        }

        // Animation code
        animateSelected();
        circleDeg = (circleDeg + 1f) % 360f;

    }

    private TButtonType getButtonPress() {
        if (!this.isClick.state) {
            return TButtonType.none;
        }
        return getTouchType();
    }

    /* Spawns a constellation using the nodes currently selected */
    private void spawnConstellation() {

        if (selection.Count <= 0) {
            return;
        }

        Vector3 tColor = Vector3.zero;
        List<NodeInfo> info = new List<NodeInfo>();
        NodeInfo n;
        GameObject theFab = customNodeFab;
        n.name = "";
        n.details = "";
        n.type = NodeType.custom;
        n.genreType = GenreType.None;
        bool noSkip = true;

        // Special case for movies
        if (selection.Count == 1) {
            foreach (GameObject nObject in selection.Values) {
                if (nObject.GetComponent<NormalNode>().getType() == NodeType.movie) {
                    info = dataContainer.fromMovie(nObject.GetComponent<NormalNode>().getInfo().name);
                    n = info[0];
                    info.RemoveAt(0);
                    noSkip = false;
                    theFab = movieNodeFab;
                    Color tempColor = nObject.GetComponent<Renderer>().material.color;
                    tColor = new Vector3(tempColor.r, tempColor.g, tempColor.b);
                    nObject.GetComponent<NormalNode>().remove();
                }
            }
        }

        // If no movies see if all the objects are a genre
        if (noSkip) {
            bool isGenre = true;
            List<GenreType> gens = new List<GenreType>();
            foreach (GameObject nObject in selection.Values) {
                if (nObject.GetComponent<NormalNode>().getType() != NodeType.genre) {
                    isGenre = false;
                } else {
                    gens.Add(nObject.GetComponent<NormalNode>().getInfo().genreType);
                }
            }

            n.name = "Custom Node";
            n.details = "Filters: ";

            // Add in the custom node text
            foreach (GameObject o in selection.Values) {
                NormalNode nN = o.GetComponent<NormalNode>();
                n.details += nN.getInfo().name + ", ";
                info.Add(nN.getInfo());
                Color c = nN.getColor();
                tColor += new Vector3(c.r, c.g, c.b);
                nN.remove();
            }
            tColor /= info.Count;
            // If it's a genre load the relevent movies
            if (isGenre) {
                info = dataContainer.fromGenres(gens);
            }
        }
        selection.Clear();

        GameObject constellation = Instantiate(ConstellationSpawner);
        constellation.transform.position = controllerPose.transform.position;
        ConstellationManager cM = constellation.GetComponent<ConstellationManager>();
        cM.init(n, theFab, 0.50f);
        cM.mainNode.setColor(new Color(tColor.x, tColor.y, tColor.z));

        // Spawn the nodes
        foreach (NodeInfo i in info) {
            NormalNode nN = cM.addNode(i);
            if (nN.getType() == NodeType.movie) {
                nN.setColor(new Color(Random.value, Random.value, Random.value));
            }
        }
        cM.mainNode.setColor(new Color(tColor.x, tColor.y, tColor.z));
    }

    /* Converts a touch into a touch type */
    private TButtonType getTouchType() {
        if (!this.isTouch.state) {
            return TButtonType.none;
        }

        if (Vector2.Distance(Vector2.zero, padPos.axis) < middleButtonRad) {
            return TButtonType.middle;
        }

        // This is to rotate the touch to fall into a quadrent so top is in quadrent 1
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

    /* Animate the selected nodes */
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

    /* Display text info! */
    private void displayInfo(NodeInfo info) {
        textObject.SetActive(true);
        textDisplay.setTitle(info.name);
        textDisplay.setText(textDisplay.formatTextForMain(info.details));
    }

    private void hideInfo() {
        textObject.SetActive(false);
    }

    /* This does the laser that emits from the controller */
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
