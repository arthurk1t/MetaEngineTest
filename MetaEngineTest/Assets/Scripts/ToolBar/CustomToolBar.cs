using UnityEditor;
using UnityEngine;
using WebServer;

namespace ToolBar
{
    public class CustomToolBar : EditorWindow
    {
        private const string Url = "http://localhost:3000";

        private string _receivedData = "";
        private CustomWebServer _customWebServer;
        private GUIContent _dataLabel;
        
        [MenuItem("Window/MetaEngineToolbar")]
        private static void OpenToolWindow()
        {
            var toolWindow = GetWindow<CustomToolBar>();
            toolWindow.Show();
        }

        private void OnEnable()
        {
            _customWebServer = new CustomWebServer();
            _customWebServer.DataReceived += UpdateData;
            _customWebServer.StartServer();
        }

        private void OnDisable()
        {
            if (!_customWebServer.IsServerRunning)
                return;
            
            _customWebServer.StopServer();
            _customWebServer.DataReceived -= UpdateData;
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            
            if (GUILayout.Button("Start Test", EditorStyles.toolbarButton))
            {
                OnButtonPressed();
            }
            
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);

            _dataLabel = new GUIContent("Message: " + _receivedData);
            GUILayout.Label(_dataLabel);
        }

        private void OnButtonPressed()
        {
            OpenWebPage(Url);
        }

        private void OpenWebPage(string url)
        {
            Application.OpenURL(url);
        }

        private void UpdateData(string data)
        {
            _receivedData = data;

           ActivateWindow();
           Debug.LogError($"Updated data is {_receivedData}");
           Repaint();
        }

        private void ActivateWindow()
        { 
            EditorWindowUtil.ActivateMainWindow();
        }
    }
}

