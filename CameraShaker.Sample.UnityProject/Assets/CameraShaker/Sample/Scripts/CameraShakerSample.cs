using UnityEngine;
using UnityEngine.UI;

namespace LucasSerrano.CameraShake.Sample
{
	public class CameraShakerSample : MonoBehaviour
	{
		CameraShaker camShaker = null;

		[Space]
		[SerializeField] Image traumaBar = null;
		[SerializeField] Image shakeBar = null;

		[Space]
		[SerializeField] Slider magnitudeSlider = null;
		[SerializeField] Text magnitudeValueText = null;

		[Space]
		[SerializeField] Toggle holdToggle = null;
		[SerializeField] Button shakeButton = null;

		[Space]
		[SerializeField] ParticleSystem explosionParticles = null;


		// -------------------------------------

		private void Start()
		{
			// Find a reference to the CameraShaker.
			camShaker = FindObjectOfType<CameraShaker>();

			// Set the default values of the UI elements.
			traumaBar.fillAmount = 0;
			shakeBar.fillAmount = 0;
			magnitudeSlider.value = .7f;
			holdToggle.isOn = false;

			// Add a listener to button to shake the camera.
			shakeButton.onClick.AddListener(Shake);
		}

		private void Update()
		{
			// Update the UI to show the current amount of Trauma and shake amount.
			float trauma = camShaker.Trauma;
			if (traumaBar != null) traumaBar.fillAmount = trauma;
			if (shakeBar != null) shakeBar.fillAmount = camShaker.ScalingCurve.Evaluate(trauma);

			if (magnitudeSlider != null)
			{
				// Hold the shake if indicated in the UI.
				float sliderValue = magnitudeSlider.value;
				if (holdToggle != null && holdToggle.isOn)
					camShaker.SetShake(magnitudeSlider.value);

				// Show the current slider value in a text.
				if (magnitudeValueText != null)
					magnitudeValueText.text = sliderValue.ToString("0.#").Replace(',', '.');
			}

			// Shake when the Space bar is pressed.
			if (Input.GetKeyDown(KeyCode.Space))
				Shake();
		}


		// -------------------------------------

		/// Function called by the button to add shake to de camera based on the slider value.
		public void Shake()
		{
			camShaker.AddShake(magnitudeSlider.value);
			if(explosionParticles != null) explosionParticles.Play();
		}
	}
}
