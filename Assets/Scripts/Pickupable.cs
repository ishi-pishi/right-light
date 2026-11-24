using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;

    Renderer rend;
    Material[] normalMats;
    Material[] outlinedMats;

    public Rigidbody Body { get; private set; }

    void Awake()
    {
        Body = GetComponent<Rigidbody>();

        rend = GetComponentInChildren<Renderer>();
        normalMats = rend.sharedMaterials;
        outlinedMats = normalMats.Append(outlineMaterial).ToArray();

        Body.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void SetHighlighted(bool highlighted)
    {
        rend.sharedMaterials = highlighted ? outlinedMats : normalMats;
    }
}