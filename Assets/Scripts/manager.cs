using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
    public DrawLine drawLine;
    public Transform endPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKey(KeyCode.Space)){
            drawLine.UpdateLine(endPoint.position,true);
        }else{
            drawLine.EndDraw();
        }
    }
}
