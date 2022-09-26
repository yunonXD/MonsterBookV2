using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceFadeOut : MonoBehaviour
{
    private bool FadeOutTime = false;
    public Material m_Material;
    private Color m_color;
    // Start is called before the first frame update
    void Start()
    {
        m_Material = gameObject.GetComponent<MeshRenderer>().material;
        Invoke("FadeOut", 2.0f);
        this.gameObject.GetComponent<MeshRenderer>().material = Instantiate(m_Material);
        m_color = this.gameObject.GetComponent<MeshRenderer>().material.color;


    }

    // Update is called once per frame
    void Update()
    {
        if (FadeOutTime == true)
        {
            Debug.Log(m_Material.color.a);
            m_color.a -= 0.01f;
            this.gameObject.GetComponent<MeshRenderer>().material.color = m_color;
            if (m_color.a < 0)
            {
                this.gameObject.SetActive(false);
            }
        }
       
    }
    private void FadeOut()
    {
        FadeOutTime = true;
    }
}
