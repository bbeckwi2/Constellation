using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalNode : MonoBehaviour
{
    private NormalNode parent;
    private List<NormalNode> children;

    private bool isInit = false;

    public GameObject connectionFab;
    private GameObject connection;

    private NodeType type;
    private NodeInfo info;

    private int dying = -1;

    private float scale = 0.0f;
    private float cScaleGoal = 0.0f;
    private float cScale = 0.0f;

    public void init() {
        children = new List<NormalNode>();
        isInit = true;
        print(this.gameObject.transform.localScale);
        this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void init(NodeType type, NodeInfo info) {
        this.init();
        this.type = type;
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

    public NodeType getType() {
        return this.type;
    }

    public NodeInfo getInfo() {
        return this.info;
    }

    public void remove() {
        print("Set to remove!");
        foreach (NormalNode c in children) {
            c.remove();
        }
        this.dying = 290;
    }

    public void addChild(NormalNode child) {
        children.Add(child);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit) {
            return;
        }

        print(this.dying);
        if (this.dying > 0) {
            this.dying--;
            if (this.scale > 0) {
                this.scale -= 0.001f;
                this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            } else {
                this.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            }

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
            Destroy(this.connection);
            Destroy(this.gameObject);
            Destroy(this);
        } else {
            if (this.scale < 0.3) {
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
