//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ContractConsumer))]
[CanEditMultipleObjects]
public class ContractConsumerEditor : Editor
{
    public SerializedProperty ContractJson, privateKey, onStart, onUpdate, onClick, onCollision, index, onStartFunction, onUpdateFunction, onClickFunction;
    ENetworks indexSelector;
    string functionToCall;
    string functionParams;
    Contract contract;


    void OnEnable()
    {
        //index = serializedObject.FindProperty("index");
        ContractJson = serializedObject.FindProperty("ContractJson");
        privateKey = serializedObject.FindProperty("privateKey");
        onStart = serializedObject.FindProperty("onStart");
        onUpdate = serializedObject.FindProperty("onUpdate");
        onClick = serializedObject.FindProperty("onClick");
        onCollision = serializedObject.FindProperty("onCollision");
        //lookAtPoint = serializedObject.FindProperty("lookAtPoint");
        Debug.Log(ContractJson.objectReferenceValue);

        //contract = (Contract)ContractJson.objectReferenceValue.ToObject<Contract>();
        //decodedABI = root["abi"].ToObject<List<DecodedABI>>();

    }

    public override void OnInspectorGUI()
    {
        /* GUILayout.BeginVertical();
        indexSelector = (ENetworks)EditorGUILayout.EnumPopup("Deploy to:", indexSelector);
        GUILayout.EndVertical(); */

        serializedObject.Update();
        //EditorGUILayout.PropertyField(index);
        EditorGUILayout.PropertyField(ContractJson);
        EditorGUILayout.PropertyField(privateKey);
        EditorGUILayout.PropertyField(onStart);
        if (onStart.boolValue)
        {
            functionToCall = EditorGUILayout.TextField("On Start Function", functionToCall);
            functionParams = EditorGUILayout.TextField("Params", functionParams);
        }
        EditorGUILayout.PropertyField(onUpdate);
        if (onUpdate.boolValue)
        {
            functionToCall = EditorGUILayout.TextField("On Update Function", functionToCall);
            functionParams = EditorGUILayout.TextField("Params", functionParams);
        }
        EditorGUILayout.PropertyField(onClick);
        if (onClick.boolValue)
        {
            functionToCall = EditorGUILayout.TextField("On Click Function", functionToCall);
            functionParams = EditorGUILayout.TextField("Params", functionParams);
        }
        EditorGUILayout.PropertyField(onCollision);
        if (onCollision.boolValue)
        {
            functionToCall = EditorGUILayout.TextField("On Click Function", functionToCall);
            functionParams = EditorGUILayout.TextField("Params", functionParams);
        }
        serializedObject.ApplyModifiedProperties();
    }
}