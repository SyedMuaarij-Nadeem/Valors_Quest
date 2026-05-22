using UnityEngine;
using UnityEngine.UI;

// =====================================================
//  ButtonSound.cs
//
//  ATTACH TO: Seedha Button pe — jis pe bhi sound chahiye
//
//  INSPECTOR MEIN:
//  Click Sound → apni sound file drag karo
//
//  FAIDA:
//  - Har button ka apna alag sound ho sakta hai
//  - Ya same sound — jo marzi
//  - Bilkul simple — drag and done!
// =====================================================
[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [Header("=== SOUND ===")]
    public AudioClip clickSound;  // Yahan apni sound drag karo

    private Button btn;
    private AudioSource audioSource;

    void Awake()
    {
        btn = GetComponent<Button>();

        // AudioSource Canvas pe dhundo ya naya banao
        audioSource = FindAnyObjectByType<AudioSource>();
        if (audioSource == null)
        {
            // Naya AudioSource is GameObject pe banao
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Start()
    {
        if (btn != null)
            btn.onClick.AddListener(PlayClick);
    }

    void PlayClick()
    {
        if (clickSound == null || audioSource == null) return;
        audioSource.PlayOneShot(clickSound);
    }
}
