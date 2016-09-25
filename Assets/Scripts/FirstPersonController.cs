using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;


[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (AudioSource))]
public class FirstPersonController : MonoBehaviour
{
    public static FirstPersonController aFirstPersonController;
    public GameObject _holdingThisGameobject; //gameobject being held

    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;
    [SerializeField] private MouseLook m_MouseLook;
    [SerializeField] private bool m_UseFovKick;
    [SerializeField] private FOVKick m_FovKick = new FOVKick();
    [SerializeField] private bool m_UseHeadBob;
    [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
    [SerializeField] private float m_StepInterval;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
    [SerializeField] private float _lengthOfHands;       //determines how far away you can interact with things

    private Camera m_Camera;
    private bool m_Jump;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;
    private AudioSource m_AudioSource;
    private bool _interacting;           //player interacts with an object
    private bool _interactable;          //object that is within range is interactable
    private float _time;
    private GameObject playerHand; //gameobject to store anything that is movable
    private bool _isObjectInHand; //if there is an object in player hand
    private LayerMask interactableLayer;

    // Use this for initialization
    private void Start()
    {
        aFirstPersonController = Singleton<FirstPersonController>.Instance; //only creates one instance of the FPC

        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_FovKick.Setup(m_Camera);
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle/2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();
		m_MouseLook.Init(transform , m_Camera.transform);
        playerHand = gameObject.transform.GetChild(1).gameObject;
        PauseMenu.aPauseMenu.gameObject.SetActive(false);
        interactableLayer.value = LayerMask.GetMask("Interactable");
    }


    // Update is called once per frame
    private void Update()
    {
        //This is menu control
        if (Input.GetKeyDown(KeyCode.Escape)) //control pause menu
        {
            if (!PauseMenu.aPauseMenu.gameObject.activeInHierarchy) //if the menu isn't up
            {
                Cursor.visible = true;
                PauseMenu.aPauseMenu.gameObject.SetActive(true); //make it appear
            }else
            {
                Cursor.visible = false;
                PauseMenu.aPauseMenu.gameObject.SetActive(false); //else make it disappear
            }
        }

        //Player Control
        if (!PauseMenu.aPauseMenu.gameObject.activeInHierarchy)
        { //check if pause menu is up, only thing that works are the buttons then
            if (!TextboxManager.aTextboxManager.gameObject.activeInHierarchy) //check if textbox is alive
            {
                //revising parts of FPC

                //check for interaction controls
                RaycastHit hit;
                _time += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Mouse0) && _time > 0.4)
                {
                    _interacting = true;
                }

                Physics.Raycast(new Ray((m_Camera.transform.position), m_Camera.transform.forward), out hit, _lengthOfHands, interactableLayer);
                //checks what is under the mouse and raycasts through it in a reasonable distance
            
                if (!_isObjectInHand) //as long as there is no object already in your hand
                {
                    if (hit.collider)
                    {
                        Debug.Log(hit.collider.name);
                        if (hit.collider.GetComponent<Interaction>())//check if we can interact with the object
                        {
                            _interactable = true; //show grab hand below

                            if (_interacting) //check if we are trying to interact with the object
                            {
                                hit.collider.gameObject.GetComponent<Interaction>().interactionAction(); //activates interaction script on object
                                _interacting = false;
                                _time = 0f;
                            }
                        }
                    }
                }
                else //if there is something in hand
                {
                    if (_interacting) //if we are trying to interact
                    {
                        removeFromHand(); //drop object
                    }
                }

                _interacting = false; //reset interacting timer
                                      //sets grabhand on and off
                if (_interactable)
                {
                    HUD.aHUD.toggleGrab(true); //turns hand on
                    _interactable = false;
                }
                else
                {
                    HUD.aHUD.toggleGrab(false); //else turn it off
                }

                //continue generic FPC
                RotateView();
                // the jump state needs to read here to make sure it is not missed
                if (!m_Jump)
                {
                    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                }

                if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
                {
                    StartCoroutine(m_JumpBob.DoBobCycle());
                    PlayLandingSound();
                    m_MoveDir.y = 0f;
                    m_Jumping = false;
                }
                if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
                {
                    m_MoveDir.y = 0f;
                }

                m_PreviouslyGrounded = m_CharacterController.isGrounded;
            }
            else
            {
                //only thing we can do is forward the text
                HUD.aHUD.toggleGrab(false); //turn off grab because we are in the textbox
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    TextboxManager.aTextboxManager.continueOn = true;
                }
            }
        }
    }

    //sets and removes objects from player hand
    public void setIntoHand(GameObject thisInHand)
    {
        //set instead the object to try to get to the transform using a velocity^2 by distance
        playerHand.GetComponent<HandObject>().setParentByVelocity(thisInHand);
        
        //sets object for FPC
        _isObjectInHand = true;
        _holdingThisGameobject = thisInHand;
    }

    //removes an object from player's hand
    public void removeFromHand()
    {
        playerHand.GetComponent<HandObject>().stopParentByVelocity(); //drop the gameobject
        _isObjectInHand = false;
        _time = 0f; //reset timer to not pick it up again
    }
    
    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }


    private void FixedUpdate()
    {
        if (!PauseMenu.aPauseMenu.gameObject.activeInHierarchy)//again check for pausemenu
        {
            if (!TextboxManager.aTextboxManager.gameObject.activeInHierarchy) //check for textbox active
            {
                float speed;
                GetInput(out speed);
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

                // get a normal for the surface that is being touched to move along it
                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                    m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;


                if (m_CharacterController.isGrounded)
                {
                    m_MoveDir.y = -m_StickToGroundForce;

                    if (m_Jump)
                    {
                        m_MoveDir.y = m_JumpSpeed;
                        PlayJumpSound();
                        m_Jump = false;
                        m_Jumping = true;
                    }
                }
                else
                {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }
                m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

                ProgressStepCycle(speed);
                UpdateCameraPosition(speed);

                m_MouseLook.UpdateCursorLock();
            }
        }
    }


    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }


    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                            Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }


    private void PlayFootStepAudio()
    {
        if (!m_CharacterController.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }


    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!m_UseHeadBob)
        {
            return;
        }
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
        {
            m_Camera.transform.localPosition =
                m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                    (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }
        m_Camera.transform.localPosition = newCameraPosition;
    }


    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
        }
    }


    private void RotateView()
    {
        m_MouseLook.LookRotation (transform, m_Camera.transform);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
    }
}

