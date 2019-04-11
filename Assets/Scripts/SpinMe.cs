using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour {

	[SerializeField] float xRotationsPerMinute = 1f;
	[SerializeField] float yRotationsPerMinute = 1f;
	[SerializeField] float zRotationsPerMinute = 1f;
	
	void Update () {
        //xDegreesPerFrame = Time.DeltaTime, 60, 360, xRotationsPerMinute
        //degrees frame^-1 = seconds frame^1 / seconds minutes ^-1, degrees rotation^1-1 * rotation minute^-1
        //degress frame^-1 = frame^-1 * degrees
		float timeDegrees = Time.deltaTime / 60 * 360;

        float xDegreesPerFrame = timeDegrees * xRotationsPerMinute;
        transform.RotateAround (transform.position, transform.right, xDegreesPerFrame);

		float yDegreesPerFrame = timeDegrees * yRotationsPerMinute;
        transform.RotateAround (transform.position, transform.up, yDegreesPerFrame);

        float zDegreesPerFrame = timeDegrees * zRotationsPerMinute;
        transform.RotateAround (transform.position, transform.forward, zDegreesPerFrame);
	}

}
