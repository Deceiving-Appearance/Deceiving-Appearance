using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeCharacter : MonoBehaviour
{

    public SkinnedMeshRenderer skinnedMeshRenderer;
    public MeshCollider meshCollider;
    public float bakeTimer;                         // Ticks for baking the animation for optimization.
    public bool activateBakeMesh;

    private Mesh bakedMesh;
    public bool isBakingReady = false;

    // Start is called before the first frame update
    void Start()
    {
        bakedMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakedMesh);
        meshCollider.sharedMesh = bakedMesh;
        isBakingReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (activateBakeMesh && isBakingReady)
        {
            isBakingReady = false;
            StartCoroutine(UpdateBakeMesh());
        }
    }

    IEnumerator UpdateBakeMesh()
    {
        isBakingReady = false;

        skinnedMeshRenderer.BakeMesh(bakedMesh);

        // If you're constantly rebaking, you should clear the old sharedMesh reference.
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = bakedMesh;
        yield return new WaitForSeconds(bakeTimer);
        isBakingReady = true;
    }

    void OnDestroy()
    {
        if (bakedMesh != null)
        {
            Destroy(bakedMesh);
        }
    }
}
