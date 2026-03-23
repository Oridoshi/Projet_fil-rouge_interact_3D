using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Nécessaire pour IEnumerator
using System.Collections.Generic;

public class InfoSectionUI : MonoBehaviour
{
    private List<Text> infoValues = new List<Text>();
    public static InfoSectionUI instance{get ; private set;}
    
    private void Awake()
    {
        if(!instance || instance != this) instance = this;
        else Destroy(gameObject);
    }
    
    // On transforme le Start en Coroutine
    public IEnumerator UpdateInfoUI()
    {
        // 1. Initialisation des composants (on le fait direct)
        foreach (Transform child in transform)
        {
            Transform valueTransform = child.Find("InfoValue");
            if (valueTransform != null)
            {
                Text textComp = valueTransform.GetComponent<Text>();
                if (textComp != null) infoValues.Add(textComp);
            }
        }

        Debug.Log("<color=cyan>[UI]</color> Attente des données météo...");

        // 2. BOUCLE D'ATTENTE : Tant que weatherData est null, on attend la frame suivante
        // On vérifie aussi que l'instance existe pour éviter les erreurs
        while (WeatherManager.instance == null || WeatherManager.instance.weatherData == null)
        {
            yield return null; // Attend la prochaine frame
        }

        Debug.Log("<color=green>[UI]</color> Données reçues ! Mise à jour de l'affichage.");

        // 3. UNE FOIS LES DONNÉES PRÊTES, ON REMPLIT
        var wdata = WeatherManager.instance.weatherData;
        List<string> textInfo = new List<string>();
        
        // On récupère les vraies données maintenant qu'on est sûr qu'elles existent
        textInfo.Add(wdata.weather[0].main);           // Météo (ex: Clear)
        textInfo.Add(wdata.main.temp.ToString("F1") + "°C"); // Température
        textInfo.Add(wdata.wind.speed.ToString("F1") + " m/s"); // Vent
        textInfo.Add(wdata.wind.deg.ToString() + "°"); // Direction
        textInfo.Add(wdata.main.humidity.ToString() + "%"); // Humidité

        // 4. APPLICATION
        for (int i = 0; i < infoValues.Count; i++)
        {
            if (i < textInfo.Count)
            {
                infoValues[i].text = textInfo[i];
            }
        }
    }
}