using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
// Ajoute bien le namespace pour que le script reconnaisse GlobalNonNativeKeyboard
using UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard;

public class VRKeyboardTrigger : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // On récupère le composant texte sur lequel on vient de cliquer
        TMP_InputField myInputField = GetComponent<TMP_InputField>();

        if (myInputField != null && GlobalNonNativeKeyboard.instance != null)
        {
            // On appelle la méthode ShowKeyboard du singleton en lui passant notre champ
            GlobalNonNativeKeyboard.instance.ShowKeyboard(myInputField);
            
            Debug.Log($"[Keyboard] Ouverture pour : {gameObject.name}");
        }
        else if (GlobalNonNativeKeyboard.instance == null)
        {
            Debug.LogError("GlobalNonNativeKeyboard.instance est introuvable dans la scène !");
        }
    }
}