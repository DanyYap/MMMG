using UnityEngine;

public interface IAnimatable
{
    void PlayIdleOrRun();
}

public class CharacterAnimator : IAnimatable
{
    private Animator animator;
    private AnimationLibrary animationLibrary;
    private PlayerState playerState;

    public CharacterAnimator(
        Animator animator, 
        AnimationLibrary library, 
        PlayerState playerState = null)
    {
        this.animator = animator;
        this.animationLibrary = library;
        this.playerState = playerState;
    }

    public void InitializePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
    }

    public void PlayIdleOrRun()
    {
        if (playerState == null)
        {
            // Debug.LogError("No player state found.");
            return;
        }

        // Define a target value based on movement state
        float targetMotion = playerState.IsMoving ? 1f : 0f;

        // Set a speed factor for transitioning
        float lerpSpeed = playerState.IsMoving ? 1f : 20f; // Faster when going to idle

        // Gradually change the Motion value
        float currentMotion = animator.GetFloat("Motion");
        float newMotion = Mathf.Lerp(currentMotion, targetMotion, Time.deltaTime * lerpSpeed);

        animator.SetFloat("Motion", newMotion);
    }
}

public class AnimatableFactory
{
    public static IAnimatable CreateAnimator(Animator animator, AnimationLibrary library, EntityType type)
    {
        switch (type)
        {
            case EntityType.Character:
                return new CharacterAnimator(animator, library);

            // Add more cases for other types (e.g., objects)
            default:
                throw new System.ArgumentException("Invalid entity type");
        }
    }
}

public enum EntityType
{
    Character,
    Animal,
    Object // Add more as needed
}
