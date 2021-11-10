using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This attribute makes the asset show up in the editor's Assets -> Create menu
// and when right-clicking in your Project window and choosing Create
[CreateAssetMenu(fileName = "Material Library", menuName = "Custom/Material Library")]
public class MaterialLibrary : ScriptableObject
{
    public Material[] materials;

}
