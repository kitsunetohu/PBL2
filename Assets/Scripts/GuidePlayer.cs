using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GuidePlayer : MonoBehaviour
{
    public Transform[] pasengerPoses;
    public Transform[] targets;
    int targetNum = 0;

    bool canInput = false;
    private SteamVR_Action_Boolean actionToHaptic = SteamVR_Actions._default.InteractUI;
    private SteamVR_Action_Vibration haptic = SteamVR_Actions._default.Haptic;

    public GameObject ctrller;
    public GameObject Passenger;
    DrawLine guidePlayerLine;
    public DrawLine passengerDrawLine;
    public GameObject guideHitPoint;
    public GameObject passengerHitPoint;
    bool guiding = false;
    Vector3 offset;
    float usedTime=0;
    // Start is called before the first frame update
    void Start()
    {
        canInput = true;
        guidePlayerLine = ctrller.GetComponent<DrawLine>();

        ChoseTarget();
        SetPassengerPos();
        usedTime=0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Start();
        }

        RaycastHit hit;
        var ray = new Ray(ctrller.transform.position, ctrller.transform.forward);
        if (Physics.Raycast(ray, out hit, 30))
        {
            if (hit.collider.tag == "Ground")
            {
                guideHitPoint.transform.position = hit.point;
                guiding = true;
            }
            else
            {
                guiding = false;
                guideHitPoint.transform.position = ctrller.transform.position + ctrller.transform.forward * 0.5f;
            }
        }
        else
        {
            guiding = false;
            guideHitPoint.transform.position = ctrller.transform.position + ctrller.transform.forward * 0.5f;
        }

        guidePlayerLine.UpdateLine(guideHitPoint.transform.position, false);
        if (guiding == true)
        {
            passengerHitPoint.transform.position = guideHitPoint.transform.position - offset;
            passengerDrawLine.UpdateLine(passengerHitPoint.transform.position, true);
        }

        if (actionToHaptic.GetStateDown(SteamVR_Input_Sources.RightHand) && canInput)
        {
            canInput = false;
            Debug.Log("sd");
            haptic.Execute(0, 0.005f, 0.005f, 1, SteamVR_Input_Sources.LeftHand);
            Tereport();
        }

        usedTime+=Time.deltaTime;
    }

    void Tereport()
    {
        var tmp1 = passengerHitPoint.transform.position;
        var tmp2 = targets[targetNum].transform.position;
        tmp1.y = 0;
        tmp2.y = 0;
        Debug.Log("distance：" + Vector3.Distance(tmp1, tmp2)+";"+" Time:"+usedTime);
        Passenger.transform.position = new Vector3(tmp1.x, Passenger.transform.position.y, tmp1.z);

    }

    void ChoseTarget()
    {
        targetNum = Random.Range(0, 5);
        for (int i = 0; i < 5; i++)
        {
            if (i != targetNum)
            {
                targets[i].gameObject.SetActive(false);
            }
            else
            {
                targets[i].gameObject.SetActive(true);
            }
        }
    }

    void SetPassengerPos()
    {
        var num = Random.Range(0, 4);
        Passenger.transform.position = pasengerPoses[num].position;
        offset = transform.position - Passenger.transform.position;
        offset.y = 0;
    }

}
