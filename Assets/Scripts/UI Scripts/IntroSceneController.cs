using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class IntroSceneController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;
    public GameObject continuePrompt; // Optional: Shows "Press SPACE to continue"
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip narrationClip;
    
    [Header("Scene Settings")]
    public string tutorialSceneName = "TutorialScene";
    
    private string firstText = "You're Sheriff Joe, the honorable sheriff of ye olde western town in the year 1880. As you're patrolling near the oil fields you find a strange looking body lying dead in the desert...";
    
    private string secondText = "If you've pried this note from my cold fingers, it means that VersiCorp found me before I had a chance to complete my mission. My message is one from the future: 2143. Our oil has all run out, leaving corporations to rely on the past—your present—for resources. They've come here, to your time, to stake their claim and drain the old world of its materials. You must stop them. Take my Time Calibrator and put an end to their greed.";
    
    private bool waitingForInput = false;
    private bool audioPlaying = false;
    
    void Start()
    {
        // Show first text
        dialogueText.text = firstText;
        waitingForInput = true;
        
        if (continuePrompt != null)
            continuePrompt.SetActive(true);
    }
    
    void Update()
    {
        // Wait for spacebar press to continue to second text
        if (waitingForInput && Input.GetKeyDown(KeyCode.Space))
        {
            waitingForInput = false;
            if (continuePrompt != null)
                continuePrompt.SetActive(false);
            
            ShowSecondTextAndPlayAudio();
        }
        
        // Check if audio has finished playing
        if (audioPlaying && !audioSource.isPlaying)
        {
            audioPlaying = false;
            LoadTutorialScene();
        }
    }
    
    void ShowSecondTextAndPlayAudio()
    {
        // Display second text
        dialogueText.text = secondText;
        
        // Play narration audio
        if (audioSource != null && narrationClip != null)
        {
            audioSource.clip = narrationClip;
            audioSource.Play();
            audioPlaying = true;
        }
        else
        {
            // If no audio, wait a few seconds then load tutorial
            StartCoroutine(WaitAndLoadTutorial(5f));
        }
    }
    
    IEnumerator WaitAndLoadTutorial(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadTutorialScene();
    }
    
    void LoadTutorialScene()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }
}