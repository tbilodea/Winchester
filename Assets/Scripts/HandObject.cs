using UnityEngine;
using System.Collections;

public class HandObject : MonoBehaviour {

    private Camera _mCamera;
    private GameObject _beingHeld;
    private Rigidbody _heldRigidbody;
    [SerializeField]private float _objectSpeed;

    // Use this for initialization
    void Start() {
        _mCamera = FindObjectOfType<Camera>();
        if (_objectSpeed==0)
        {
            _objectSpeed = 5f;
        }

    }

    // Update is called once per frame
    void Update() {

        //making the hand gameobject transform be in front of the camera
        gameObject.transform.position = FirstPersonController.aFirstPersonController.transform.position + (_mCamera.transform.forward) * 1.5f + new Vector3(0f, .35f, 0f);
        if (gameObject.transform.childCount > 0)
        {
            gameObject.transform.GetChild(0).transform.position = gameObject.transform.position;
        }

        //checking for object, and putting it close to the transform as possible.
        if (_beingHeld)
        {
            float x = -_beingHeld.transform.position.x + gameObject.transform.position.x;
            float y = -_beingHeld.transform.position.y + gameObject.transform.position.y;
            float z = -_beingHeld.transform.position.z + gameObject.transform.position.z;
            float mag = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
            if (mag < .05f) //to check if object has gotten close enough to the center (stops jittery movement)
            {
                _heldRigidbody.velocity = new Vector3(0f, 0f, 0f);
            }
            else
            {
                Vector3 normalized = new Vector3(x / mag, y / mag, z / mag);
                _heldRigidbody.velocity = normalized * _objectSpeed*mag*2;
            }
        }

    }

    //uses the game object in the person's hand and makes it try to go to the holding point on the player
    public void setParentByVelocity(GameObject holdingThis)
    {
        _beingHeld = holdingThis;
        _heldRigidbody = holdingThis.GetComponent<Rigidbody>();
        //_heldRigidbody.freezeRotation = true;
        _heldRigidbody.useGravity = false;
    }

    //ceases the function setparentbyveloctiy
    public void stopParentByVelocity()
    {
        //_heldRigidbody.freezeRotation = false;
        _heldRigidbody.useGravity = true;
        _beingHeld = null;
    }
}
