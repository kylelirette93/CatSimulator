using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    Interactable interactable;
    private IClimbMovement _playerClimbMovement;

    #region Input Handling
    private void Awake()
    {;
        TryGetComponent<IClimbMovement>(out _playerClimbMovement);

        if (_playerClimbMovement == null)
        {
            Debug.LogError("PlayerController requires a component that implements IClimbMovement.");
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            IClimbable climbable = other.GetComponent<IClimbable>();
            if (climbable != null)
            {
                Vector3 entryPosition = climbable.GetClimbEntryPosition(transform.position);
                _playerClimbMovement.ClimbUpSurface(other.GetComponent<Collider2D>(), entryPosition);
                climbable.Climb(this.gameObject);
            }
            interactable = other.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
            }

        }
    }
}
