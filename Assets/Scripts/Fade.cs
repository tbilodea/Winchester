using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
    Animator anim;
    bool isFading = false;
    public static Fade aFade; //Singleton this class
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        aFade = Singleton<Fade>.Instance; //singular instance of aFade
    }

    public IEnumerator FadetoClear()
    {
        isFading = true;
        anim.SetTrigger("fadeOutOfScene");

        while (isFading)
            yield return null;


    }

    public IEnumerator FadetoBlack()
    {
        isFading = true;
        anim.SetTrigger("fadeIntoScene");

        while (isFading)
            yield return null;

    }

    void AnimationComplete()
    {
        isFading = false;
    }
}
