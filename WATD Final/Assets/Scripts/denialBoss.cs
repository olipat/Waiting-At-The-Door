using UnityEngine;
using System.Collections;

public class denialBoss : MonoBehaviour
{

    public Transform[] targetLocations;
    private int light;
    private bool loopFlag = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(bossCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(loopFlag == true)
        {
            loopFlag = false;
            StartCoroutine(bossCoroutine());
        }
    }

    IEnumerator bossCoroutine()
    {
        light = Random.Range(0, 8);

        switch (light)
        {
            case 8:
                //shine light on pos 8
                print("shine light on point 8");
                break;
            case 7:
                //shine light on pos 7
                print("shine light on point 7");
                break;
            case 6:
                //shine light on pos 6
                print("shine light on point 6");
                break;
            case 5:
                //shine light on pos 5
                print("shine light on point 5");
                break;
            case 4:
                //shine light on pos 4
                print("shine light on point 4");
                break;
            case 3:
                //shine light on pos 3
                print("shine light on point 3");
                break;
            case 2:
                //shine light on pos 2
                print("shine light on point 2");
                break;
            case 1:
                //shine light on pos 1
                print("shine light on point 1");
                break;
            default:
                //shine light on pos 0
                print("shine light on point 0");
                break;
        }

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        loopFlag = true;
    }
}
