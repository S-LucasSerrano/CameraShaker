using UnityEngine;
using System.Collections;

[AddComponentMenu ("Camera Shake/Sergio's Cam Shaker 2D")]
public class CameraShaker2D : CameraShaker
{
	public CameraShaker2D ()
	{
		maxShake = new Vector3(1, 1, 3);
	}


	// ------------------------------------------------------
	#region Shaking Routine

	protected override IEnumerator ShakingRoutine()
	{
		Quaternion originalRot = transform.localRotation;
		Vector3 originalPos = transform.localPosition;

		shaking = true;
		while (trauma > 0)
		{
			//  move and rotate
			transform.localPosition = originalPos;
			transform.localRotation = originalRot;
			MovementShake(Vector3.right, verMaxShake, verNoiseCoor);
			MovementShake(Vector3.up, horMaxShake, horNoiseCoor);
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
	/// Moves the object in local space in an axis, based on a perlin noise value.
	/// </summary>
	/// <param name="axis">The axis to rotate in.</param>
	/// <param name="maxShake">The max shake than can be added in that axis.</param>
	/// <param name="noiseCoor">The noise map coordinate from where the direction to move within that axis will be extracted.</param>
	void MovementShake(Vector3 axis, float maxShake, float noiseCoor)
	{
		float _dir = Mathf.PerlinNoise(noiseCoor, noiseCoor);
		_dir = CamShakeMath.Remap(_dir, 0, 1, -1, 1);

		if (reductionMethod == 0)
			transform.localPosition += axis.normalized * maxShake * (trauma) * _dir;
		else if (reductionMethod == 1)
			transform.localPosition += axis.normalized * maxShake * (trauma * trauma) * _dir;
		else if (reductionMethod == 2)
			transform.localPosition += axis.normalized * maxShake * (trauma * trauma * trauma) * _dir;
	}

	#endregion
}
