using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceNode : MonoBehaviour
{
    int childCount = 0;
    public ForceNode parent;
    private float wiggle = 0.05f;
    // Start is called before the first frame update
    void Start(){
    }

    public void setPos(Vector3 pos) {
        this.transform.position = pos;
    }

    public void setParent(ForceNode n) {
        this.parent = n;
    }

    // Update is called once per frame
    void Update(){
        if (this.parent == null) {
            return;
        }
        if (parent.childCount < childCount + 1) {
            parent.childCount = childCount + 1;
        }
        Vector3 heading = this.transform.position - this.parent.transform.position;
        Vector3 direction = heading / heading.magnitude;
        float distance = Vector3.Distance(this.parent.transform.position, this.transform.position);
        float reqDist = Mathf.Pow(2f, ((float)childCount));
        if (distance < reqDist - wiggle  || distance > reqDist + wiggle) {
            print("Moving...");
            this.transform.position = this.transform.position + (direction * 0.1f * ((distance > reqDist)? -1f: 1f));
        }
    }

    void OnDrawGizmos() {
        if (parent != null) {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, parent.transform.position);
        }
    }
}
