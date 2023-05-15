using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlzNoThrowOffEdge : MonoBehaviour
{
    Vector3 startPoint;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.transform.position.y < -5)
            //this.transform.position = startPoint;
    }
}
