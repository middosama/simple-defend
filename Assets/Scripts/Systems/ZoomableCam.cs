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
    float minX, maxX, minY, maxY;

    [HideInInspector]
    public float ratio;
    [HideInInspector]
    public Camera camera;
    public float currentSize;


    public static ZoomableCam Instance;
    float yOffset;


    Tweener zooming;

    public Vector2 position
    {
        get => new Vector2(camera.transform.position.x, camera.transform.position.y - yOffset);
    }
    
    private void Awake()
    {
        camera = GetComponent<Camera>();
        yOffset = camera.transform.position.y;
        Instance = this;
        ratio = (float)Screen.width / Screen.height;
        currentSize = camera.orthographicSize;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Focus(Vector2.zero,-0.5f);
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            Focus(Vector2.zero, -0.5f,6);
        }
    }

    public void ClampedSmoothZoom(float size)
    {
        size = Mathf.Clamp(size, minSize, maxSize);
        
        
        if(size!= currentSize)
        {
            SmoothZoom(size);
        }
    }

    void SmoothZoom(float size)
    {
        zooming.Kill();
        currentSize = size;
        zooming = camera.DOOrthoSize(currentSize, smoothness);
    }

    public void MoveTo(Vector2 destination, float smoothness)
    {
        camera.transform.DOLocalMove(new Vector3(Mathf.Clamp(destination.x,minX,maxX), Mathf.Clamp(destination.y,minY,maxY) + yOffset,camera.transform.position.z), smoothness);
    }

    public void Focus(Vector2 destination,float xOffset, float size = 0)
    {
        size = Mathf.Clamp(size, minSize, maxSize);
        destination.x += size * ratio * xOffset;
        MoveTo(destination,smoothness);
        SmoothZoom(size);

    }
    
}
