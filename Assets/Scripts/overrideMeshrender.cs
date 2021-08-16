using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overrideMeshrender : MonoBehaviour
{

    public Color hoverColor;
    public Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameObject.GetComponent<Material>().SetColor("_Color", new Vector4(1,1,1,1));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameObject.GetComponent<Material>().SetColor("_Color", new Vector4(0,0,0,0));
        }


    }

    public void IsHover(bool hover)
    {

        foreach (Transform childButton in gameObject.transform)
        {

            foreach (Transform childOfChildButton in childButton)
            {
                Debug.Log("Found 2nd Child");
                if (childOfChildButton.CompareTag("quad"))
                {
                    Debug.Log("Entered Quad");
                    if (hover)
                        childOfChildButton.GetComponent<MeshRenderer>().material.SetColor("_Color", hoverColor);
                    else
                        childOfChildButton.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);

                }

            }
        }
    }
}
