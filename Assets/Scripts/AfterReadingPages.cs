using UnityEngine;
using System.Collections;

public class AfterReadingPages : MonoBehaviour {
    [SerializeField] private GameObject _note1;
    [SerializeField] private GameObject _note2;
    [SerializeField] private GameObject _note3;
    [SerializeField] private GameObject _hannover;
    [SerializeField] private GameObject _Door;
    [SerializeField] private GameObject _WarpBackDoor;

    private Door _libraryDoor;
    private bool doOnce;
    private bool hannoverDone;

    void Start()
    {
        _libraryDoor = _Door.GetComponent<Door>();
    }

	// Update is called once per frame
	void Update () {
        if (_note1.GetComponent<Interaction>()._interactionFinished && _note2.GetComponent<Interaction>()._interactionFinished && _note3.GetComponent<Interaction>()._interactionFinished && !doOnce)
        {
            _libraryDoor.doorLockedForever = true;
            _libraryDoor.CloseDoor();
            GetComponent<Interaction>().interactionAction();

            doOnce = true;
        }

        if (GetComponent<Interaction>()._interactionFinished)
        {
            if (_hannover)
            {
                _hannover.SetActive(true);
            }
            hannoverDone = true;
        }
        if (_hannover)
        {
            if (!_hannover.activeInHierarchy && hannoverDone) //if ms hannover left open the door
            {
                _libraryDoor.doorLockedForever = false;
                _WarpBackDoor.GetComponent<Interaction>().setInteractionType(4);
            }
        }
    }
}
