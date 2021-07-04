using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
	public float speed = 20;
    // Update is called once per frame
    void Update()
    {
		transform.Rotate(20 * Time.deltaTime * Vector3.up);
    }
}
