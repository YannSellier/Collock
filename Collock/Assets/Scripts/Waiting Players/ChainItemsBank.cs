using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ChainItemsBank", menuName = "Custom/Create Chain Items Bank", order = 1)]
public class ChainItemsBank : ScriptableObject
{
	public List<Sprite> imgs;

	public int[] ConvertSolution(List<Sprite> sprites)
	{
		int[] solution = new int[sprites.Count];

		int i = 0;
		foreach (var sprite in sprites)
		{
			solution[i] = imgs.IndexOf(sprite);
			i++;
		}

		return solution;
	}
}
