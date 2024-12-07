using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ButtonAttribute : PropertyAttribute {
	public string Name { get; set; }
	public bool HideInPlayMode { get; set; }
	public bool HideInEditMode { get; set; }

	public ButtonAttribute() : this("") { }

	public ButtonAttribute(string name = "", bool hideInPlayMode = false, bool hideInEditMode = false) {
		Name = name;
		HideInPlayMode = hideInPlayMode;
		HideInEditMode = hideInEditMode;
	}
}
