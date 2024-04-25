using System;
using UnityEngine;

public class Readme : ScriptableObject {
	public Texture2D icon;
<<<<<<< HEAD
	public float iconMaxWidth = 128f;
=======
>>>>>>> d1b153c3b18e8e2940077858b14cdacb8a610b46
	public string title;
	public Section[] sections;
	public bool loadedLayout;
	
	[Serializable]
	public class Section {
		public string heading, text, linkText, url;
	}
}
