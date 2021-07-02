using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SolutionDisplayer : MonoBehaviour
{
	public GameObject voteBtns;

	public void ShowVoteBtns(bool bShow)
	{
		voteBtns.SetActive(bShow);
	}
	public void Init()
	{
		ShowVoteBtns(true);
	}

	public abstract void DisplaySolution(int[] solution);

	public abstract void ShowError(bool[] errors);



}
