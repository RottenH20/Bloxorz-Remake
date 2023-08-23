using UnityEngine;
using System;

public class HueChange : MonoBehaviour
{
    [SerializeField]
    int[] indexToSwitch = null; // Set to null to hide warning in Unity

    [SerializeField]
    Material[] newMaterials = null;

    Material[] firstMaterials = null;

    bool Swapped = false;

    private void Start()
    {
        firstMaterials = GetComponent<Renderer>().materials;
    }

    private Material GetMeshMaterialAtIndex(int index)
    {
        return GetComponent<Renderer>().materials[index];
    }

    public void SwitchMaterial()
    {
        this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play(true);

        if (!Swapped)
            for (int i = 0; i < indexToSwitch.Length; i++)
            {
                Material currentMaterial = GetMeshMaterialAtIndex(indexToSwitch[i]);

                Material[] materials = GetComponent<Renderer>().materials;
                materials[indexToSwitch[i]] = newMaterials[i];
                GetComponent<Renderer>().materials = materials;
                Swapped = true;
            }
        else
        {
            GetComponent<Renderer>().materials = firstMaterials;
            Swapped = false;
        }
    }
}