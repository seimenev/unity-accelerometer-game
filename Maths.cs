using UnityEngine;
using System.Collections;
using System;

public static class Maths {
	
	static float BetterFloatLerp(float fromValue, float toValue, float speed)
	{
		if(Math.Abs(fromValue - toValue) <= 0.001)
		{
			return toValue;
		}
		else
		{
			return Mathf.Lerp(fromValue, toValue, speed * Time.deltaTime);
		}
	}
}
