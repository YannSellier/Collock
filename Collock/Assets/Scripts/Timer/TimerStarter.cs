using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		TimerManager.instance.StartTimer();
		Destroy(gameObject);
    }

}
