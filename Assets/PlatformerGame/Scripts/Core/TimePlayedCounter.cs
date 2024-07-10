using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlayedCounter : MonoBehaviour, ITimePlayedCounter
{
    private double Timer;

    delegate void State();
    State CurrentState = () => { };

    private void Awake()
    {
        CurrentState = DoNothing;
    }

    private void Update()
    {
        CurrentState();
    }

    public void StartCount()
    {
        CurrentState = CountTime;
    }

    public void StopCount()
    {
        CurrentState = DoNothing;
    }

    public int GetTimeAndReset()
    {
        int data = (int)Timer;
        Timer = 0;
        return data;
    }

    private void DoNothing()
    {
        //
    }

    private void CountTime()
    {
        Timer += Time.deltaTime;
    }
}
