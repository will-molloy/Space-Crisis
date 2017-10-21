using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
/*This Script will autoremove all helper scripts */
namespace GridEditor2D
{
    public class EditorPostProcessor
    {
        [PostProcessScene]
        public static void OnPostprocessScene()
        {
            GameObject[] allgo = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            for (int i = 0; i < allgo.Length; i++)
            {
                if (allgo[i].GetComponent<Grid>())
                    Object.DestroyImmediate(allgo[i].GetComponent<Grid>());

                if (allgo[i].GetComponent<LevelEditorLayer>())
                    Object.DestroyImmediate(allgo[i].GetComponent<LevelEditorLayer>());

                if (allgo[i].GetComponent<TileManager>())
                    Object.DestroyImmediate(allgo[i].GetComponent<TileManager>());
            }

        }
    }
}