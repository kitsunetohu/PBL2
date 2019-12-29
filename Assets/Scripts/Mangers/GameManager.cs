﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : Manager<GameManager>
{
    public DrawLine playerLine;
    public DrawLine guideLine;
    public Button showLine;
    public Button startTereport;
    public Button reset;
    public Transform[] targetPoints;
    public Transform[] guidePoints;
    public Collider pillar;

    public GameObject guide;
    public GameObject passenger;

    public Vector3 targetPoint;
    bool isShowing = false;

    public GameObject guideHitPoint;
    public GameObject passengerHitPoint;

    public Vector3 playerOffest;

    //VR的UI
    public Text vrText;


    float usedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        vrText.text = " ";
        isShowing = false;
        showLine.onClick.AddListener(ShowLine);
        startTereport.onClick.AddListener(() => StartCoroutine(StartTereport()));
        reset.onClick.AddListener(Reset);
        targetPoint = targetPoints[0].position;
        guide.SetActive(false);
        LineInvisible();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerOffest = passenger.transform.position - guide.transform.position;
        if (isShowing)
        {
            usedTime += Time.deltaTime;
            passengerHitPoint.GetComponent<MeshRenderer>().enabled = true;
            guideLine.UpdateLine(guideHitPoint.transform.position, false);
            passengerHitPoint.transform.position = guideHitPoint.transform.position + playerOffest;
            playerLine.UpdateLine(passengerHitPoint.transform.position, true);
            
        }
    }

    void ShowLine()
    {

    }


    IEnumerator StartTereport()
    {

        guide.SetActive(true);
        int a = Random.Range(0, 4);
        Vector3 tmpFrom = guide.transform.position = guidePoints[a].position;
        tmpFrom.y = 0;
        yield return new WaitForSeconds(1);

        isShowing = true;
        guideHitPoint.SetActive(true);
        passengerHitPoint.SetActive(true);
        a = Random.Range(0, 5);
        Vector3 tmpTarget = targetPoints[a].position-playerOffest;
        guide.transform.LookAt(tmpTarget);
        tmpTarget.y = 0;
        guideHitPoint.GetComponent<MeshRenderer>().enabled = true;
        guideHitPoint.transform.position = tmpFrom;
        guideHitPoint.transform.DOMove(tmpTarget, 4).OnComplete(() =>
        {
           passenger.transform.position=targetPoints[a].position;
           isShowing=false;
           LineInvisible();
           
        });
        isShowing = true;
    }

    void LineInvisible(){
        guideHitPoint.SetActive(false);
        passengerHitPoint.SetActive(false);
        playerLine.LineInvisible();
        guideLine.LineInvisible();
    }
    void Reset()
    {
        Start();
        playerLine.reStart();
        guideLine.reStart();
        passenger.transform.position = GameObject.Find("StartPoint").transform.position;
    }

    public void TouchedPillar()
    {
        vrText.text = "clear,used " + usedTime + "s";
    }
}
