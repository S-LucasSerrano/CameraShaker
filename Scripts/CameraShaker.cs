using UnityEngine;
using System.Collections;

[AddComponentMenu ("Camera Shake/Sergio's Cam Shaker")]
public class CameraShaker : MonoBehaviour
{
	protected float trauma = 0;       // current amount of shake. Always between 0 and 1.

	// max amount of rotation when shaking (When trauma is at 1)
	public float verMaxShake = 10f;
	public float horMaxShake = 5f;
	public float rollMaxShake = 3f;

	public float magnitudeReduction = .8f;
	[SerializeReference] protected int reductionMethod = 1;
	public float noiseSpeed = .2f;
	   
	protected bool shaking = false;       // true while the object is still shaking

	// -----------------------------------------------------
	#region AddShake()

	/// <summary>
	/// Increase the amount of camera shake.
	/// </summary>
	/// <param name="magnitude">The amount of shake to be added. Must be between 0 and 1.</param>
	public void AddShake(float magnitude)
	{
		// add trauma and clamp it between 0 and 1
		trauma += magnitude;
		trauma = Mathf.Clamp01(trauma);

		// start shake if it's not already started
		if (!shaking)
			StartCoroutine(ShakingRoutine());
	}

	#endregion


	#region AddClampShake()

	/// <summary>
	/// Increase the cam shake by [magnitude], only if the current shake is less than [max].
	/// </summary>
	/// <param name="magnitude">The amount of shake to be added. Must be between 0 and 1.</param>
	/// <param name="max">Only adds shake if it is already under this value. Must be between 0 and 1.</param>
	public void AddClampShake(float magnitude, float max)
	{
		if (trauma >= max)
			return;
		if (max - trauma < magnitude)
			AddShake(max - magnitude);
		else
			AddShake(magnitude);
	}

	#endregion


	#region AddLimitedShake()

	/// <summary>
	/// Set the camera shake to [magnitude], only if it is not already more than that.
	/// </summary>
	/// <param name="magnitude">The amount of shake to be added. Must be between 0 and 1.</param>
	public void AddLimitedShake(float magnitude)
	{
		if (trauma >= magnitude)
			return;
		AddShake(magnitude - trauma);
	}

	#endregion


	#region SetShake()

	/// <summary>
	/// Sets camera shake to a specific value.
	/// </summary>
	/// <param name="magnitude">The value for the shake to be set to. Must be between 0 and 1.</param>
	public void SetShake(float magnitude)
	{
		magnitude = Mathf.Clamp01(magnitude);
		trauma = 0;
		AddShake(magnitude);
	}

	#endregion


	#region AddShakeByDistance()
	/// <summary>
	/// Shake the camera based on the distance to a point.
	/// </summary>
	/// <param name="origin">Position to calculate the distance to.</param>
	/// <param name="distanceShake">DisatanceShake struct with info that defines how to calculate the shake to add.</param>
	public void AddShakeByDistance(Vector3 origin, DistanceShake distanceShake)
	{
		// error detection
		if (distanceShake.minMaxDis.x > distanceShake.minMaxDis.y)
		{
			Debug.LogError("Min distance has to be less than the max in the DistanceShake values.");
			return;
		}
		else if (distanceShake.minMagnitude > distanceShake.maxMagnitude)
		{
			Debug.LogError("Min shake has to be less than the max in the DistanceShake values.");
			return;
		}

		if (distanceShake.minMaxDis.y == 0)
			return;

		float value = (transform.position - origin).magnitude;								// calculate distance to origin
		value = Mathf.Clamp(value, distanceShake.minMaxDis.x, distanceShake.minMaxDis.y);	// clamp the distance between min and max
		value = CamShakeMath.Remap(value, distanceShake.minMaxDis.x, distanceShake.minMaxDis.y, 0, 1);   // remap that distance to be between 0 and 1
		value = distanceShake.falloffCurve.Evaluate(value);									// evaluate the shake fallof curve at that point
		value = CamShakeMath.Remap(value, 0, 1, distanceShake.minMagnitude, distanceShake.maxMagnitude);			// remap again between min and max shake to have the magnitude to add

		AddLimitedShake(value);
	}

	#endregion


	// -----------------------------------------------------
	#region Shaking Routine

	protected float verNoiseCoor = 0;
	protected float horNoiseCoor = 20;
	protected float rollNoiseCoor = 40;

	/// <summary>
	/// Each frame shakes the camera according to the trauma.
	/// Reducing it until it's 0, then the routine ends.
	/// </summary>
	protected virtual IEnumerator ShakingRoutine()
	{
		Quaternion originalRot = transform.localRotation;

		shaking = true;
		while (trauma > 0)
		{
			//  rotate
			transform.localRotation = originalRot;
			RotationShake(Vector3.right, verMaxShake, verNoiseCoor);
			RotationShake(Vector3.up, horMaxShake, horNoiseCoor);
			RotationShake(Vector3.forward, rollMaxShake, rollNoiseCoor);

			// move through the perlin noise
			verNoiseCoor += noiseSpeed;
			horNoiseCoor += noiseSpeed;
			rollNoiseCoor += noiseSpeed;

			trauma -= magnitudeReduction * Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
		shaking = false;
		transform.localRotation = originalRot;
	}

	/// <summary>
	/// Rotates the object in local space in an axis based on a perlin noise value.
	/// </summary>
	/// <param name="axis">The axis to rotate in.</param>
	/// <param name="maxShake">The max shake than can be added in that axis.</param>
	/// <param name="noiseCoor">The noise map coordinate from where the direction to move within that axis will be extracted.</param>
	protected void RotationShake(Vector3 axis, float maxShake, float noiseCoor)
	{
		float _dir = Mathf.PerlinNoise(noiseCoor, noiseCoor);
		_dir = CamShakeMath.Remap(_dir, 0, 1, -1, 1);

		if (reductionMethod == 0)
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + (axis.normalized * maxShake * (trauma) * _dir));
		else if (reductionMethod == 1)
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + (axis.normalized * maxShake * (trauma * trauma) * _dir));
		else if (reductionMethod == 2)
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + (axis.normalized * maxShake * (trauma * trauma * trauma) * _dir));
	}

	#endregion


	// -----------------------------------------------------
	#region Get_() & Set_()

	/// <summary>
	/// The current amount camera shake magnitude.
	/// <para>When 0, the camera is not shaking. When is 1, is shaking at the max values.</para>
	/// </summary>
	public float magnitude
	{
		get
		{
			return (trauma);
		}
	}

	/// <summary>
	/// Get or Set the max cam shake values as a Vector3.
	/// <para>If you want to keep the current value of one them, set it as a negative number.</para>
	public Vector3 maxShake
	{
		get
		{
			return new Vector3(verMaxShake, horMaxShake, rollMaxShake);
		}
		set
		{
			verMaxShake = value.x;
			horMaxShake = value.y;
			rollMaxShake = value.z;
		}
	}

	#endregion
}
