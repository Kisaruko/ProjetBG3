using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
    public bool isFiltered;
	#region MonoBehaviour Methods
	/// <summary>
	/// Called when image is renderer for graphics
	/// </summary>
	/// <param name="_Source">Source image</param>
	/// <param name="_Destination">Destination Image</param>
	private void OnRenderImage(RenderTexture _Source, RenderTexture _Destination)
	{
        if (isFiltered)
        {
            Graphics.Blit(_Source, _Destination, m_Material);
        }
        else
        {
            _Destination = null;
            Graphics.Blit(_Source, null as RenderTexture);
        }
        // Set the image to draw. 
        // On fait passer l'image dans un filtre, et on la fait ressortir avant de la rendre à l'image
    }

    #endregion

    #region Attributes

    [SerializeField]
	public Material m_Material = null;

	#endregion
}
