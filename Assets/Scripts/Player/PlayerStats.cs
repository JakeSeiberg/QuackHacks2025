using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    
    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth = 5;
    
    [Header("Weapon")]
    public int currentWeaponIndex = 0;
    public int currentAmmo = 6;
    public int maxAmmo = 6;
    
    [Header("Currency")]
    public int currency = 0;

    [Header("Position")]
    public bool inFuture = false;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    } 
    
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }
    
    public void UseAmmo(int amount = 1)
    {
        currentAmmo = Mathf.Max(0, currentAmmo - amount);
    }
    
    public void Reload()
    {
        currentAmmo = maxAmmo;
    }
    
    public void AddCurrency(int amount)
    {
        currency += amount;
    }
    
    public void SpendCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
        }
    }
    
    public void SwitchWeapon(int weaponIndex)
    {
        currentWeaponIndex = weaponIndex;
        Reload();
    }
    
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    
    public void DecreaseMaxHealth(int amount)
    {
        maxHealth = Mathf.Max(1, maxHealth - amount);
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
    
    public void Die()
    {
        Debug.Log("Player died!");
        // Add death logic here
    }
    
    void Update()
    {
        // Test controls
        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage(1);
        if (Input.GetKeyDown(KeyCode.J))
            Heal(1);
        if (Input.GetKeyDown(KeyCode.K))
            UseAmmo(1);
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
        if (Input.GetKeyDown(KeyCode.C))
            AddCurrency(10);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            IncreaseMaxHealth(1);
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            DecreaseMaxHealth(1);
    }
}