using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		if(TimerManager.instance)
			TimerManager.instance.DeclareDisplayer(this);
    }


	public TextMeshProUGUI timerText;

}
