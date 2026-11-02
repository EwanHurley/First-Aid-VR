using UnityEngine;

public class DentDriver : MonoBehaviour
{
    [SerializeField] Renderer rend;
    [SerializeField] Transform pressTip;
    [SerializeField] Transform pressSurface;   // up = surface normal
    [SerializeField] float maxDepth = 0.05f;
    [SerializeField] float radius = 0.12f;
    [SerializeField] float tipRadius = 0.02f;

    MaterialPropertyBlock mpb;
    float depth, vel;

    void Awake() => mpb = new MaterialPropertyBlock();

    void Update()
    {
        Vector3 local = pressSurface.InverseTransformPoint(pressTip.position);
        float penetration = Mathf.Max(0f, tipRadius - local.y);
        float target = Mathf.Min(penetration, maxDepth);
        depth = Mathf.SmoothDamp(depth, target, ref vel, 0.05f);

        rend.GetPropertyBlock(mpb);
        mpb.SetVector("_PressPos", pressTip.position);
        mpb.SetVector("_PressDir", -pressSurface.up);
        mpb.SetFloat("_PressRadius", radius);
        mpb.SetFloat("_PressDepth", depth);
        rend.SetPropertyBlock(mpb);
    }
}