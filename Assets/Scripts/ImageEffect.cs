using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
	#region MonoBehaviour Methods

	// Use this for initialization
	private void Start()
	{

	}

	// Update is called once per frame
	private void Update()
	{

	}

	/// <summary>
	/// Called when image is renderer for graphics
	/// </summary>
	/// <param name="_Source">Source image</param>
	/// <param name="_Destination">Destination Image</param>
	private void OnRenderImage(RenderTexture _Source, RenderTexture _Destination)
	{
		// Set the image to draw
		Graphics.Blit(_Source, _Destination, m_Material);
	}

	#endregion

	#region Attributes

	[SerializeField]
	public Material m_Material = null;

	#endregion
}
