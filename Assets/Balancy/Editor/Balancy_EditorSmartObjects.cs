using Balancy.Data;
using Balancy.Dictionaries;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Balancy.Editor
{
    [ExecuteInEditMode]
    public class Balancy_EditorSmartObjects : EditorWindow
    {
        private class ContentInfo
        {
            public string Header;
            public string Description;
            public bool Selected = true;
        }
        
        private static ContentInfo[] FIELDS_TO_RESET =
        {
            new ContentInfo
            {
                Header = "GeneralInfo",
                Description = "Contains information about your first login and playtime"
            },
            new ContentInfo
            {
                Header = "ScriptsState",
                Description = "All active scripts information"
            }, 
            new ContentInfo
            {
                Header = "SmartInfo",
                Description = "All active GameEvent and GameOffers"
            },
            new ContentInfo
            {
                Header = "Payments",
                Description = "The history of payments"
            },
            new ContentInfo
            {
                Header = "SegmentsInfo",
                Description = "Segmentation of the user"
            },
            new ContentInfo
            {
                Header = "TestsInfo",
                Description = "AB Testing info"
            }
        };
        
        [MenuItem("Tools/Balancy/SmartObjects", false, -104000)]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(Balancy_EditorSmartObjects));
            window.titleContent.text = "Balancy SmartObjects";
            window.titleContent.image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Balancy/Editor/BalancyLogo.png");
        }

        private void Awake()
        {
            minSize = new Vector2(300, 500);
        }
        
        private void OnEnable()
        {
            EditorApplication.update += update;
        }
        
        private void OnDisable()
        {
            EditorApplication.update -= update;
        }
        
        private void update()
        {
            // if (_downloading)
            Repaint();
        }

        private Balancy_Plugins plugins;
        
        private Balancy_Plugins Plugins => plugins ?? (plugins = new Balancy_Plugins(this));

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("You can clean up your profile here");
            bool anythingToReset = false;
            foreach (var field in FIELDS_TO_RESET)
            {
                if (!IsFileExists(field))
                    GUI.enabled = false;

                field.Selected = GUILayout.Toggle(field.Selected, field.Header);
                anythingToReset = anythingToReset || (field.Selected && GUI.enabled);
                GUI.enabled = true;
            }

            GUI.enabled = anythingToReset;
            if (GUILayout.Button("Reset"))
            {
                ResetSelectedFields();
            }
            GUI.enabled = true;
            
            GUILayout.EndVertical();
        }

        private static bool IsFileExists(ContentInfo contentInfo)
        {
            var path = Application.persistentDataPath + "/UnnyProfile/";
            return File.Exists(path + contentInfo.Header);
        }

        private void ResetSelectedFields()
        {
            var path = Application.persistentDataPath + "/UnnyProfile/";
            foreach (var field in FIELDS_TO_RESET)
            {
                if (field.Selected)
                    File.Delete(path + field.Header);
            }

            var systemPath = path + "UnnySystem";
            if (File.Exists(systemPath))
            {
                var fileContent = FileHelper.LoadFromFilePath(systemPath);
                var systemData = JsonConvert.DeserializeObject<UnnySystemData>(fileContent);
                systemData.ForceUse = true;
                FileHelper.SaveToFilePath(systemPath, JsonConvert.SerializeObject(systemData));
            }
        }
    }
}