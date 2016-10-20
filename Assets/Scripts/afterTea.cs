using UnityEngine;
using System.Collections;

public class afterTea : MonoBehaviour {
    [SerializeField] private GameObject _interactionObject;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _unlockLevelDoor;

    private Animator _anim;
    private Interaction _interaction;
    private NavMeshAgent _navMesh;
    private Vector3 position;
    
    
	// Use this for initialization
	void Start () {
        _interaction = _interactionObject.GetComponent<Interaction>();
        _anim = GetComponent<Animator>();
        _navMesh = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_interaction._interactionFinished) //when the dialogue is done
        {
            _interaction.gameObject.SetActive(false);
            StartCoroutine(doCommands());
            makeDoorWarp();
            _unlockLevelDoor.GetComponent<Door>().doorLockedForever = false;
            //check how close character is to the door, if close they disappear
            position = gameObject.transform.position;
            if(Mathf.Sqrt(Mathf.Abs(position.x-_target.transform.position.x)+Mathf.Abs(position.y-_target.transform.position.y)) < 1.5)
            {
                
               
                gameObject.SetActive(false);
            }
            
        }
	}

    private IEnumerator doCommands()
    {
        _anim.SetBool("sitting", false); //make character stand up
        yield return new WaitForSeconds(3f);
        if(gameObject.name == "robin")
        {
            _anim.SetBool("walking", true);
        }else
        {
            _anim.SetTrigger("walk");
        }
        _navMesh.SetDestination(_target.transform.position);
       
        yield return null;
    }

    private void makeDoorWarp()
    {
        if (_target)
        {
            
        }
    }
}
