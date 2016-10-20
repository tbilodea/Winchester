using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {
    private NavMeshAgent _navMeshAgent;
    private float _timeLastSeen = -5;
    private float _timeSearchStart = -3;
    private bool _playerIsSeen = false;
    private Transform _playerTransform;
    private GameObject _player;
    private Animator _anim;
    private bool _searchWaitSpot;
    private bool _capturedPlayer;

    
    [SerializeField] private float _aiRunSpeed;
    [SerializeField] private float _aiWalkSpeed;
    [SerializeField] private float _searchTime;
    [SerializeField] private float _walkRadius;
    [SerializeField] private float _playerCaughtRadius;
    [SerializeField] private float _distanceOfSight;
    [SerializeField] private bool _playSoundOnAlerted;
    [SerializeField] private float degreesInView;

    [SerializeField] private float _timerLife;

    [SerializeField] private GameObject onEnableAlert;

    public bool Stay;

    private bool alertOnce;
    private float _timeLastPlayed;
    private float _birthTime;

    // Use this for initialization
    void Start () {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _anim = GetComponent<Animator>();
        if (degreesInView == 0)
        {
            degreesInView = 45;
        }
        if(_searchTime == 0)
        {
            _searchTime = 2;
        }
        if(_aiRunSpeed == 0)
        {
            _aiRunSpeed = 5;
        }
        if(_aiWalkSpeed == 0)
        {
            _aiWalkSpeed = 3;
        }
        if(_walkRadius == 0)
        {
            _walkRadius = 10;
        }
        if(_playerCaughtRadius == 0)
        {
            _playerCaughtRadius = 2;
        }
        _birthTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!_capturedPlayer && !Stay)
        {
            if (_timerLife != 0)
            {
                if (Time.time - _birthTime > _timerLife)
                {
                    if (!GetComponent<OnPlayerCapture>().isWorking)
                    {
                        GetComponent<OnPlayerCapture>().PlayerNotCaptured();
                        Destroy(gameObject);
                    }
                }
            }
            _anim.SetFloat("speed", _navMeshAgent.velocity.magnitude);
            //check if player is in sight
            checkPlayer();

            if (_playerIsSeen)
            {
                _playerIsSeen = false; //reset to prepare next round
                _timeLastSeen = Time.time; //sets time last seen
                rushPlayer();
            }
            else if (Time.time - _timeLastSeen < _searchTime)
            {
                alerted();
            }
            else
            {
                search();
            }
        }
	}

    private void checkPlayer()
    {

        Vector3 AIForward = gameObject.transform.forward;
        _playerTransform = _player.transform;
        Vector3 AIposition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Vector3 VecToPlayer = new Vector3(_playerTransform.position.x-AIposition.x,_playerTransform.position.y+.5f-AIposition.y,_playerTransform.position.z-AIposition.z);
        float angle = Vector3.Angle(AIForward, VecToPlayer);

        //DrawLine(AIposition, VecToPlayer, Color.white, .2f);

        RaycastHit hit;
        Ray toPlayer = new Ray(AIposition, VecToPlayer);
        Physics.Raycast(toPlayer, out hit);
        //drawMyLine(hit.point, AIposition, Color.white, 0.2f);
        Debug.Log(hit.collider.tag);
        if(hit.collider.tag == "Player")
        {
            if(angle < degreesInView)
            {
                if(_distanceOfSight == 0 || _distanceOfSight > VecToPlayer.magnitude)
                {
                    _playerIsSeen = true;
                }
            }

            if (VecToPlayer.magnitude < _playerCaughtRadius) ////end condition
            {
                _capturedPlayer = true;
                StartCoroutine(GetComponent<OnPlayerCapture>().PlayerCaptured());
                Debug.Log("AI has captured the player");
                GetComponent<AudioSource>().Play();
            }
            if (VecToPlayer.magnitude < 4) //too close to ai, alerted by footsteps
            {
                _playerIsSeen = true;
            }
        }

        if (onEnableAlert)
        {
            if (onEnableAlert.activeInHierarchy && !alertOnce)
            {
                _playerIsSeen = true;
                alertOnce = true;
            }
        }
    }

    void rushPlayer()
    {
        Debug.Log("rushplayer");
        if (_playSoundOnAlerted)
        {
            if (!GetComponent<AudioSource>().isPlaying && Time.time-_timeLastPlayed>15f)
            {
                _timeLastPlayed = Time.time;
                GetComponent<AudioSource>().Play();
            }
        }
        _anim.SetBool("run", true);
        _navMeshAgent.speed = _aiRunSpeed;
        _navMeshAgent.destination = _player.transform.position;
    }

    void alerted()
    {
        Debug.Log("alerted");
        _anim.SetBool("run", false);
        _navMeshAgent.speed = _aiWalkSpeed;
        _navMeshAgent.destination = _player.transform.position;
    }

    void search()
    {
        Debug.Log("searching");
        if(Time.time - _timeSearchStart > 2 && _searchWaitSpot)
        {
            //move to random point in the area
            Vector3 randomDirection = Random.insideUnitSphere * _walkRadius;
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position+randomDirection, out hit, _walkRadius, 1);
            Vector3 finalPosition = hit.position;
            _navMeshAgent.destination = finalPosition;
            _searchWaitSpot = false;
        }

        //wait for a second before moving on
        if(_navMeshAgent.velocity.magnitude <= 0.1 && !_searchWaitSpot) {
            _searchWaitSpot = true;
            _timeSearchStart = Time.time;
        }
    }

    public void setCapturedPlayer()
    {
        _capturedPlayer = false;
    }
   void DrawLine(Vector3 point, Vector3 vector, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = point;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, point);
        lr.SetPosition(1, point+vector);
        GameObject.Destroy(myLine, duration);
    }
}
