using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamShakeTester : MonoBehaviour
{
	[Space]
	[Tooltip("CameraShaker component")]
	[SerializeField] CameraShaker shaker = null;
	[Range(0,1)][SerializeField] float magnitude = .75f;

	int functionCall = 0;		// indicator of the shaking function to call

	[Space][SerializeField] ParticleSystem explosionParticles = null;

	[Header ("    Distance Shake")]
	[Tooltip("Info needed for the AddShakeByDistance() function to work.")]
	[SerializeField] DistanceShake distanceShake = new DistanceShake(1,10,0,1);
	[Space]
	[SerializeField] Transform disShakeOrigin = null;
	[SerializeField] Transform minDisPoint = null;
	[SerializeField] Transform maxDisPoint = null;	

	[Header("    UI")]			// references to the UI elements
	[Space]
	[SerializeField] Image traumaImg = null;
	[SerializeField] Image shakeImg = null;
	[Space]
	[SerializeField] Slider magnitudeSld = null;
	[SerializeField] Text magnitudeVal = null;
	[Space]
	[SerializeField] InputField verInpt = null;
	[SerializeField] InputField horInpt = null;
	[SerializeField] InputField rollInpt = null;
	[Space]
	[SerializeField] InputField trmaRedInpt = null;
	[SerializeField] InputField noiseSpdInpt = null;
	[Space]
	[SerializeField] Dropdown ftDrpdwn = null;
	[SerializeField] Text ftTooltip = null;
	string tip_0 = "Adds [magnitude] to the trauma.";
	string tip_1 = "Sets the trauma to [magnitude], only if its not already more than that.";
	string tip_2 = "Shake the camera based on the distance to a point.\n" +
		"Use the mouse wheel to move the red cube.\n\n" +
		"To change the distanceShake values, go to the Cam Shake Tester inspector.";

	void Start()
	{
		// initialize values
		magnitudeSld.value = magnitude;
		verInpt.text = shaker.verMaxShake.ToString();
		horInpt.text = shaker.horMaxShake.ToString();
		rollInpt.text = shaker.rollMaxShake.ToString();
		trmaRedInpt.text = shaker.magnitudeReduction.ToString();
		noiseSpdInpt.text = shaker.noiseSpeed.ToString();
		SetFunction();		
	}

	void Update()
    {
		// shake if the key is pressed
		if (Input.GetKeyDown(KeyCode.Space))
			PlayShake();

		// Move distance shake origin if theres an input
		if (Input.mouseScrollDelta.y != 0)
			MoveOrigin();

		// Hide the UI when Esc is pressed
		if (Input.GetKeyDown(KeyCode.Escape))
			traumaImg.transform.parent.parent.gameObject.SetActive (!traumaImg.transform.parent.parent.gameObject.activeSelf);

		// Update the UI bars for trauma and shake amount
		if (traumaImg.fillAmount != shaker.magnitude)
		{
			traumaImg.fillAmount = shaker.magnitude;
			shakeImg.fillAmount = shaker.magnitude * shaker.magnitude;
		}
	}

	void MoveOrigin()
	{
		if (disShakeOrigin.position.z < maxDisPoint.position.z && Input.mouseScrollDelta.y > 0.1f)
			disShakeOrigin.Translate(Vector3.forward * Input.mouseScrollDelta.y);
		else if (disShakeOrigin.position.z > minDisPoint.position.z && Input.mouseScrollDelta.y < -0.1f)
			disShakeOrigin.Translate(Vector3.forward * Input.mouseScrollDelta.y);
	}

	void PlayShake()
	{
		if (functionCall == 0)
			shaker.AddShake(magnitude);
		else if (functionCall == 1)
			shaker.AddLimitedShake(magnitude);
		else if (functionCall == 2)
			shaker.AddShakeByDistance(disShakeOrigin.position, distanceShake);

		explosionParticles.Play();
	}

	// -----------------------------------------------------
	#region Public functions to be called by the UI

	public void SetMagnitude()
	{
		magnitude = magnitudeSld.value;
		magnitudeVal.text = magnitude.ToString("0.00");
		distanceShake.maxMagnitude = magnitude;
		//shaker.SetShake(magnitude);
	}

	public void SetMaxVer()
	{
		shaker.verMaxShake = float.Parse(verInpt.text);
	}
	public void SetMaxHor()
	{
		shaker.horMaxShake = float.Parse(horInpt.text);
	}
	public void SetMaxRoll()
	{
		shaker.rollMaxShake = float.Parse(rollInpt.text);
	}

	public  void SetTrmaRed()
	{
		shaker.magnitudeReduction = float.Parse(trmaRedInpt.text);
	}

	public void SetNoiseSpeed()
	{
		shaker.noiseSpeed = float.Parse(noiseSpdInpt.text);
	}

	public void SetFunction()
	{
		functionCall = ftDrpdwn.value;
		if (functionCall == 0)
			ftTooltip.text = tip_0;
		else if (functionCall == 1)
			ftTooltip.text = tip_1;
		else if (functionCall == 2)
			ftTooltip.text = tip_2;
		else
			ftTooltip.text = "";
	}

	#endregion
}
