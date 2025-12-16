using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt = "Press 'E' to interact";
    public virtual void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
    }
}

public interface IClimbable : IInteractable
{
    Vector3 GetClimbEntryPosition(Vector3 climberPosition);
    void Climb(GameObject climber);

    Vector3 ClimbSurfacePosition { get; }
}
public interface IInteractable
{
    void Interact();
}
