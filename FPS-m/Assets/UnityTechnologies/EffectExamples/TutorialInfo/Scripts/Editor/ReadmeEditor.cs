using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;

[CustomEditor(typeof(Readme))]
[InitializeOnLoad]
public class ReadmeEditor : Editor {
	
	static string kShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";
	
<<<<<<< HEAD
	static float kSpace = 15f;
=======
	static float kSpace = 16f;
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
	
	static ReadmeEditor()
	{
		EditorApplication.delayCall += SelectReadmeAutomatically;
	}
	
	static void SelectReadmeAutomatically()
	{
		if (!SessionState.GetBool(kShowedReadmeSessionStateName, false ))
		{
			var readme = SelectReadme();
			SessionState.SetBool(kShowedReadmeSessionStateName, true);
			
			if (readme && !readme.loadedLayout)
			{
				LoadLayout();
				readme.loadedLayout = true;
			}
		} 
	}
	
	static void LoadLayout()
	{
		var assembly = typeof(EditorApplication).Assembly; 
		var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
		var method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
<<<<<<< HEAD
		method.Invoke(null, new object[]{Path.Combine(Application.dataPath, "»Readme/Layout.wlt"), false});
	}
	
	[MenuItem("Help/Project Readme")]
=======
		method.Invoke(null, new object[]{Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false});
	}
	
	[MenuItem("Tutorial/Show Tutorial Instructions")]
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
	static Readme SelectReadme() 
	{
		var ids = AssetDatabase.FindAssets("Readme t:Readme");
		if (ids.Length == 1)
		{
			var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
			
			Selection.objects = new UnityEngine.Object[]{readmeObject};
			
			return (Readme)readmeObject;
		}
		else
		{
			Debug.Log("Couldn't find a readme");
			return null;
		}
	}
	
	protected override void OnHeaderGUI()
	{
		var readme = (Readme)target;
		Init();
		
<<<<<<< HEAD
		var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth/3f - 20f, readme.iconMaxWidth);
=======
		var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth/3f - 20f, 128f);
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
		
		GUILayout.BeginHorizontal("In BigTitle");
		{
			GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
			GUILayout.Label(readme.title, TitleStyle);
		}
		GUILayout.EndHorizontal();
	}
	
	public override void OnInspectorGUI()
	{
		var readme = (Readme)target;
		Init();
		
		foreach (var section in readme.sections)
		{
			if (!string.IsNullOrEmpty(section.heading))
			{
				GUILayout.Label(section.heading, HeadingStyle);
			}
			if (!string.IsNullOrEmpty(section.text))
			{
				GUILayout.Label(section.text, BodyStyle);
			}
			if (!string.IsNullOrEmpty(section.linkText))
			{
<<<<<<< HEAD
				GUILayout.Space(kSpace / 2);
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
				if (LinkLabel(new GUIContent(section.linkText)))
				{
					Application.OpenURL(section.url);
				}
			}
			GUILayout.Space(kSpace);
		}
	}
	
	
	bool m_Initialized;
	
	GUIStyle LinkStyle { get { return m_LinkStyle; } }
	[SerializeField] GUIStyle m_LinkStyle;
	
	GUIStyle TitleStyle { get { return m_TitleStyle; } }
	[SerializeField] GUIStyle m_TitleStyle;
	
	GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
	[SerializeField] GUIStyle m_HeadingStyle;
	
	GUIStyle BodyStyle { get { return m_BodyStyle; } }
	[SerializeField] GUIStyle m_BodyStyle;
	
	void Init()
	{
		if (m_Initialized)
			return;
		m_BodyStyle = new GUIStyle(EditorStyles.label);
		m_BodyStyle.wordWrap = true;
<<<<<<< HEAD
		m_BodyStyle.fontSize = 12;
		
		m_TitleStyle = new GUIStyle(m_BodyStyle);
		m_TitleStyle.fontSize = 22;

		m_HeadingStyle = new GUIStyle(m_BodyStyle);
		m_HeadingStyle.fontSize = 14;
		m_HeadingStyle.fontStyle = FontStyle.Bold;
		
		m_LinkStyle = new GUIStyle(m_BodyStyle);
=======
		m_BodyStyle.fontSize = 14;
		
		m_TitleStyle = new GUIStyle(m_BodyStyle);
		m_TitleStyle.fontSize = 26;
		
		m_HeadingStyle = new GUIStyle(m_BodyStyle);
		m_HeadingStyle.fontSize = 18 ;
		
		m_LinkStyle = new GUIStyle(m_BodyStyle);
		m_LinkStyle.wordWrap = false;
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
		// Match selection color which works nicely for both light and dark skins
		m_LinkStyle.normal.textColor = new Color (0x00/255f, 0x78/255f, 0xDA/255f, 1f);
		m_LinkStyle.stretchWidth = false;
		
		m_Initialized = true;
	}
	
	bool LinkLabel (GUIContent label, params GUILayoutOption[] options)
	{
		var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

		Handles.BeginGUI ();
		Handles.color = LinkStyle.normal.textColor;
		Handles.DrawLine (new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
		Handles.color = Color.white;
		Handles.EndGUI ();

		EditorGUIUtility.AddCursorRect (position, MouseCursor.Link);

		return GUI.Button (position, label, LinkStyle);
	}
}

