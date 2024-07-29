using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMeshManager : MonoBehaviour
{
    // 일상적인 애들
    [SerializeField]
    public Mesh[] backpackMeshes;

    [SerializeField]
    public Mesh[] bodyMeshes;

    [SerializeField]
    public Mesh[] eyebrowMeshes;

    [SerializeField]
    public Mesh[] fullbodyMeshes;

    [SerializeField]
    public Mesh[] glassesMeshes;

    [SerializeField]
    public Mesh[] gloveMeshes;

    [SerializeField]
    public Mesh[] hairMeshes;

    [SerializeField]
    public Mesh[] hatMeshes;

    [SerializeField]
    public Mesh[] mustacheMeshes;

    [SerializeField]
    public Mesh[] outerwearMeshes;

    [SerializeField]
    public Mesh[] pantsMeshes;

    [SerializeField]
    public Mesh[] shoeMeshes;

    // 이상한 애들
    [SerializeField]
    public Mesh[] special_backpackMeshes;

    [SerializeField]
    public Mesh[] special_bodyMeshes;

    [SerializeField]
    public Mesh[] special_eyebrowMeshes;

    [SerializeField]
    public Mesh[] special_fullbodyMeshes;

    [SerializeField]
    public Mesh[] special_glassesMeshes;

    [SerializeField]
    public Mesh[] special_gloveMeshes;

    [SerializeField]
    public Mesh[] special_hairMeshes;

    [SerializeField]
    public Mesh[] special_hatMeshes;

    [SerializeField]
    public Mesh[] special_mustacheMeshes;

    [SerializeField]
    public Mesh[] special_outerwearMeshes;

    [SerializeField]
    public Mesh[] special_pantsMeshes;

    [SerializeField]
    public Mesh[] special_shoeMeshes;


    public Mesh loadBackpackMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if(rand == 0) return backpackMeshes[index];
            else return special_backpackMeshes[index];
        }
        else return backpackMeshes[index];
    }

    public Mesh loadBodyMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return bodyMeshes[index];
            else return special_bodyMeshes[index];
        }
        else return bodyMeshes[index];
    }

    public Mesh loadEyebrowMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return eyebrowMeshes[index];
            else return special_eyebrowMeshes[index];
        }
        else return eyebrowMeshes[index];
    }

    public Mesh loadFullbodyMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return fullbodyMeshes[index];
            else return special_fullbodyMeshes[index];
        }
        else return fullbodyMeshes[index];
    }

    public Mesh loadGlassesMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return glassesMeshes[index];
            else return special_glassesMeshes[index];
        }
        else return glassesMeshes[index];
    }

    public Mesh loadGloveMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return gloveMeshes[index];
            else return special_gloveMeshes[index];
        }
        else return gloveMeshes[index];
    }

    public Mesh loadHairMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return hairMeshes[index];
            else return special_hairMeshes[index];
        }
        else return hairMeshes[index];
    }

    public Mesh loadHatMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return hatMeshes[index];
            else return special_hatMeshes[index];
        }
        else return hatMeshes[index];
    }

    public Mesh loadMustacheMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return mustacheMeshes[index];
            else return special_mustacheMeshes[index];
        }
        else return mustacheMeshes[index];
    }

    public Mesh loadOuterwearMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return outerwearMeshes[index];
            else return special_outerwearMeshes[index];
        }
        else return outerwearMeshes[index];
    }

    public Mesh loadPantsMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return pantsMeshes[index];
            else return special_pantsMeshes[index];
        }
        else return pantsMeshes[index];
    }

    public Mesh loadShoeMesh(bool isSpecial, int index)
    {
        if (isSpecial)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return shoeMeshes[index];
            else return special_shoeMeshes[index];
        }
        else return shoeMeshes[index];
    }

}
