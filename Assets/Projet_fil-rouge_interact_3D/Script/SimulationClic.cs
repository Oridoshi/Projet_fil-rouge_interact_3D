using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SimulationClic : MonoBehaviour
{
    [Header("Configuration UI")]
    public Button debugButton;

    [Header("Configuration Touche")]
    public InputActionProperty inputActiveDebugButton; 

    private void OnEnable()
    {
        // Active l'écoute de la touche quand le script est activé
        if (inputActiveDebugButton.action != null)
        {
            inputActiveDebugButton.action.Enable();
            // On s'abonne à l'événement 'performed'
            inputActiveDebugButton.action.performed += OnDebugButtonTriggered;
        }
    }

    private void OnDisable()
    {
        if (inputActiveDebugButton.action != null)
        {
            // Désabonnement pour éviter les erreurs de mémoire
            inputActiveDebugButton.action.performed -= OnDebugButtonTriggered;
            inputActiveDebugButton.action.Disable();
        }
    }

    private void OnDebugButtonTriggered(InputAction.CallbackContext context)
    {
        // Simule le clic si le bouton est assigné et interactif
        if (debugButton != null && debugButton.interactable)
        {
            debugButton.onClick.Invoke();
            Debug.Log("Bouton cliqué via inputActiveDebugButton !");
        }
    }
}