using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;
	private PhotonView pv;

	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Unity Buil-in functions

	void Awake()
	{
		if (!instance)
			instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
		pv = GetComponent<PhotonView>();
		
	}
	private void Start()
	{
		SetupScore();
	}
	void Update()
	{

	}

	#endregion




	///==========================================================================================================
	///		SCORE
	///==========================================================================================================

	#region variables

	private int score;
	private ScoreDisplayer displayer;

	#endregion

	#region Score Management functions

	private void SetupScore()
	{
		score = 0;

	}

	public int GetScore()
	{
		return score;
	}
	[PunRPC]
	public void ChangeScore(int change, bool bAlreadyRep=false)
	{
		if(!bAlreadyRep)
		{
			pv.RPC("ChangeScore", RpcTarget.All, change,true);
			return;
		}

		score += change;
		UpdateDisplay();
	}

	#endregion

	#region Displaying functions

	public void DeclareDisplayer(ScoreDisplayer displayer)
	{
		this.displayer = displayer;
		UpdateDisplay();
	}
	private void UpdateDisplay()
	{
		displayer.UpdateDisplay(score);
	}

	#endregion


}
