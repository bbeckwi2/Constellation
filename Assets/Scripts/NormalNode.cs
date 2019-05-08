using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalNode : MonoBehaviour
{
    private NormalNode parent;
    private List<NormalNode> children;

    private bool isInit = false;

    public float size = 0.25f;

    public GameObject connectionFab;
    private GameObject connection;

    private NodeInfo info;

    private int dying = -1;

    
    private float scale = 0.0f;
    private float cScaleGoal = 0.0f;
    private float cScale = 0.0f;

    private Color color;
    private Color tColor;
    private float tmpCTimeLeft = -2;
    private float tmpMTimeLeft = -2;

    public void init() {
        children = new List<NormalNode>();
        isInit = true;
        this.color = this.gameObject.GetComponent<Renderer>().material.color;
        this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void init(NodeInfo info) {
        this.init();
        this.info = info;
    }

    public void addParent(NormalNode parent) {
        this.parent = parent;

        // Connection fabrication
        connection = Instantiate(connectionFab);

        // Positions
        Vector3 gPos = this.parent.gameObject.transform.position;
        Vector3 sPos = this.gameObject.transform.position;

        // Get the transform and distance
        Transform t = connection.transform;
        float dist = Vector3.Distance(gPos, sPos);

        // Get and set the link between the stars
        t.position = Vector3.Lerp(sPos, gPos, 0.5f);
        cScaleGoal = dist;
        t.localScale = new Vector3(t.localScale.x, t.localScale.y, 0f);
        t.LookAt(gPos);
    }

    public Color getColor() {
        return this.color;
    }

    public void setInfo(NodeInfo info) {
        this.info = info;
    }

    public NodeType getType() {
        return this.info.type;
    }

    public NodeInfo getInfo() {
        return this.info;
    }

    public void remove() {
        foreach (NormalNode c in children) {
            c.remove();
        }
        this.dying = 290;
    }

    public void addChild(NormalNode child) {
        children.Add(child);
    }

    public void setColor(Color color) {
        this.gameObject.GetComponent<Renderer>().material.color = color;
        this.color = color;
    }

    public void tempColor(Color c, float time) {
        this.tColor = c;
        this.tmpCTimeLeft = time;
        this.tmpMTimeLeft = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit) {
            return;
        }

        if (this.tmpCTimeLeft > 0f) {
            float per = tmpCTimeLeft / tmpMTimeLeft;
            Vector3 gV = new Vector3(color.r, color.g, color.b);
            Vector3 sV = new Vector3(tColor.r, tColor.g, tColor.b);
            Vector3 cV = Vector3.Lerp(gV, sV, per);
            this.gameObject.GetComponent<Renderer>().material.color = new Color(cV.x, cV.y, cV.z);
            this.tmpCTimeLeft = Mathf.Max(tmpCTimeLeft - 0.05f, 0f);
        } else if (this.tmpCTimeLeft > -1f) {
            tmpCTimeLeft = -2f;
            this.gameObject.GetComponent<Renderer>().material.color = this.color;
        }
        
        /* Death Conditions */
        if (this.dying > 0) {
            // If we are in the process of dying shrink stuff and decrement dying
            this.dying--;
            if (this.scale > 0) {
                this.scale -= 0.001f;
                this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            } else {
                this.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            }

            // Special case for the head node, it has no parent so we have to exit
            if (this.parent == null) {
                return;
            }

            if (this.cScale > 0) {
                cScale -= 0.01f;
                this.connection.transform.localScale = new Vector3(this.connection.transform.localScale.x, this.connection.transform.localScale.y, cScale);
            } else {
                this.connection.transform.localScale = new Vector3(0f, 0f, 0f);
            }

        } else if (this.dying == 0) {
            // If we are dead, we destroy everything and exit
            Destroy(this.connection);
            Destroy(this.gameObject);
            Destroy(this);
        } else {
            // This is used to grow the nodes and their connections 
            if (this.scale < size) {
                this.scale += 0.01f;
                this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }

            if (this.cScale < cScaleGoal) {
                cScale += 0.05f;
                this.connection.transform.localScale = new Vector3(this.connection.transform.localScale.x, this.connection.transform.localScale.y, cScale);
            }
        }
    }
}
