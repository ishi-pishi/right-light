using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float reach = 4f;
    [SerializeField] private LayerMask hitMask = ~0;

    [SerializeField] private float followSpeed = 15f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 4f;
    private InputAction grabAction;

    Pickupable hovered;
    Pickupable held;
    float holdDistance;

    void Awake()
    {
        grabAction = InputSystem.actions.FindAction("Grab");
    }

    void OnEnable() => grabAction.Enable();

    void OnDisable()
    {
        grabAction.Disable();
        if (held != null) Drop();
        SetHovered(null);
    }

    void Update()
    {
        if (held != null)
        {
            if (grabAction.WasReleasedThisFrame()) Drop();
            return;
        }

        SetHovered(FindTarget());

        if (hovered != null && grabAction.WasPressedThisFrame())
            Grab(hovered);
    }

    void FixedUpdate()
    {
        if (held == null) return;

        Vector3 point = cameraTransform.position + cameraTransform.forward * holdDistance;
        held.Body.linearVelocity = (point - held.Body.position) * followSpeed;
    }

    Pickupable FindTarget()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out RaycastHit hit, reach, hitMask, QueryTriggerInteraction.Ignore))
        {
            return hit.collider.GetComponentInParent<Pickupable>();
        }
        return null;
    }

    void SetHovered(Pickupable next)
    {
        if (next == hovered) return;

        if (hovered != null) hovered.SetHighlighted(false);
        if (next != null) next.SetHighlighted(true);
        hovered = next;
    }

    void Grab(Pickupable target)
    {
        held = target;
        SetHovered(null);

        float d = Vector3.Distance(cameraTransform.position, held.transform.position);
        holdDistance = Mathf.Clamp(d, minDistance, maxDistance);

        held.Body.useGravity = false;
    }

    void Drop()
    {
        held.Body.useGravity = true;
        held = null;
    }
}