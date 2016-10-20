using UnityEngine;
using System.Collections;

public class OnInteractionLookAt : MonoBehaviour {
    public GameObject interactionObject;
    public bool smooth;
    public GameObject lookAtLocation;
    public bool knownFlip;

    private float rotateX;
    private float rotateY;
    private GameObject player;
    public bool doOnce;
    private Interaction interact;
    void Start()
    {
        if (interactionObject)
        {
            interact = interactionObject.GetComponent<Interaction>();
        }else
        {
            interact = GetComponent<Interaction>();
        }
    }
	// Update is called once per frame
	void Update () {
        if (doOnce)
        {
            return;
        }
        if (interact._interactionFinished)
        {
            LookAt();
        }
	}

    public void LookAt()
    {
        //determine look at location rotation
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 lookAtPos = lookAtLocation.transform.position;
        Vector3 vecPlayerPoint = (player.transform.position - lookAtPos);
        Vector3 UnitStart = new Vector3(0f, 0f, -1f);
        Vector3 vecYPlane = new Vector3(vecPlayerPoint.x, 0f, vecPlayerPoint.z);
        rotateY = Vector3.Angle(UnitStart, vecYPlane);//GameManager.aGameManager.getOriginalPlayerY();
        rotateX = -Vector3.Angle(new Vector3(1f, 0f, 0f), new Vector3(Mathf.Sqrt(Mathf.Pow(vecPlayerPoint.x, 2) + Mathf.Pow(vecPlayerPoint.z, 2)), vecPlayerPoint.y, 0f));

        //determine if y is positive or negative, as angle is absolute value, never greater than 180, which is great for rotatex but sucks for y
        if (Vector3.Dot(new Vector3(0f,0f,-1f),vecYPlane) < Vector3.Dot(new Vector3(1f,0f,0f), vecYPlane) || knownFlip)
        {
            rotateY = -rotateY; //counterclock
        }

        //rotateY += GameManager.aGameManager.getOriginalPlayerY();
        
        Debug.Log(rotateX + " xrot " + rotateY + " yrot " + vecPlayerPoint + " player to point ");


        if (smooth)
        {
            FirstPersonController.aFirstPersonController.RotateCharacterSmooth(rotateX, rotateY);
        }
        else
        {
            FirstPersonController.aFirstPersonController.RotateCharacter(rotateX, rotateY);
        }

        //doOnce = true;
        interact._interactionFinished = false;
    }
}
