using System.Linq;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;

    Renderer rend;
    Material[] normalMats;
    Material[] outlinedMats;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        normalMats = rend.sharedMaterials;
        outlinedMats = normalMats.Append(outlineMaterial).ToArray();
    }

    public void SetHighlighted(bool highlighted)
    {
        rend.sharedMaterials = highlighted ? outlinedMats : normalMats;
    }
}