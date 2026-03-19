using UnityEngine;
using UnityEngine.UI; // Si c'est un slider UI classique utilisé en VR
// using UnityEngine.XR.Interaction.Toolkit.UI; // Décommente si c'est un slider spécifique XR

public class AudioSliderLinker : MonoBehaviour
{
    [Header("Références")]
    public AudioSource targetAudioSource;
    public Slider volumeSlider; // Ton slider VR

    void Start()
    {
        if (volumeSlider != null && targetAudioSource != null)
        {
            // Initialise le slider avec le volume actuel de la source
            volumeSlider.value = targetAudioSource.volume;

            // Ajoute un écouteur : chaque fois que le slider bouge, on appelle OnSliderValueChanged
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void OnSliderValueChanged(float value)
    {
        // La valeur 'value' est généralement comprise entre 0 et 1
        targetAudioSource.volume = value;
        
        Debug.Log($"[Volume] Nouvelle valeur : {value * 100}%");
    }

    private void OnDestroy()
    {
        // Bonne pratique : on retire l'écouteur quand l'objet est détruit
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}