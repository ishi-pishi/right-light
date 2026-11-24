using System.Collections.Generic;
using UnityEngine;

public class EaselScorer : MonoBehaviour
{
    [Tooltip("The camera that renders onto the easel plane, not the player camera.")]
    [SerializeField] private Camera easelCamera;

    Dictionary<string, ViewState> reference;

    public Dictionary<string, ViewState> Capture()
    {
        var result = new Dictionary<string, ViewState>();
        var planes = GeometryUtility.CalculateFrustumPlanes(easelCamera);

        foreach (var p in FindObjectsByType<Pickupable>(FindObjectsSortMode.None))
        {
            Bounds b = GetBounds(p);
            if (!GeometryUtility.TestPlanesAABB(planes, b)) continue; // not in frame

            var state = ProjectBounds(b);
            state.rotation = p.transform.rotation;

            if (!result.TryAdd(p.name, state))
                Debug.LogWarning($"Duplicate pickup name '{p.name}'. Names must be unique for now.", p);
        }
        return result;
    }

    static Bounds GetBounds(Component c)
    {
        var renderers = c.GetComponentsInChildren<Renderer>();
        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);
        return b;
    }

    ViewState ProjectBounds(Bounds b)
    {
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        for (int i = 0; i < 8; i++)
        {
            Vector3 sign = new Vector3((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -1 : 1, (i & 4) == 0 ? -1 : 1);
            Vector3 corner = b.center + Vector3.Scale(b.extents, sign);
            Vector2 v = easelCamera.WorldToViewportPoint(corner);
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }

        return new ViewState { center = (min + max) / 2f, size = max - min };
    }

    void RecordReference()
    {
        reference = Capture();
        Debug.Log($"Recorded {reference.Count} objects as the reference.");
    }

    void LogCapture()
    {
        foreach (var kv in Capture())
            Debug.Log($"{kv.Key}: center {kv.Value.center}, size {kv.Value.size}");
    }
}

// <summary>
// Represents the data for one Pickupable object in the screen view
// </summary>
public struct ViewState
{
    public Vector2 center;      // viewport coordinates, 0 to 1
    public Vector2 size;        // width and height of the object on-screen rectangle, 0 to 1
}