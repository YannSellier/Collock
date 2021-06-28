using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainSolutionDisplayer : SolutionDisplayer
{

	public List<Image> slotImgs;
	public ChainItemsBank chainItemBank;

	public override void DisplaySolution(int[] solution)
	{
		for(int i = 0; i < solution.Length; i++)
		{
			Display(i, solution[i]);
		}
	}


	public void Display(int indexSlot, int indexImg)
	{
		slotImgs[indexSlot].sprite = chainItemBank.imgs[indexImg];
	}

}
