using UnityEngine;

/// <summary>
/// Struct with all the info needed for the AddShakeByDistance() function to work.
/// </summary>
[System.Serializable]
public struct DistanceShake
{
	[Tooltip ("Min and Max distance to have in count.")]
	public Vector2 minMaxDis;

	[Tooltip ("Lower shake magnitude that can be added.")]
	[Range(0, 1)] public float minMagnitude;
	[Tooltip ("Greater shake magnitude that can be added.")]
	[Range(0, 1)] public float maxMagnitude;

	[Tooltip ("Curve that defines how the shake is reduce by the distance.\n" +
		"The X axis is distance. 0 = minDis and 1 = maxDis.\n" +
		"The Y axis is magnitude. 0 = minShake and 1 = maxShake.")]
	public AnimationCurve falloffCurve;

	public DistanceShake (float setMinDis, float setMaxDis, float setMinMagnitude, float setMaxMagnitude)
	{
		minMaxDis.x = setMinDis;
		minMaxDis.y = setMaxDis;
		minMagnitude = setMinMagnitude;
		maxMagnitude = setMaxMagnitude;
		falloffCurve = new AnimationCurve();
		falloffCurve.AddKey(0, 1);		// make the falloff linear by default
		falloffCurve.AddKey(1, 0);
	}
}
