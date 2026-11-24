using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float reach = 3f;
    [SerializeField] private LayerMask hitMask = ~0;

    Pickupable lookedAt;
    Pickupable highlighted;

    public Pickupable Target => lookedAt;

    void Awake()
    {
        if (cameraTransform == null && transform.parent != null)
        {
            var cam = transform.parent.GetComponentInChildren<Camera>();
            if (cam != null) cameraTransform = cam.transform;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("InteractionController needs a camera transform assigned.", this);
            enabled = false;
        }
    }

    void Update()
    {
        FindObject();
        SetHighlight();
    }

    void OnDisable()
    {
        if (highlighted != null) highlighted.SetHighlighted(false);
        highlighted = null;
        lookedAt = null;
    }

    void FindObject()
    {
        lookedAt = null;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out RaycastHit hit, reach, hitMask, QueryTriggerInteraction.Ignore))
        {
            lookedAt = hit.collider.GetComponentInParent<Pickupable>();
        }
    }

    void SetHighlight()
    {
        if (lookedAt == highlighted) return;

        if (highlighted != null) highlighted.SetHighlighted(false);
        if (lookedAt != null) lookedAt.SetHighlighted(true);

        highlighted = lookedAt;
    }
}