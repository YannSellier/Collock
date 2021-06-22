using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticLib
{

	public static Vector2 GetMouseWorldPos2D()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new Vector2(mousePos.x, mousePos.y);
	}

	public static Collider2D GetAimedCollider2D()
	{
		return GetAimedCollider2D(GetMouseWorldPos2D());
	}
	public static Collider2D GetAimedCollider2D(Vector2 mousePos)
	{
		RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
		if (hit.collider != null)
		{
			return hit.collider;
		}
		return null;
	}

	public static (Collider2D,Interactable) SearchForInteractable()
	{
		Collider2D coll = GetAimedCollider2D();
		if (!coll || coll.GetComponentInParent<Interactable>() == null) return (null,null);

		return (coll, coll.GetComponentInParent<Interactable>());
	}

}
