using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameGUI : MonoBehaviour
{
    [Header("Health UI")]
    public GameObject heartContainer;
    public Sprite heartFullSprite;
    public Sprite heartEmptySprite;
    public int maxHealth = 5;
    private Image[] heartImages;
    
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
        SetupHearts();
        UpdateHUD();
    }
    
    void Update()
    {
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
            rectTransform.sizeDelta = new Vector2(40, 40);
            
            heartImages[i] = heartImage;
        }
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