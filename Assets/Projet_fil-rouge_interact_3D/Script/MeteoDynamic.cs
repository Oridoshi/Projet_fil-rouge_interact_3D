using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
// Assure-toi que cette ligne correspond au namespace de ton asset Tenkoku
// Parfois, selon la version, c'est "using Tenkoku.Core;"
using Tenkoku.Core; 

public class WeatherManager : MonoBehaviour
{
    [Header("Configuration API")]
    public string apiKey = "efc2205553006d7bd24124c909dd6c5c";
    public float latitude = 35.6895f;  // Exemple : Tokyo
    public float longitude = 139.6917f;

    // Référence au module principal de Tenkoku
    private TenkokuModule tenkoku;

    void Start()
    {
        // On récupère automatiquement le composant Tenkoku sur ce GameObject
        tenkoku = GetComponent<TenkokuModule>();
        
        if (tenkoku == null)
        {
            Debug.LogError("TenkokuModule introuvable ! Assure-toi que ce script est bien sur le prefab Tenkoku.");
            return;
        }
        
        //tenkoku.enableSoundFX = true;
        //tenkoku.enabled = true;
        
        // Lance la requête au démarrage du jeu
        StartCoroutine(GetWeatherAndSetTime(latitude, longitude));
    }

    IEnumerator GetWeatherAndSetTime(float lat, float lon)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur lors de la requête météo : " + webRequest.error);
                Debug.Log("Lien demamder : " + url);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                WeatherData data = JsonUtility.FromJson<WeatherData>(jsonResponse);

                if (data.weather != null && data.weather.Length > 0)
                {
                    string meteoActuelle = data.weather[0].main;
                    int decalageSecondes = data.timezone;
                    float windSpeed = data.wind.speed;
                    float windDirection = data.wind.deg;
                    
                    Debug.Log($"Météo reçue : {meteoActuelle} | Décalage : {decalageSecondes}s");

                    // 1. APPLIQUER L'HEURE À TENKOKU
                    // On prend l'heure UTC actuelle et on ajoute le décalage local
                    DateTime heureLocale = DateTime.UtcNow.AddSeconds(decalageSecondes);
                    
                    tenkoku.currentYear = heureLocale.Year;
                    tenkoku.currentMonth = heureLocale.Month;
                    tenkoku.currentDay = heureLocale.Day;
                    tenkoku.currentHour = heureLocale.Hour;
                    tenkoku.currentMinute = heureLocale.Minute;
                    tenkoku.currentSecond = heureLocale.Second;

                    // 2. APPLIQUER LA MÉTÉO À TENKOKU
                    AppliquerMeteoEtSonTenkoku(meteoActuelle, windSpeed, windDirection);
                }
            }
        }
    }

    void AppliquerMeteoEtSonTenkoku(string etatMeteo, float windSpeed, float windDirection)
    {
        tenkoku.weather_cloudCirrusAmt = 0f;
        tenkoku.weather_OvercastAmt = 0f;
        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_lightning = 0f;
        tenkoku.weather_SnowAmt = 0f;

        tenkoku.volumeRain = 0f;
        tenkoku.volumeThunder = 0f;
        tenkoku.volumeWind = 0f;
        tenkoku.volumeTurb1 = 0f;
        tenkoku.volumeTurb2 = 0f;
        
        // on modifie les sons de jour et de nuit en de l'heure a 12h day et 0h night
        tenkoku.volumeAmbDay = Mathf.Clamp01(1f - Mathf.Abs(tenkoku.currentHour - 12) / 12f); // Max à 12h, diminue vers 0 à 0h
        tenkoku.volumeAmbNight = Mathf.Clamp01(1f - Mathf.Abs(tenkoku.currentHour - 0) / 12f); // Max à 0h, diminue vers 0 à 12h
        
        // Ajustement du son du vent en fonction de la vitesse
        if (windSpeed > 0)
        {
            tenkoku.volumeWind = Mathf.Clamp(windSpeed / 20f, 0f, 1f); // Supposons que 20 m/s correspond à un vent très
            tenkoku.volumeTurb1 = tenkoku.volumeWind * 0.5f;
            tenkoku.volumeTurb2 = tenkoku.volumeWind * 0.5f;
            
            tenkoku.weather_WindAmt = tenkoku.volumeWind; // On peut aussi lier l'intensité du vent à la météo
            tenkoku.weather_WindDir = windDirection;
        }
        
        //print etat meteo
        Debug.Log(etatMeteo);
        
        switch (etatMeteo)
        {
            case "Clear":
                // Ciel dégagé, on laisse tout à 0
                tenkoku.weather_cloudCirrusAmt = 0.1f;
                break;
            case "Clouds":
                tenkoku.weather_cloudCumulusAmt = 0.4f;
                tenkoku.weather_OvercastAmt = 0.1f;
                break;
            case "Rain":
            case "Drizzle":
                tenkoku.weather_cloudAltoStratusAmt = 0.8f;
                tenkoku.weather_OvercastAmt = 0.8f;
                tenkoku.weather_RainAmt = 1f;
                
                tenkoku.volumeRain = 1f;
                break;
            case "Thunderstorm":
                tenkoku.weather_cloudAltoStratusAmt = 1f;
                tenkoku.weather_OvercastAmt = 1f;
                tenkoku.weather_RainAmt = 1.0f;
                tenkoku.weather_lightning = 1f;
                
                tenkoku.volumeRain = 1f;
                tenkoku.volumeThunder = 1f;
                break;
            case "Snow":
                tenkoku.weather_cloudAltoStratusAmt = 0.8f;
                tenkoku.weather_OvercastAmt = 0.8f;
                tenkoku.weather_SnowAmt = 1f;
                break;
            case "Mist":
            case "Fog":
            case "Haze":
                tenkoku.weather_cloudAltoStratusAmt = 1f;
                break;
            default:
                Debug.Log("État météo non géré spécifiquement : " + etatMeteo);
                break;
        }
    }
}

// --- CLASSES DE DÉSÉRIALISATION JSON ---
[Serializable]
public class WeatherData
{
    public Weather[] weather; 
    public MainData main;     
    public int timezone; 
    public WindData wind;
}

[Serializable]
public class Weather
{
    public int id;
    public string main;       
    public string description;
}

[Serializable]
public class MainData
{
    public float temp;        
    public float humidity;
}

[Serializable]
public class WindData
{
    public float speed;
    public float deg;
}