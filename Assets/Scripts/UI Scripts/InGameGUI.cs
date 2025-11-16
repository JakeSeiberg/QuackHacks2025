using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Health UI")]
    public GameObject heartContainer;
    public Sprite heartFullSprite;
    public Sprite heartEmptySprite;
    public int maxHealth = 5;
    public float heartSize = 60f; // Adjustable heart size
    private Image[] heartImages;
    private int previousMaxHealth;
    
    [Header("Weapon UI")]
    public Image weaponIcon;
    public Sprite[] weaponSprites; // Array of different gun sprites
    public TextMeshProUGUI ammoText;
    
    [Header("Currency UI")]
    public TextMeshProUGUI currencyText;
    public string currencySymbol = "$";
    
    [Header("Player Reference")]
    public PlayerStats playerStats;
    
    void Start()
    {
        previousMaxHealth = maxHealth;
        SetupHearts();
        UpdateHUD();
    }
    
    void Update()
    {
        // Check if max health changed and recreate hearts if needed
        if (playerStats != null && playerStats.maxHealth != previousMaxHealth)
        {
            maxHealth = playerStats.maxHealth;
            previousMaxHealth = maxHealth;
            ClearHearts();
            SetupHearts();
        }
        
        UpdateHUD();
    }
    
    void SetupHearts()
    {
        if (heartContainer == null) return;
        
        // Create heart images based on max health
        heartImages = new Image[maxHealth];
        
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = new GameObject($"Heart_{i}");
            heart.transform.SetParent(heartContainer.transform, false);
            
            Image heartImage = heart.AddComponent<Image>();
            heartImage.sprite = heartFullSprite;
            heartImage.preserveAspect = true;
            
            RectTransform rectTransform = heart.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(heartSize, heartSize);
            
            heartImages[i] = heartImage;
        }
    }
    
    void ClearHearts()
    {
        if (heartContainer == null) return;
        
        // Destroy all existing heart GameObjects
        foreach (Transform child in heartContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        heartImages = null;
    }
    
    void UpdateHUD()
    {
        if (playerStats == null) return;
        
        UpdateHealthDisplay();
        UpdateWeaponDisplay();
        UpdateCurrencyDisplay();
    }
    
    void UpdateHealthDisplay()
    {
        if (heartImages == null || heartImages.Length == 0) return;
        
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < playerStats.currentHealth)
            {
                heartImages[i].sprite = heartFullSprite;
                heartImages[i].color = Color.white;
            }
            else
            {
                heartImages[i].sprite = heartEmptySprite;
                heartImages[i].color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }
    
    void UpdateWeaponDisplay()
    {
        // Update weapon icon
        if (weaponIcon != null && weaponSprites.Length > 0)
        {
            int weaponIndex = Mathf.Clamp(playerStats.currentWeaponIndex, 0, weaponSprites.Length - 1);
            weaponIcon.sprite = weaponSprites[weaponIndex];
        }
        
        // Update ammo text
        if (ammoText != null)
        {
            ammoText.text = $"{playerStats.currentAmmo}/{playerStats.maxAmmo}";
        }
    }
    
    void UpdateCurrencyDisplay()
    {
        if (currencyText != null)
        {
            currencyText.text = $"{currencySymbol}{playerStats.currency}";
        }
    }
}