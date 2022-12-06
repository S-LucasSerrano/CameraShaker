using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LucasSerrano.CameraShake.Sample
{
	public class ShakeByDistanceSample : MonoBehaviour
	{
		CameraShaker camShaker = null;

		[Space]
		[SerializeField] Transform[] cubes = { };
		[SerializeField] ShakeByDistanceData shakeByDistance = new ShakeByDistanceData(5, 35);

		[Space]
		[SerializeField] ParticleSystem explosionParticles = null;


		// -------------------------------------

		private void Start()
		{
			// Find a reference to the CameraShaker.
			camShaker = FindObjectOfType<CameraShaker>();
			// Make ure it has the PhysicsRaycaster componet to be able to click on 3D GameObjects.
			Camera.main.gameObject.AddComponent<PhysicsRaycaster>();

			// Add a new component for each cube that will trigger an event when clicked.
			foreach (Transform cube in cubes)
			{
				ClickableCube clickComponent = cube.gameObject.AddComponent<ClickableCube>();
				clickComponent.onClick.AddListener(CubeClicked);
			}
		}

		/// Function call by the cubes when clicking on them.
		public void CubeClicked(Transform cube)
		{
			camShaker.AddShakeByDistance(cube.position, shakeByDistance);

			explosionParticles.transform.position = cube.position;
			explosionParticles.Play();
		}


		// -------------------------------------

		public class ClickableCube : MonoBehaviour, IPointerDownHandler
		{
			public UnityEvent<Transform> onClick = new UnityEvent<Transform>();

			public void OnPointerDown(PointerEventData eventData)
			{
				onClick.Invoke(gameObject.transform);
			}
		}
	}
}
