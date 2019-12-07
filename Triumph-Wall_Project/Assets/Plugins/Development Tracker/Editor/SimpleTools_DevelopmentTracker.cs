using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEditor.Build.Reporting;

namespace SimpleTools {
	[CustomEditor (typeof(DevelopmentTracker))]
	public class SimpleTools_DevelopmentTracker : Editor {


		[MenuItem( "Tools/BuildTracker/Development Tracker %k", false, 1001 )]
		public static void createAsset(){
			GameObject[] objects = new GameObject[1];
			Object obj = AssetDatabase.LoadMainAssetAtPath ("Assets/Plugins/Development Tracker/Editor/DevelopmentAsset.asset");
			Selection.activeObject = obj;

		}

		int selectedHistory = -1;

#if UNITY_EDITOR
		static DevelopmentTracker devTracker;
		static bool showOptions = false;
		static int bundleVersionCounter = 0;
		static bool bundleCodeWentUp = false;

		bool unFold = false;
		public override void OnInspectorGUI(){
			
			devTracker = (DevelopmentTracker)target;
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Development Tracker");
			EditorGUILayout.Space ();


			EditorGUILayout.LabelField ("Version");
			devTracker.version = EditorGUILayout.TextField (devTracker.version);

			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical();
			GUILayout.Label("BuildName", GUILayout.ExpandWidth(false));
			GUILayout.Label(devTracker.version + "_" + devTracker.build.ToString(), GUILayout.ExpandWidth(false));
			GUILayout.EndVertical();
			GUILayout.Space(50);

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();
			GUILayout.Label("BundleVersion: ", GUILayout.ExpandWidth(false));
			GUILayout.Label(PlayerSettings.Android.bundleVersionCode.ToString(), GUILayout.ExpandWidth(false));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Version UP"))
			{
				PlayerSettings.Android.bundleVersionCode++;
				bundleVersionCounter++;
				bundleCodeWentUp = true;
			}
			GUILayout.Label("SOLO si la Build es de Deployment");
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Build Number");

			EditorGUILayout.BeginHorizontal();
			devTracker.build = EditorGUILayout.IntField (devTracker.build);
			if (GUILayout.Button ("Iterate")) {
				devTracker.build++;

				BuildPlayerWindow.ShowBuildPlayerWindow();
				EditorUtility.SetDirty(devTracker);
			}
			EditorGUILayout.EndHorizontal ();

			if(devTracker.build > 0)
			{
				PlayerSettings.bundleVersion = devTracker.version +"_"+ devTracker.build.ToString(); //se actualiza el player
			}
			else
			{
				PlayerSettings.bundleVersion = devTracker.version;
			}
	
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Description");
			devTracker.description = EditorGUILayout.TextArea (devTracker.description);

			//---CUSTOM BUILD OPTIONS---//
			EditorGUILayout.Space ();
			GUILayout.BeginHorizontal();
			GUI.color = Color.green;
			if (GUILayout.Button ("Finalize Build"))
			{
				BuildPlayerWindow.ShowBuildPlayerWindow();
				
				showOptions = true;
			}
			GUI.color = Color.white;

			GUI.color = Color.cyan;
			if (GUILayout.Button("Open Player Settings", GUILayout.ExpandWidth(false)))
			{
				//var windowType = typeof(Editor).Assembly.GetType("UnityEditor.EditorUserSettings");
				//EditorWindow.GetWindow(windowType);
				EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
				//Selection.activeObject = Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings");
				//Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings");
			}
			GUILayout.EndHorizontal();
			GUI.color = Color.white;
			if (showOptions)
			{
				
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Do your Build Now");
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				
				if (GUILayout.Button("Cancel Build Entry", GUILayout.ExpandWidth(false)))
				{
					showOptions = false;
					if (PlayerSettings.Android.bundleVersionCode > 1 && bundleCodeWentUp)
					{
						PlayerSettings.Android.bundleVersionCode -= bundleVersionCounter;
						bundleVersionCounter = 0;
						bundleCodeWentUp = false;
					}
				}
				GUI.color = Color.yellow;
				if (GUILayout.Button("Force Build Entry", GUILayout.ExpandWidth(false)))
				{
					devTracker.history.Add(System.DateTime.Now.ToString() +
					"\nVersion: " + devTracker.version +
					"\nIteration " + devTracker.build.ToString() + "\nDescription:\n" +
					devTracker.description + "\n" +
					EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget) 
					+ "\n" + "Entrada al diario forzada");
					EditorUtility.SetDirty(devTracker);
					showOptions = false;
				}
				GUI.color = Color.white;
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}

			EditorGUILayout.Space ();
			unFold = EditorGUILayout.Foldout(unFold, "Entries", true);
			if (unFold)
			{
				EditorGUI.indentLevel++;
				for (int i = devTracker.history.Count -1; i >= 0; i--) {
					GUILayout.BeginHorizontal();
					if (GUILayout.Button ("View Build " + i.ToString())) {
						if (selectedHistory != i) {
							selectedHistory = i;
						} else {
							selectedHistory = -1;
						}
					}
					GUI.color = Color.yellow;
					if (GUILayout.Button("Delete This entry", GUILayout.ExpandWidth(false)))
					{
						//delete from devtracker
						devTracker.history.RemoveAt(i);
						break;
					}
					GUI.color = Color.white;
					GUILayout.EndHorizontal();


					if (selectedHistory == i) {
						//Content
						string[] info = devTracker.history [i].Split ("\n" [0]);
						foreach (string ii in info) {
							EditorGUILayout.LabelField (ii);
						}
					}
				}
			}
			EditorGUI.indentLevel--;
			GUILayout.Space (10);

			GUILayout.Label("Ultima Build");
			GUILayout.BeginHorizontal();

			GUILayout.Space(20);
			GUILayout.Label(EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget), GUILayout.ExpandWidth(false) );
			if (GUILayout.Button("Open Build Folder", GUILayout.ExpandWidth(false)))
			{
				EditorUtility.RevealInFinder(EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget));
			}

			//if (GUILayout.Button("Build Report", GUILayout.ExpandWidth(false)))
			//{
			//	EditorApplication.ExecuteMenuItem("BuildTracker/Show Build Report");
			//}

			GUILayout.EndHorizontal();

			//GUILayout.Space(10);

			//GUILayout.Label("Carpetas");
			//GUILayout.BeginHorizontal();
			//GUILayout.Space(20);
			//if (GUILayout.Button("ReportFolder", GUILayout.ExpandWidth(false)))
			//{
			//	//comprobar si este archivo existe sino crearlo
			//	if (!System.IO.File.Exists("../UnityBuildReports/NO_TOUCH.txt"))
			//	{
			//		using (System.IO.File.Create("../UnityBuildReports/NO_TOUCH.txt"))
			//		{ }
			//	}
			//	EditorUtility.RevealInFinder(System.IO.Path.Combine(Application.dataPath + "/../../UnityBuildReports/NO_TOUCH.txt"));
			//}
			//if (GUILayout.Button("SaveFolder", GUILayout.ExpandWidth(false)))
			//{
			//	//comprobar si este archivo existe sino crearlo
			//	if (!System.IO.File.Exists("../SaveData/NO_TOUCH.txt"))
			//	{
			//		using (System.IO.File.Create("../SaveData/NO_TOUCH.txt"))
			//		{ }
			//	}
			//	EditorUtility.RevealInFinder(System.IO.Path.Combine(Application.dataPath + "/../../SaveData/NO_TOUCH.txt"));
			//}
			//GUILayout.EndHorizontal();

			GUI.color = Color.red;
			GUILayout.Space (20);
			if (GUILayout.Button ("Delete All Builds")) {
				devTracker.history.Clear ();
			}
			EditorUtility.SetDirty(devTracker);
		}

		[UnityEditor.Callbacks.PostProcessBuild(1000)]
		public static void AfterBuildIsDone(BuildTarget target, string pathToBuiltProject)
		{
			if (Application.isEditor)
			{
				if (showOptions)
				{
					devTracker.history.Add(System.DateTime.Now.ToString() +
					"\nVersion: " + devTracker.version +
					"\nIteration " + devTracker.build.ToString() + "\nDescription:\n" +
					devTracker.description + "\n" +
					EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget));
					bundleVersionCounter = 0;
					bundleCodeWentUp = false;
					showOptions = false;

					EditorUtility.SetDirty(devTracker);
				}
			}
		}

#endif
	}
}
