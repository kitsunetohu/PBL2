using UnityEngine;
using System.Collections.Generic;
/*
使用方法：
1.把DrawLine这个prefab放到射线发出点。（比如放到Vive的手柄下）
2.画线时使用 UpdateLine(Vector3 finishPoint) 每一帧传入参数为终点的坐标。
3.结束画线时调用  EndDraw() 。

 */


[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
   
    public Transform[] controlPoints= new Transform[3];
    public LineRenderer lineRenderer;
 
    private int layerOrder = 0;
    private int _segmentNum = 50;
 
 
    void Start()
    {
        if (lineRenderer==null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
       LineInvisible();
    }

    public void LineInvisible(){
        if(lineRenderer!=null){
            lineRenderer.enabled=false;
        }
    }

    public void reStart(){
        Start();
    }
 
    void Update()
    {

    }
 
    public void UpdateLine(Vector3 finishPoint,bool isCurve){
        if(lineRenderer.enabled==false){
           lineRenderer.enabled=true;
        }
        controlPoints[0].position=transform.position;
        float length=(finishPoint-controlPoints[0].position).magnitude;
        controlPoints[1].position=controlPoints[0].position+transform.forward*length/3;
        controlPoints[2].position=finishPoint;
        
        if(isCurve){
            DrawCurve();
        }
        else{
            DrawStreat(finishPoint);
            Debug.Log(finishPoint);
        }
        

    }

    public void EndDraw(){
         lineRenderer.enabled=false;
    }
    
    void DrawStreat(Vector3 finishPoint){
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,finishPoint);

    }


    void DrawCurve()
    {
            for (int i = 1; i <= _segmentNum; i++)
            {
                float t = i / (float)_segmentNum;
                int nodeIndex = 0;
                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position,
                    controlPoints[nodeIndex+1].position, controlPoints[nodeIndex+2].position);
                lineRenderer.positionCount = i;
                lineRenderer.SetPosition(i - 1, pixel);
            }
 
    }
 
    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
 
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
 
        return p;
    }
     
}