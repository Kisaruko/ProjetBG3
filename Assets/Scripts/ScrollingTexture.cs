using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour {
    Material m_mat;
    public float scrollXSpeed;
    public float scrollYSpeed;
    private Vector2 scrollSpeed;
    private void Start()
    {
        m_mat = GetComponent<MeshRenderer>().material;
        scrollSpeed = new Vector2(scrollXSpeed, scrollYSpeed);
    }
    void LateUpdate ()
    {
        m_mat.mainTextureOffset = scrollSpeed * Time.time;
	}
}
