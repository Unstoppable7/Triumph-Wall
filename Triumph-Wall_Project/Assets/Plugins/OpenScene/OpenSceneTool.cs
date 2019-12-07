/*
Versión 1.0
Último cambio: 23/09/2018
Creado por: Marc Baqués Sàbat
Modificado por:
-
Función:
    Herramienta en el top menú para abrir las escenas en editor sin tener que abrirlas desde el explorador de archivos.
*/

using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class OpenSceneTool
{
    const string sceneFolderPath = "Assets/Scenes/";
    const string sceneFormat = ".unity";

	//[MenuItem("Tools/Open Scene/<SceneName>")]
	//static void OpenSceneName()
	//{
	//    OpenScene("<SceneName>");
	//}
	//}
	
	
	//static OdinMenuTree tree;
	//[MenuItem("Tools/OpenScene/Load")]
	//[OnInspectorGUI]
	//static void LoadScenes ( )
	//{
	//	tree = new OdinMenuTree();
	//	tree.AddAllAssetsAtPath( "Tools/OpenScene", "Assets/Scenes/", typeof( Scene ), true );
	//	// etc...
	//	tree.DrawMenuTree();
	//}

	private static void OpenScene(string scene)
    {
        if (SceneManager.GetActiveScene().name != scene)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(sceneFolderPath + scene + sceneFormat);
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Scene already open!", "La escena ya se encuentra en este momento abierta.", "OK");
        }
    }
}

/*
Comentarios:
    Esta herramienta se debe implementar con todas las escenas manualmente. 
    Ejemplo:
        [MenuItem("Open Scene/StandardScene")]
        static void OpenSceneStandardScene()
        {
            OpenScene("StandardScene");
        }
*/
