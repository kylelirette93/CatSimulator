using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClimbableSurface : Interactable, IClimbable
{
    [SerializeField] private Transform _climbSpot;

    public Vector3 ClimbSurfacePosition => _climbSpot.position;

    public override void Interact()
    {
        
    }

    public Vector3 GetClimbEntryPosition(Vector3 climberPosition)
    {
        if (!TryGetComponent<BoxCollider2D>(out BoxCollider2D collider))
        {
            return transform.position;
        }
        Vector3 closestPoint = collider.bounds.ClosestPoint(climberPosition);
        const float entryNudge = 0.15f;
        Vector3 directionToClosestPoint = (closestPoint - collider.bounds.center).normalized;
        return closestPoint - (directionToClosestPoint * entryNudge);
    }

    public void Climb(GameObject climber)
    {          
        SpriteRenderer climberRenderer = climber.GetComponent<SpriteRenderer>();
        if (climberRenderer != null)
        {
            climberRenderer.sortingOrder = 10; 
        }
    }
}

public interface IClimbMovement 
{
    bool IsClimbing { get; }
    void ClimbUpSurface(Collider2D surfaceBounds, Vector3 entryPosition);

    void ClimbDownSurface();
}

