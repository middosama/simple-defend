using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ZoomableCam : MonoBehaviour
{
    //make cam lookat center in prefab first

    [SerializeField]
    float smoothness;
    [SerializeField]
    float minSize = 2, maxSize = 6;
    [SerializeField]
    float questioningRatio = 1.65f; // wut is this :)) ?


    public static ZoomableCam Instance;
    float yOffset;
    Camera camera;
    
    private void Awake()
    {
        camera = GetComponent<Camera>();
        yOffset = camera.transform.position.y;
        Instance = this;
        
    }
    private void Update()
    {
        Debug.Log(camera.ScreenToWorldPoint(Vector3.zero,Camera.MonoOrStereoscopicEye.Mono));
    }

    public void SmoothZoom(float size)
    {
        camera.DOOrthoSize(Mathf.Clamp(size,minSize,maxSize), smoothness);
    }

    public void MoveTo(Vector2 destination)
    {
        camera.transform.DOLocalMove(new Vector2(destination.x, destination.y + yOffset), smoothness);
    }

    public void Focus(Vector2 destination, Vector2 anchorPos)
    {
        anchorPos *= questioningRatio * minSize;
        SmoothZoom(minSize);
        MoveTo(destination + anchorPos);
    }
}
