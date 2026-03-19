using UnityEngine;
using UnityEngine.UI;
using TMPro; // À retirer si tu utilises des InputField classiques

public class WeatherConfigUI : MonoBehaviour
{
    [Header("Champs de saisie")]
    public TMP_InputField latInput;
    public TMP_InputField lonInput;
    public Button applyButton;

    void Start()
    {
        // On lie le clic du bouton à la fonction de mise à jour
        if (applyButton != null)
        {
            applyButton.onClick.AddListener(OnApplyClicked);
        }

        // Optionnel : Afficher les valeurs actuelles du manager au départ
        if (WeatherManager.instance != null)
        {
            latInput.text = WeatherManager.instance.latitude.ToString();
            lonInput.text = WeatherManager.instance.longitude.ToString();
        }
    }

    public void OnApplyClicked()
    {
        // 1. On essaie de convertir le texte en float (plus précis que int pour la position)
        if (float.TryParse(latInput.text, out float newLat) && 
            float.TryParse(lonInput.text, out float newLon))
        {
            Debug.Log($"<color=green>[Config]</color> Application : Lat {newLat}, Lon {newLon}");

            // 3. On demande au manager de relancer la coroutine
            // On utilise StopAllCoroutines pour éviter d'avoir deux requêtes en même temps
            WeatherManager.instance.StopAllCoroutines();
            WeatherManager.instance.StartCoroutine(WeatherManager.instance.GetWeatherAndSetTime(newLat, newLon));
        }
        else
        {
            Debug.LogError("Format de Latitude ou Longitude invalide !");
        }
    }
}