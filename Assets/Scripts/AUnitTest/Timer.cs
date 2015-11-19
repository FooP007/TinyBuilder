using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    private float _time;
    private int _sec;
    private bool startTime = false;
    // Use this for initialization
    void Awake ()
    {
        Reset();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (startTime)
        {
            _time += 1 * Time.deltaTime;
        }
	}

    public void StartTimer()
    {
        startTime = true;
        Debug.Log("CAll");
    }

    public void Stop()
    {
        startTime = false;
    }

    public void Reset()
    {
        _time = 0;
        startTime = false;
    }

    public float time
    {
        get { return _time; }
        set { _time = value; }
    }
}
