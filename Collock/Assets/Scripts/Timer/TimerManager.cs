using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
	public static TimerManager instance;

	public PhotonView pv;

	///==========================================================================================================
	///		UNITY BUILT-IN
	///==========================================================================================================

	#region Unity's functions

	public void Awake()
	{

		if (!instance) instance = this;
		else Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		pv = GetComponent<PhotonView>();
	}

	public void Update()
	{
		UpdateTimer(Time.deltaTime);
	}

	#endregion




	///==========================================================================================================
	///		TIMER
	///==========================================================================================================

	#region Timer variables

	public TimerDisplayer displayer;

	public float initialTime;
	public float currentTime;
	private float currentDeltaChange = 0;

	private bool bShouldTime = false;

	#endregion

	#region Timer functions
	

	public void StartTimer()
	{
		ResetTimer();
		bShouldTime = true;
	}
	public void ResetTimer()
	{
		currentTime = initialTime;
	}
	public void StopTimer()
	{
		bShouldTime = false;
	}


	[PunRPC]
	public void ChangeTimer(float change)
	{
		currentTime += change;
		UpdateTimerDisplay();
	}
	public void UpdateTimer(float deltaTime)
	{
		if (!bShouldTime) return;


		currentDeltaChange += deltaTime;
		if(currentDeltaChange >= 1)
		{
			if (currentTime <= 1) pv.RPC("TimerEnd", RpcTarget.AllBuffered);

			pv.RPC("ChangeTimer", RpcTarget.AllBuffered, -1.0f);
			currentDeltaChange -= 1;
		}
	}

	[PunRPC]
	public void TimerEnd()
	{
		StopTimer();
		print("The timer has ended");
	}


	#endregion

	#region TimerDisplay functions


	public void DeclareDisplayer(TimerDisplayer displayer)
	{
		this.displayer = displayer;
	}

	public void UpdateTimerDisplay()
	{
		if (!displayer) return;


		
		displayer.timerText.text = ConvertTime(currentTime);
	}
	public string ConvertTime(float time)
	{
		int min = (int)(time / 60);
		int sec = (int)time - min * 60;
		string ms = min < 10 ? "0" + min : "" + min;
		string ss = sec < 10 ? "0" + sec : "" + sec;
		return ms + ":" + ss;
	}


	#endregion
}
