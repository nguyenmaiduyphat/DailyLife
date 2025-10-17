using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioClip hoverClip;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(hoverClip);
        animator.Play("HoverButton"); 
        Debug.Log("Begin Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.Play("ExitHoverButton");
        Debug.Log("End Hover");
    }
}
