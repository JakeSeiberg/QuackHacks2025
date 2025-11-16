using UnityEngine;
public class PlayerAnimation : MonoBehaviour
{
    [Header("Idle Animation Sprites")]
    public Sprite[] downIdleSprites;   // Idle facing down
    public Sprite[] leftIdleSprites;   // Idle facing left
    public Sprite[] rightIdleSprites;  // Idle facing right
    public Sprite[] upIdleSprites;     // Idle facing up
    
    [Header("Walk Animation Sprites")]
    public Sprite[] downWalkSprites;   // Walking down animation
    public Sprite[] leftWalkSprites;   // Walking left animation
    public Sprite[] rightWalkSprites;  // Walking right animation
    public Sprite[] upWalkSprites;     // Walking up animation
    
    [Header("Animation Settings")]
    public float walkFrameRate = 10f; // Frames per second for walking
    public float idleFrameRate = 15f;  // Frames per second for idle (usually slower)
    public bool playOnStart = true;
    public bool loop = true;
    
    [Header("Current State")]
    public AnimationDirection currentDirection = AnimationDirection.Down;
    public AnimationState currentState = AnimationState.Idle;
    
    private SpriteRenderer spriteRenderer;
    private float timer;
    private int currentFrame;
    
    public enum AnimationDirection
    {
        Down,
        Left,
        Right,
        Up
    }
    
    public enum AnimationState
    {
        Idle,
        Walking
    }
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        if (playOnStart)
        {
            currentFrame = 0;
            UpdateSprite();
        }
    }
    
    void Update()
    {
        Sprite[] currentSprites = GetCurrentSprites();
        
        if (currentSprites == null || currentSprites.Length == 0) return;
        
        // Use appropriate frame rate based on state
        float frameRate = currentState == AnimationState.Walking ? walkFrameRate : idleFrameRate;
        
        timer += Time.deltaTime;
        
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame++;
            
            if (currentFrame >= currentSprites.Length)
            {
                if (loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame = currentSprites.Length - 1;
                }
            }
            
            UpdateSprite();
        }
    }
    
    Sprite[] GetCurrentSprites()
    {
        if (currentState == AnimationState.Idle)
        {
            switch (currentDirection)
            {
                case AnimationDirection.Down:
                    return downIdleSprites.Length > 0 ? downIdleSprites : new Sprite[] { downWalkSprites[0] };
                case AnimationDirection.Left:
                    return leftIdleSprites.Length > 0 ? leftIdleSprites : new Sprite[] { leftWalkSprites[0] };
                case AnimationDirection.Right:
                    return rightIdleSprites.Length > 0 ? rightIdleSprites : new Sprite[] { rightWalkSprites[0] };
                case AnimationDirection.Up:
                    return upIdleSprites.Length > 0 ? upIdleSprites : new Sprite[] { upWalkSprites[0] };
            }
        }
        else // Walking
        {
            switch (currentDirection)
            {
                case AnimationDirection.Down:
                    return downWalkSprites;
                case AnimationDirection.Left:
                    return leftWalkSprites;
                case AnimationDirection.Right:
                    return rightWalkSprites;
                case AnimationDirection.Up:
                    return upWalkSprites;
            }
        }
        
        return downIdleSprites.Length > 0 ? downIdleSprites : downWalkSprites;
    }
    
    void UpdateSprite()
    {
        if (spriteRenderer == null) return;
        
        Sprite[] currentSprites = GetCurrentSprites();
        
        if (currentSprites != null && currentFrame >= 0 && currentFrame < currentSprites.Length)
        {
            spriteRenderer.sprite = currentSprites[currentFrame];
        }
    }
    
    public void SetDirection(AnimationDirection direction)
    {
        if (currentDirection != direction)
        {
            currentDirection = direction;
            currentFrame = 0;
            timer = 0f;
            UpdateSprite();
        }
    }
    
    public void SetState(AnimationState state)
    {
        if (currentState != state)
        {
            currentState = state;
            currentFrame = 0;
            timer = 0f;
            UpdateSprite();
        }
    }
    
    public void SetDirectionAndState(AnimationDirection direction, AnimationState state)
    {
        bool changed = false;
        
        if (currentDirection != direction)
        {
            currentDirection = direction;
            changed = true;
        }
        
        if (currentState != state)
        {
            currentState = state;
            changed = true;
        }
        
        if (changed)
        {
            currentFrame = 0;
            timer = 0f;
            UpdateSprite();
        }
    }
    
    public void PlayAnimation(AnimationDirection direction, AnimationState state)
    {
        SetDirectionAndState(direction, state);
    }
    
    public void StopAnimation()
    {
        SetState(AnimationState.Idle);
        currentFrame = 0;
        UpdateSprite();
    }
    
    public void ResetAnimation()
    {
        currentFrame = 0;
        timer = 0f;
        UpdateSprite();
    }
}