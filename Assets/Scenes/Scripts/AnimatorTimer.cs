using System.Collections;
using UnityEngine;

public class AnimationTimer : MonoBehaviour
{
    public Animator animator;
    public string animationTriggerName = "SitTrigger"; // nombre del trigger en el Animator
    public float duration = 5f; // duración en segundos

    void Start()
    {
        StartCoroutine(PlayAndStopAnimation());
    }

    IEnumerator PlayAndStopAnimation()
    {
        // Activa el trigger para iniciar la animación
        animator.SetTrigger(animationTriggerName);

        // Espera la duración deseada
        yield return new WaitForSeconds(duration);

        // Opcional: vuelve al estado "Idle" o detiene el Animator
        animator.enabled = false; // Detiene la animación completamente
        // O puedes hacer una transición a otro estado si tu Animator está configurado
        // animator.Play("Idle"); 
    }
}
