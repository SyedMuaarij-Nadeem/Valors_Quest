using UnityEngine;
using UnityEngine.SceneManagement;

// =====================================================
//  ATTACH THIS TO: Player object (scene mein)
//
//  INSPECTOR MEIN SIRF ITNA KARO:
//  Hero Prefabs (Size=4) mein charon prefabs drag karo:
//    [0] Player
//    [1] Wizard-blue
//    [2] Wizard-green
//    [3] Wizard-red
//
//  Bus! Script khud sab extract kar legi.
// =====================================================
public class HeroSpawner : MonoBehaviour
{
    [Header("=== 4 PREFABS DRAG ===")]
    [Tooltip("Same order as MainMenu: Player, Wizard-blue, Wizard-green, Wizard-red")]
    public GameObject[] heroPrefabs;  // Size = 4

    void Start()
    {
        // ✅ Last level save karo
        GameProgressManager.SaveLastLevel(SceneManager.GetActiveScene().name);

        // ✅ Selected hero index
        int heroIndex = GameProgressManager.GetSelectedHero();

        // ✅ Validate
        if (heroPrefabs == null || heroPrefabs.Length == 0)
        {
            Debug.LogWarning("[HeroSpawner] Prefabs assign nahi kiye!");
            return;
        }
        if (heroIndex < 0 || heroIndex >= heroPrefabs.Length)
        {
            Debug.LogWarning("[HeroSpawner] Invalid index, using 0");
            heroIndex = 0;
        }

        GameObject selectedPrefab = heroPrefabs[heroIndex];
        if (selectedPrefab == null)
        {
            Debug.LogWarning("[HeroSpawner] Prefab null hai index: " + heroIndex);
            return;
        }

        // ✅ Index 0 = default Player — kuch swap karne ki zaroorat nahi
        if (heroIndex == 0)
        {
            Debug.Log("[HeroSpawner] Default hero — koi swap nahi.");
            return;
        }

        // ✅ Scene wale Player ka SkinnedMeshRenderer dhundo
        SkinnedMeshRenderer sceneSMR = GetComponentInChildren<SkinnedMeshRenderer>();
        if (sceneSMR == null)
        {
            Debug.LogWarning("[HeroSpawner] Scene Player mein SkinnedMeshRenderer nahi mila!");
            return;
        }

        // ✅ Prefab ka SkinnedMeshRenderer dhundo — automatically
        SkinnedMeshRenderer prefabSMR = selectedPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        if (prefabSMR == null)
        {
            Debug.LogWarning("[HeroSpawner] Prefab mein SkinnedMeshRenderer nahi mila!");
            return;
        }

        // ✅ Mesh swap
        sceneSMR.sharedMesh = prefabSMR.sharedMesh;

        // ✅ Materials swap
        sceneSMR.sharedMaterials = prefabSMR.sharedMaterials;

        // ✅ Root bone bhi swap karo — taake animation sahi chale
        // (same hierarchy hai teeno mein, toh yeh safe hai)
        // sceneSMR.rootBone already sahi hai scene Player ka

        // ✅ Animator swap — prefab se Controller aur Avatar lo
        Animator sceneAnimator = GetComponent<Animator>();
        Animator prefabAnimator = selectedPrefab.GetComponent<Animator>();

        if (sceneAnimator != null && prefabAnimator != null)
        {
            if (prefabAnimator.runtimeAnimatorController != null)
                sceneAnimator.runtimeAnimatorController = prefabAnimator.runtimeAnimatorController;

            if (prefabAnimator.avatar != null)
                sceneAnimator.avatar = prefabAnimator.avatar;

            Debug.Log("[HeroSpawner] Animator swapped.");
        }

        Debug.Log("[HeroSpawner] Hero swapped to: " + selectedPrefab.name);
    }
}