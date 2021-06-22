using UnityEngine;
using Photon.Pun;

public class Link : MonoBehaviour
{
	LineRenderer lr;
	public PhotonView pv;

    // Start is called before the first frame update
    void Awake()
    {
		pv = GetComponent<PhotonView>();

		lr = GetComponent<LineRenderer>();
		lr.startWidth = 0.1f;
		lr.endWidth = 0.1f;
	}

	[PunRPC]
	public void UpdatePos(Vector2 startPos, Vector2 endPos)
	{
		lr.SetPosition(0, startPos);
		lr.SetPosition(1, endPos);
	}
}
