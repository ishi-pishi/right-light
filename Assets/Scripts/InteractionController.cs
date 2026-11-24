using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float reach = 3f;
    [SerializeField] private LayerMask hitMask = ~0;

    Pickupable current;

    void Update()
    {
        if (current == null) current = null;

        Pickupable target = null;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
                out RaycastHit hit, reach, hitMask, QueryTriggerInteraction.Ignore))
        {
            target = hit.collider.GetComponentInParent<Pickupable>();
        }

        if (target == current) return;

        if (current != null) current.SetHighlighted(false);
        if (target != null) target.SetHighlighted(true);
        current = target;
    }
}