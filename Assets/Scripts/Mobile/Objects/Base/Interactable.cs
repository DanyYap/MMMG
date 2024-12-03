public interface IInteractable
{
    void Interact(); // Method to be called on interaction
}

// This interface defines methods for anything that can catch fire
public interface IFireable : IInteractable
{
    void Ignite(); // Method to start fire
    void Extinguish(); // Method to extinguish fire
}
