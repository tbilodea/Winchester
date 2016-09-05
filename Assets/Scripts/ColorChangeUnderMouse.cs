using UnityEngine;
using System.Collections;

public class ColorChangeUnderMouse : MonoBehaviour {
    private TextMesh txMesh;

    void Start()
    {
        txMesh = gameObject.GetComponent<TextMesh>();

    }
    void OnMouseOver()
    {
        txMesh.color = Color.red;
    }

    void OnMouseExit()
    {
        txMesh.color = Color.white;
    }
	
}
