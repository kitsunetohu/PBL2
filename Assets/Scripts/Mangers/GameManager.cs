using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager:Manager<GameManager>
{
    public DrawLine playerLine;
    public DrawLine guideLine;
    public Button showLine;
    public Button startTereport;
    public Button reset;
    public Transform[] targetPoints;
    public Transform[] guidePoints;

    public GameObject guide;
    public GameObject passenger;

    public Vector3 targetPoint;
    bool isShowing=false;

    public GameObject guideHitPoint;
    public GameObject passengerHitPoint;

    public Vector3 playerOffest;

    // Start is called before the first frame update
    void Start()
    {
        showLine.onClick.AddListener(ShowLine);
        startTereport.onClick.AddListener(()=>StartCoroutine(StartTereport()));
        reset.onClick.AddListener(Reset);
        targetPoint=targetPoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        playerOffest=passenger.transform.position-guide.transform.position;
        if(isShowing){
            passengerHitPoint.GetComponent<MeshRenderer>().enabled=true;               
            guideLine.UpdateLine(guideHitPoint.transform.position,false);
            passengerHitPoint.transform.position=guideHitPoint.transform.position+playerOffest;
            playerLine.UpdateLine(passengerHitPoint.transform.position,true);
        }
    }

    void ShowLine(){
        isShowing=true;
    }


    IEnumerator StartTereport(){
        int a=Random.Range(0,4);
        Vector3 tmpFrom= guide.transform.position=guidePoints[a].position;
        tmpFrom.y=0;
        yield return new WaitForSeconds(1);
        a=Random.Range(0,5);
        Vector3 tmpTarget=targetPoints[a].position;
        tmpTarget.y=0;
        guideHitPoint.GetComponent<MeshRenderer>().enabled=true;
        guideHitPoint.transform.position=tmpFrom;
        guideHitPoint.transform.DOMove(tmpTarget, 4);
        isShowing=true;
    }

    void Reset(){

    }
}
