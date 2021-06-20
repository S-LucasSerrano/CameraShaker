using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CameraShaker))]
[CanEditMultipleObjects]
public class CameraShaker_Editor : Editor
{
	private SerializedProperty verMaxShake;
	private SerializedProperty horMaxShake;
	private SerializedProperty rollMaxShake;
	private SerializedProperty traumaRed;
	private SerializedProperty noiseSpeed;
	private SerializedProperty reductionMethod;

	static private bool showMax = true;
	   
	private void OnEnable()
	{
		verMaxShake = serializedObject.FindProperty("verMaxShake");
		horMaxShake = serializedObject.FindProperty("horMaxShake");
		rollMaxShake = serializedObject.FindProperty("rollMaxShake");
		traumaRed = serializedObject.FindProperty("magnitudeReduction");
		noiseSpeed = serializedObject.FindProperty("noiseSpeed");
		reductionMethod = serializedObject.FindProperty("reductionMethod");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		showMax = EditorGUILayout.BeginFoldoutHeaderGroup(showMax, new GUIContent("Max Shake Values", "Amount of camera displacement when the shake magnitude is at 1"));
		if (showMax)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(verMaxShake, new GUIContent("Ver Max Shake", "Max shake in the X axis."));
			EditorGUILayout.PropertyField(horMaxShake, new GUIContent("Hor Max Shake", "Max shake in the Y axis."));
			EditorGUILayout.PropertyField(rollMaxShake, new GUIContent("Roll Max Shake", "Max shake in the Z axis."));
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(traumaRed, new GUIContent("Magnitude Reduction", "Reduction of shake per frame. Multiplied by delta time."));
		GUIContent[] options = { new GUIContent("Linear"), new GUIContent("Square"), new GUIContent("Cube") };
		reductionMethod.intValue = EditorGUILayout.Popup(new GUIContent("Reduction Method"), reductionMethod.intValue, options);

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(noiseSpeed, new GUIContent("Noise Speed", "Speed at which the perlin noise moves per frame."));

		//EditorGUILayout.Space();
		/* EditorGUILayout.HelpBox("This component manipulates the local rotation of the object.\n" +
				"If you want to rotate it, you should make it a children of other GameObject and rotate than one instead.", MessageType.Info); */

		serializedObject.ApplyModifiedProperties();
	}
}



[CustomEditor(typeof(CameraShaker2D))]
[CanEditMultipleObjects]
public class CameraShaker2D_Editor : CameraShaker_Editor
{
	// I want it to do the same thing as the regular CameraShaker_Editor
}
