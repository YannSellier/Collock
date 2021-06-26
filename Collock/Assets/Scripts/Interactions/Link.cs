using UnityEngine;
using Photon.Pun;

public class Link : MonoBehaviour
{
	LineRenderer lr;
	public PhotonView pv;
	public Color startColor;
	public Color endColor;
	public Color invisibleColor;

    // Start is called before the first frame update
    void Awake()
    {
		pv = GetComponent<PhotonView>();

		lr = GetComponent<LineRenderer>();
		lr.startWidth = 0.1f;
		lr.endWidth = 0.1f;

		
		
	}

	private void Start()
	{
		transform.parent = GameObject.Find("CodeLinks").transform;
		transform.position = new Vector3();


		EnableLink(LinkingObj.instance.bIsOpen);
	}

	[PunRPC]
	public void EnableLink(bool bEnable)
	{
		print("Link enabled " + bEnable);
		if (bEnable)
		{
			lr.startColor = startColor;
			lr.endColor = endColor;
		}
		else
		{
			lr.startColor = invisibleColor;
			lr.endColor = invisibleColor;
		}
	}


	[PunRPC]
	public void UpdatePos(Vector2 startPos, Vector2 endPos)
	{
		float z = -14;
		Vector3 startPos3 = new Vector3(startPos.x, startPos.y, z);
		Vector3 endPos3 = new Vector3(endPos.x, endPos.y, z);
		lr.SetPosition(0, startPos3);
		lr.SetPosition(1, endPos3);
	}
}
