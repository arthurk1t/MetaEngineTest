using UnityEditor;

namespace ToolBar
{
    public static class EditorWindowUtil
    {
        public static void ActivateMainWindow()
        {
            EditorApplication.delayCall += () =>
            {
                EditorWindow.GetWindow<SceneView>().Focus();
            };
        }
    }
}