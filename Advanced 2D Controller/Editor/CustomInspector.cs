using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(TwoDController))]
public class CustomInspector : Editor {       

    bool showPosition;

    
    TwoDController controller;

    SerializedProperty horizontalAxis;
    SerializedProperty ladderAxis;

    SerializedProperty jumpButton;
    SerializedProperty sprintButton;
    SerializedProperty crouchButton;
    SerializedProperty jetpackButton;
    SerializedProperty dashButton;

    SerializedProperty moveType;
    SerializedProperty jumpType;
    SerializedProperty jetpackType;
    SerializedProperty ladderType;
    SerializedProperty dashType;


    SerializedProperty speed;
    SerializedProperty crouchSpeed;
    SerializedProperty sprintSpeed;

    SerializedProperty gravity;

    SerializedProperty jumpForce;
    SerializedProperty doubleJumpForce;

    SerializedProperty ladderSpeed;

    SerializedProperty jetpackDuration;
    SerializedProperty jetpackRecharge;
    SerializedProperty jetpackForce;

    SerializedProperty dashDuration;
    SerializedProperty dashForce;
    SerializedProperty dashCooldown;

    void OnEnable ()
    {
        horizontalAxis = serializedObject.FindProperty("axis");
        ladderAxis = serializedObject.FindProperty("ladderAxis");
        
        jumpButton = serializedObject.FindProperty("jumpKey");
        sprintButton = serializedObject.FindProperty("sprintKey");
        crouchButton = serializedObject.FindProperty("crouchKey");
        jetpackButton = serializedObject.FindProperty("jetpackKey");
        dashButton = serializedObject.FindProperty("dashKey");

        moveType = serializedObject.FindProperty("playerMoveType");
        jumpType = serializedObject.FindProperty("playerJumpType");
        jetpackType = serializedObject.FindProperty("jetpackType");
        ladderType = serializedObject.FindProperty("ladderType");
        dashType = serializedObject.FindProperty("dashType");

        speed = serializedObject.FindProperty("speed");         
        crouchSpeed = serializedObject.FindProperty("crouchSpeed");
        sprintSpeed = serializedObject.FindProperty("sprintSpeed");

        dashForce = serializedObject.FindProperty("dashForce");
        dashDuration = serializedObject.FindProperty("dashDuration");
        dashCooldown = serializedObject.FindProperty("dashCooldown");

        gravity = serializedObject.FindProperty("gravity");

        jumpForce = serializedObject.FindProperty("jumpForce");
        doubleJumpForce = serializedObject.FindProperty("doubleJumpForce");

        ladderSpeed = serializedObject.FindProperty("ladderSpeed");

        jetpackDuration = serializedObject.FindProperty("jetpackDuration");
        jetpackRecharge = serializedObject.FindProperty("jetpackRecharge");
        jetpackForce = serializedObject.FindProperty("jetForce");
    }

	public override void OnInspectorGUI ()
	{
        serializedObject.Update();        

        
        EditorGUILayout.LabelField("Settings: ", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(moveType, new GUIContent("Movement "));
        EditorGUILayout.PropertyField(jumpType, new GUIContent("Jumping "));
        EditorGUILayout.PropertyField(jetpackType, new GUIContent("Jetpack "));
        EditorGUILayout.PropertyField(ladderType, new GUIContent("Ladders "));
        EditorGUILayout.PropertyField(dashType, new GUIContent("Dashing "));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Controls ", EditorStyles.boldLabel);

        EditorGUILayout.Space(); 
		EditorGUILayout.Space();


        EditorGUILayout.PropertyField(horizontalAxis, new GUIContent ("Horizontal Axis "));

        EditorGUILayout.Space();

        if (ladderType.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(ladderAxis, new GUIContent("Ladder Axis "));
            EditorGUILayout.Space();
        }



        if (jumpType.enumValueIndex == 0 || jumpType.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(jumpButton, new GUIContent("Jump Button "));
            EditorGUILayout.Space();
        }



        if (moveType.enumValueIndex == 0 || moveType.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(sprintButton, new GUIContent("Sprint Button "));
            EditorGUILayout.Space();
        }



        if (moveType.enumValueIndex == 1 || moveType.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(crouchButton, new GUIContent("Crouch Button "));
            EditorGUILayout.Space();
        }



        if (jetpackType.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(jetpackButton, new GUIContent("Jetpack Button "));
            EditorGUILayout.Space();
        }

        if (dashType.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(dashButton, new GUIContent("Dash Button "));
            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Modifiers", EditorStyles.boldLabel);

        EditorGUILayout.Space();       

        EditorGUILayout.PropertyField(speed, new GUIContent("Speed"));

        if (moveType.enumValueIndex == 0 || moveType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(sprintSpeed, new GUIContent("Sprint Speed "));

        if (moveType.enumValueIndex == 1 || moveType.enumValueIndex == 2)
            EditorGUILayout.PropertyField(crouchSpeed, new GUIContent("Crouch Speed "));

        if (ladderType.enumValueIndex == 0)
            EditorGUILayout.PropertyField (ladderSpeed, new GUIContent("Ladder Speed "));

        EditorGUILayout.Space ();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Gravity", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(gravity, new GUIContent("Gravity "));

        if (jumpType.enumValueIndex == 0 || jumpType.enumValueIndex == 1)
            EditorGUILayout.PropertyField(jumpForce, new GUIContent("Jump Force "));

        if (jumpType.enumValueIndex == 1)
            EditorGUILayout.PropertyField(doubleJumpForce, new GUIContent("Double Jump Force "));

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (jetpackType.enumValueIndex == 0)
        {
            EditorGUILayout.LabelField("Jetpack", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(jetpackDuration, new GUIContent("Jetpack Duration "));

            EditorGUILayout.PropertyField(jetpackForce, new GUIContent("Jetpack Force "));

            EditorGUILayout.PropertyField(jetpackRecharge, new GUIContent("Jetpack Recharge Duration "));

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        if (dashType.enumValueIndex ==  0)
        {
            EditorGUILayout.LabelField("Dash", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(dashDuration, new GUIContent("Dash Duration "));
            EditorGUILayout.PropertyField(dashForce, new GUIContent("Dash Force "));
            EditorGUILayout.PropertyField(dashCooldown, new GUIContent("Dash Cooldown "));

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();

    }
 
}
