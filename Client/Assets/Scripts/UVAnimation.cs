using UnityEngine;
using System.Collections;

public class UVAnimation : MonoBehaviour {

    private Vector2 m_offset = Vector2.zero;
    public Vector2 Speed = Vector2.zero;
    public Material Mat = null;
    public bool IsNguiTexture = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        m_offset += Speed;
        if (m_offset.x >= 1)
        {
            m_offset.x = 0;
        }
        else if (m_offset.x <= 0)
        {
            m_offset.x = 1;
        }
        if (m_offset.y >= 1)
        {
            m_offset.y = 0;
        }
        else if (m_offset.y <= 0)
        {
            m_offset.y = 1;
        }

        if (Mat == null)
        {
            if (IsNguiTexture)
            {
                gameObject.GetComponent<UITexture>().material.SetTextureOffset("_MainTex", m_offset);
            }
            else
            {
                gameObject.renderer.material.SetTextureOffset("_MainTex", m_offset);
            }
        }
        else
        {
            for (int i = 0; i < this.renderer.materials.Length; i++ )
            {
                Material mat = IsNguiTexture ? gameObject.GetComponent<UITexture>().material : gameObject.renderer.materials[i];
                if (mat.name.Length >= Mat.name.Length && mat.name.Substring(0, Mat.name.Length) == Mat.name)
                {
                    mat.SetTextureOffset("_MainTex", m_offset);
                    break;
                }
            }
        }        
	}
}
