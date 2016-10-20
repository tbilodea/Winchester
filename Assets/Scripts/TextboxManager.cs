using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class TextboxManager : MonoBehaviour {
    public static TextboxManager aTextboxManager;
    public bool continueOn;
    public TextAsset credits;
    
    private Text _textAsset; //UI's showing text
    private string _currentText; //current text showing
    private string[] _textToShow; //text that will show in the end
    private bool _printing; //true while adding letters, interrupted by continueOn
    private int _lineOfText; //current line that we are on
    private int _lengthOfText;
    private bool _breakLetterPrinter;
    private Interaction calledByThisInteraction;
    private AudioSource textAudioSource;
    
    //IO for files
    private FileStream _fileStream;
    private StreamReader _streamReader;

    // Use this for initialization
    void Start() {
        aTextboxManager = Singleton<TextboxManager>.Instance;
        if (gameObject.transform.childCount > 0) {
            _textAsset = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        }
        _currentText = ""; //show nothing to begin with
        gameObject.SetActive(false); //and turn off our textbox

        textAudioSource = gameObject.GetComponent<AudioSource>();
        if (credits)
        {
            loadScript(credits, null);
        }
    }

    //this loads the script in so that it is ready for printing
    public void loadScript(TextAsset txtAsset, Interaction thisInteraction)
    {
        calledByThisInteraction = thisInteraction;
        //open textScript and read into _textToShow array by line
        if(txtAsset != null)
        {
            _textToShow = new string[1];
            _textToShow = txtAsset.text.Split('\n');
        }else //there is an error otherwise-should be that the txtasset was not put into the interaction script
        {
            _textToShow = new string[1];
            _textToShow[0] = "Nothing in textAsset fed to TextboxManager from Interaction script.";
            Debug.LogError(_textToShow[0]);
        }

        _lineOfText = 0;
        _printing = false;
        enableTextbox(); //begins textbox
    }

    private void enableTextbox()
    {
        gameObject.SetActive(true);
        continueOn = true;
        //disable grab hand HUD
        //FREEZE PLAYER MOVEMENT FirstPersonController.aFirstPersonController.frozen = true;
    }
    
    private void disableTextbox()
    {
        gameObject.SetActive(false);
        _textAsset.text = "";
        _lineOfText = 0;
        if (calledByThisInteraction)
        {
            calledByThisInteraction._interactionFinished = true;
        }
        //UNFREEZE PLAYER MOVEMENT
    }

    //once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy) //checks the object is active
        {
            if (continueOn) //if button is pressed to proceed to change textbox accordingly
            {
                continueOn = false; //ready for next input
                if (_lineOfText < _textToShow.Length || _printing) //check if we have shown all text or are still printing
                {//THIS AREA COULD CRASH THE GAME WITH A OUTSIDE ARRAY BOUNDS EXCEPTION IF _printing SWITCHES BETWEEN THESE TWO IFS
                    if (_printing) //check if we are cancelling the rest of printing
                    {
                        _breakLetterPrinter = true; //this will break the coroutine and print the whole text
                        _printing = false;
                    }
                    else //if continue on and all of the line is showing move to the next line
                    {
                        _printing = true;
                        StartCoroutine(printByLetter(_textToShow[_lineOfText])); //scroll through text
                        _lineOfText++; //go to next line
                    }
                }else //close textbox if done with showing text
                {
                    disableTextbox();
                }
            }
        }
    }

    private IEnumerator printByLetter(string printThis)
    {
        _lengthOfText = 0;
        while (!_breakLetterPrinter && _printing) //add one letter at a time until cancelled or finished
        {
            _currentText = printThis.Substring(0,_lengthOfText);
            _lengthOfText++;
            _textAsset.text =_currentText;

            textAudioSource.Play();
            yield return new WaitForSeconds(.05f); //give a pause so it looks like it's slowly scrolling out
            if (_lengthOfText == printThis.Length)
            {
                _printing = false; //exits loop
            }
        }
        continueOn = false; //reset cancelled so you can exit the next set
        _breakLetterPrinter = false;
        _textAsset.text = printThis; //make sure this is displayed nontheless
    }
}
