using UnityEngine;

// =====================================================
//  KISI OBJECT PAR LAGANE KI ZAROORAT NAHI
//  Sirf Script folder mein rakho
//  MainMenuManager + HeroSpawner dono use karte hain
// =====================================================
public class GameProgressManager
{
    private const string LAST_LEVEL_KEY = "LastPlayedLevel";
    private const string LAST_HERO_KEY  = "LastSelectedHero";
    private const string DEFAULT_LEVEL  = "Level1_Prison";

    // ── LEVEL ──────────────────────────────────────
    public static void SaveLastLevel(string sceneName)
    {
        PlayerPrefs.SetString(LAST_LEVEL_KEY, sceneName);
        PlayerPrefs.Save();
        Debug.Log("[Progress] Level saved: " + sceneName);
    }

    public static string GetLastLevel()
    {
        return PlayerPrefs.GetString(LAST_LEVEL_KEY, DEFAULT_LEVEL);
    }

    // ── HERO ───────────────────────────────────────
    public static void SaveSelectedHero(int index)
    {
        PlayerPrefs.SetInt(LAST_HERO_KEY, index);
        PlayerPrefs.Save();
        Debug.Log("[Progress] Hero saved: index " + index);
    }

    public static int GetSelectedHero()
    {
        return PlayerPrefs.GetInt(LAST_HERO_KEY, 0); // 0 = Player (default)
    }
}
