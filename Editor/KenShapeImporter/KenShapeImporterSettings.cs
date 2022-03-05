//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEditor;
//using UnityEngine.UIElements;
//using UnityEditor.UIElements;

//namespace BracedFramework
//{
//    public class KenShapeImporterSettings : ScriptableObject
//    {
//        public const string k_MyCustomSettingsPath = "Assets/Editor/MyCustomSettings.asset";

//        [SerializeField]
//        private int m_Number;

//        [SerializeField]
//        private string m_SomeString;

//        internal static KenShapeImporterSettings GetOrCreateSettings()
//        {
//            var settings = AssetDatabase.LoadAssetAtPath<KenShapeImporterSettings>(k_MyCustomSettingsPath);
//            if (settings == null)
//            {
//                settings = ScriptableObject.CreateInstance<KenShapeImporterSettings>();
//                settings.m_Number = 42;
//                settings.m_SomeString = "The answer to the universe";
//                AssetDatabase.CreateAsset(settings, k_MyCustomSettingsPath);
//                AssetDatabase.SaveAssets();
//            }
//            return settings;
//        }

//        internal static SerializedObject GetSerializedSettings()
//        {
//            return new SerializedObject(GetOrCreateSettings());
//        }
//    }


//    // Create MyCustomSettingsProvider by deriving from SettingsProvider:
//    public class KenShapeSettingsProvider : SettingsProvider
//    {
//        private SerializedObject m_CustomSettings;

//        class Styles
//        {
//            public static GUIContent number = new GUIContent("My Number");
//            public static GUIContent someString = new GUIContent("Some string");
//        }

//        const string k_MyCustomSettingsPath = "Assets/Editor/MyCustomSettings.asset";
//        public KenShapeSettingsProvider(string path, SettingsScope scope = SettingsScope.User)
//            : base(path, scope) { }

//        public static bool IsSettingsAvailable()
//        {
//            return File.Exists(k_MyCustomSettingsPath);
//        }

//        public override void OnActivate(string searchContext, VisualElement rootElement)
//        {
//            // This function is called when the user clicks on the MyCustom element in the Settings window.
//            m_CustomSettings = KenShapeImporterSettings.GetSerializedSettings();
//        }

//        public override void OnGUI(string searchContext)
//        {
//            // Use IMGUI to display UI:
//            EditorGUILayout.PropertyField(m_CustomSettings.FindProperty("m_Number"), Styles.number);
//            EditorGUILayout.PropertyField(m_CustomSettings.FindProperty("m_SomeString"), Styles.someString);
//            m_CustomSettings.ApplyModifiedPropertiesWithoutUndo();
//        }

//        // Register the SettingsProvider
//        [SettingsProvider]
//        public static SettingsProvider CreateMyCustomSettingsProvider()
//        {
//            if (IsSettingsAvailable())
//            {
//                var provider = new KenShapeSettingsProvider("Project/MyCustomSettingsProvider", SettingsScope.Project);

//                // Automatically extract all keywords from the Styles.
//                provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
//                return provider;
//            }

//            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
//            return null;
//        }
//    }
//}
