//C# Example (LookAtPointEditor.cs)
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.EditorCoroutines.Editor;

[CustomEditor(typeof(ContractConsumer))]
[CanEditMultipleObjects]
public class ContractConsumerEditor : Editor
{
    public SerializedProperty ContractJson, privateKey, onStart, onUpdate, onClick, onCollision, index, onStartFunction, onUpdateFunction, onClickFunction, onCollisionFunction, abi, onClickParameters;
    ENetworks indexSelector;
    string functionParams;
    List<DecodedABI> decodedABI = new List<DecodedABI>();


    int onStartSelected, onUpdateSelected, onClickSelected, onCollisionSelected;
    string[] options;// = new List<string>();

    void OnEnable()
    {
        ContractJson = serializedObject.FindProperty("ContractJson");
        privateKey = serializedObject.FindProperty("privateKey");
        onStart = serializedObject.FindProperty("onStart");
        onUpdate = serializedObject.FindProperty("onUpdate");
        onClick = serializedObject.FindProperty("onClick");
        onCollision = serializedObject.FindProperty("onCollision");
        abi = serializedObject.FindProperty("abi");
        onStartFunction = serializedObject.FindProperty("onStartFunction");
        onClickFunction = serializedObject.FindProperty("onClickFunction");
        onUpdateFunction = serializedObject.FindProperty("onUpdateFunction");
        onCollisionFunction = serializedObject.FindProperty("onCollisionFunction");
        onClickParameters = serializedObject.FindProperty("onClickParameters");

        List<DecodedABI> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DecodedABI>>(abi.stringValue);
        options = new string[data.Count];
        for (int i = 0; i < data.Count; i++)
        {
            options[i] = data[i].name;
            if (data[i].name == onStartFunction.stringValue)
            {
                onStartSelected = i;
            }
            if (data[i].name == onClickFunction.stringValue)
            {
                onClickSelected = i;
            }
            if (data[i].name == onUpdateFunction.stringValue)
            {
                onUpdateSelected = i;
            }
            if (data[i].name == onCollisionFunction.stringValue)
            {
                onCollisionSelected = i;
            }
        }

        /*
            GUILayout.BeginVertical(EditorStyles.helpBox);
            index = (ENetworks)EditorGUILayout.EnumPopup("Deploy to:", index);
            GUILayout.EndVertical();
        */

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(ContractJson);
        EditorGUILayout.PropertyField(privateKey);

        onStart.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(onStart.boolValue, "Call Function On Start");
        if (onStart.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            onStartSelected = EditorGUILayout.Popup("Function", onStartSelected, options);
            /* if (EditorGUI.EndChangeCheck())
            { */
            onStartFunction.stringValue = options[onStartSelected];
            /*  serializedObject.ApplyModifiedProperties();
         } */
            functionParams = EditorGUILayout.TextField("Params", functionParams);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        onUpdate.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(onUpdate.boolValue, "Call Function On Update");
        if (onUpdate.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            onUpdateSelected = EditorGUILayout.Popup("Function", onUpdateSelected, options);
            if (EditorGUI.EndChangeCheck())
            {
                onUpdateFunction.stringValue = options[onUpdateSelected];
            }
            functionParams = EditorGUILayout.TextField("Params", functionParams);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        onClick.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(onClick.boolValue, "Call Function On Click");
        if (onClick.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            onClickSelected = EditorGUILayout.Popup("Function", onClickSelected, options);
            if (EditorGUI.EndChangeCheck())
            {
                onClickFunction.stringValue = options[onClickSelected];
            }
            onClickParameters.stringValue = EditorGUILayout.TextField("Params", onClickParameters.stringValue);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();



        onCollision.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(onCollision.boolValue, "Call Function On Collision");
        if (onCollision.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            onCollisionSelected = EditorGUILayout.Popup("Function", onCollisionSelected, options);
            if (EditorGUI.EndChangeCheck())
            {
                onCollisionFunction.stringValue = options[onCollisionSelected];
            }
            functionParams = EditorGUILayout.TextField("Params", functionParams);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        serializedObject.ApplyModifiedProperties();
    }
}