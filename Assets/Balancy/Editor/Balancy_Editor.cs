using System;
using UnityEngine;
using UnityEditor;

namespace Balancy.Editor
{
    [ExecuteInEditMode]
    public class Balancy_Editor : EditorWindow
    {
        public delegate void SynchAddressablesDelegate(string gameId, string token, Constants.Environment environment, Action<string, float> onProgress, Action<string> onComplete);
        public static event SynchAddressablesDelegate SynchAddressablesEvent;

        [MenuItem("Tools/Balancy/Config", false, -104002)]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(Balancy_Editor));
            window.titleContent.text = "Balancy Config";
            window.titleContent.image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Balancy/Editor/BalancyLogo.png");
        }

        private void Awake()
        {
            minSize = new Vector2(500, 500);
        }

        readonly string[] SERVER_TYPE = {"Development", "Stage", "Production"};
        private Balancy_EditorAuth _authHelper;
        
        private int _selectedServer;
        private bool _downloading;
        private float _downloadingProgress;
        private string _downloadingFileName;
        
        private Balancy_EditorAuth AuthHelper => _authHelper ?? (_authHelper = new Balancy_EditorAuth(this));

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
        
        private void OnGUI()
        {
            GUI.enabled = !_downloading;
            
            RenderSettings();
            EditorGUILayout.Space();
            RenderLoader();
        }
        
        private void RenderSettings()
        {
            AuthHelper.Render();
        }

        private void RenderLoader()
        {
            GUI.enabled = !_downloading && AuthHelper.HasSelectedGame();
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Label("Data Editor");
            _selectedServer = GUILayout.SelectionGrid(_selectedServer, SERVER_TYPE, SERVER_TYPE.Length, EditorStyles.radioButton);

            if (_downloading)
            {
                GUI.enabled = true;
                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                EditorGUI.ProgressBar(rect, _downloadingProgress, _downloadingFileName);
                GUI.enabled = false;
            }
            else
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Generate Code"))
                    StartCodeGeneration();

                if (GUILayout.Button("Download Data"))
                    StartDownloading();
                
                if (GUILayout.Button("Synch Addressables"))
                    StartSynchingAddressables();
                
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUI.enabled = true;
        }

        private void StartCodeGeneration()
        {
            _downloading = true;
            _downloadingProgress = 0.5f;
            _downloadingFileName = "Generating the code...";
            var gameInfo = _authHelper.GetSelectedGameInfo();
            var token = _authHelper.GetAccessToken();
            Balancy_CodeGeneration.StartGeneration(
                gameInfo.GameId,
                token,
                (Constants.Environment) _selectedServer,
                () => { _downloading = false; },
                PluginUtils.CODE_GENERATION_PATH
            );
        }

        private void StartSynchingAddressables()
        {
            if (SynchAddressablesEvent == null)
            {
                EditorUtility.DisplayDialog("Warning", "Addressables Plugin is not installed. Please install it below and don't forget to import Unity's Addressables from Package Manager", "Got it");
            }
            else
            {
                _downloading = true;
                _downloadingProgress = 0f;
                _downloadingFileName = "Synchronizing addressables...";
                var gameInfo = _authHelper.GetSelectedGameInfo();
                var token = _authHelper.GetAccessToken();
                SynchAddressablesEvent(
                    gameInfo.GameId,
                    token,
                    (Constants.Environment) _selectedServer,
                    (fileName, progress) =>
                    {
                        _downloadingFileName = fileName;
                        _downloadingProgress = progress;
                    },
                    (error) =>
                    {
                        _downloading = false;
                        if (!string.IsNullOrEmpty(error))
                            EditorUtility.DisplayDialog("Error", error, "Ok");
                        else
                            EditorUtility.DisplayDialog("Success", "Addressables are now synched. Please reload Balancy web page", "Ok");
                    }
                );
            }
        }

        private void StartDownloading()
        {
            _downloading = true;
            _downloadingProgress = 0;

            var gameInfo = _authHelper.GetSelectedGameInfo();
            var appConfig = new AppConfig
            {
                ApiGameId = gameInfo.GameId,
                PublicKey = gameInfo.PublicKey,
                Environment = (Constants.Environment) _selectedServer
            };
            
            DicsHelper.LoadDocs(appConfig, responseData =>
            {
                _downloading = false;
                if (!responseData.Success)
                    EditorUtility.DisplayDialog("Error", responseData.Error.Message, "Ok");
            }, (fileName, progress) =>
            {
                _downloadingFileName = fileName;
                _downloadingProgress = progress;
            });
        }
    }
}