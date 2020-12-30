using UnityEngine;
using System.Collections;

public class JTInput : MonoBehaviour {

	static Vector3 restPosition = new Vector3(0,0,-1);
	static Quaternion calibrationRotation;
	
	bool doOnce;

	void Update () {

		if (!doOnce)
		{
			CalibrateAccelerometer();
			doOnce = true;
		}
	}
		
	public static void CalibrateAccelerometer() 
    {
		Vector3 wantedDeadZone = Input.acceleration;
		calibrationRotation = Quaternion.FromToRotation(wantedDeadZone, restPosition);
		
		for(int i=0; i<16; i++)
		{
            if (PlayerPrefs.HasKey("AccelCalibration" + i.ToString()))
				PlayerPrefs.DeleteKey("AccelCalibrtaion" + i.ToString());
		}
	}
	
	public static Vector3 GetAccelerometer(Vector3 original)
    {
		Vector3 accel = new Vector3(0.0f, 0.0f, 0.0f);
		float totalAccel = Mathf.Sqrt((original.x * original.x) + (original.y * original.y) + (original.z * original.z));	// total amount of accelration  
		
		if ((totalAccel != 0))	// make sure we can divide correctly
		{
			accel.x = Mathf.Asin(original.y / totalAccel);	// from accelration to tilt, X
			accel.y = -Mathf.Asin(original.x / totalAccel); // from accelration to tilt, Y
			accel.z = Mathf.Asin(original.z / totalAccel);	// from accelration to tilt, Z
		}
		
		accel = calibrationRotation * accel;
		
	    return accel;	
	}
}
