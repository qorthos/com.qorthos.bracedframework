using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using BracedFramework;

namespace BracedFramework
{
    [CustomEditor(typeof(KenShapeAssetImporter))]
    [CanEditMultipleObjects]
    public class KenShapeAsserImporterEditor : ScriptedImporterEditor
    {
        // Stored SerializedProperty to draw in OnInspectorGUI.
        SerializedProperty m_RotateAxis;
        SerializedProperty m_HDRIntensities;

        SerializedProperty m_UseBackfaceColor;
        SerializedProperty m_BackfaceColor;


        public override void OnEnable()
        {
            base.OnEnable();
            // Once in OnEnable, retrieve the serializedObject property and store it.
            m_RotateAxis = serializedObject.FindProperty("RotateAxis");
            m_HDRIntensities = serializedObject.FindProperty("HDRIntensities");

            m_UseBackfaceColor = serializedObject.FindProperty("UseBackfaceColor");
            m_BackfaceColor = serializedObject.FindProperty("BackfaceColor");
        }

        public override void OnInspectorGUI()
        {
            // Update the serializedObject in case it has been changed outside the Inspector.
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_RotateAxis);
            EditorGUILayout.PropertyField(m_HDRIntensities);

            EditorGUILayout.PropertyField(m_UseBackfaceColor);
            EditorGUILayout.PropertyField(m_BackfaceColor);

            // Apply the changes so Undo/Redo is working
            serializedObject.ApplyModifiedProperties();

            // Call ApplyRevertGUI to show Apply and Revert buttons.
            ApplyRevertGUI();
        }
    }
}