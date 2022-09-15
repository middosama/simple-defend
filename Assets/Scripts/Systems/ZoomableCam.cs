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
    [SerializeField]
    GameObject map;
    [HideInInspector]
    public float ratio;
    [HideInInspector]
    public Camera camera;
    public float _currentSize;
    public float CurrentSize
    {
        get => _currentSize;
        set
        {
            _currentSize = value;
            _currentYSpace = _currentSize / Mathf.Cos(degree45);
        }
    }
    


    public static ZoomableCam Instance;
    float yOffset;


    Tweener zooming;
    const float degree45 = Mathf.PI / 4;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        yOffset = camera.transform.position.y;
        Instance = this;
        ratio = (float)Screen.width / Screen.height;
        CurrentSize = camera.orthographicSize;

        var bound = map.gameObject.GetComponent<SpriteRenderer>().bounds;
        minY = bound.min.y;
        maxY = bound.max.y;
        minX = bound.min.x;
        maxX = bound.max.x;
        // 
        //        minX = map.gameObject.GetComponent<Renderer>().bounds.min.x;
        //minY = map.gameObject.GetComponent<Renderer>().bounds.min.y;
        //maxX = map.gameObject.GetComponent<Renderer>().bounds.max.x;
        //maxY = map.gameObject.GetComponent<Renderer>().bounds.max.y;
        //Debug.Log(map.gameObject.GetComponent<Renderer>().bounds.min);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Focus(Vector2.zero, -0.5f);
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            Focus(Vector2.zero, -0.5f, 6);
        }
    }

    public void ClampedSmoothZoom(float size)
    {
        size = Mathf.Clamp(size, minSize, maxSize);


        if (size != CurrentSize)
        {
            SmoothZoom(size);
        }
    }

    void SmoothZoom(float size)
    {
        zooming.Kill();
        CurrentSize = size;
        zooming = camera.DOOrthoSize(CurrentSize, smoothness);
    }

    public void MoveTo(Vector2 destination, float smoothness)
    {
        float ySpace = CurrentYSpace;
        float botCamY = -camera.transform.position.z - ySpace;
        float topCamY = botCamY + ySpace * 2;
        float xSpace = CurrentSize * ratio;
        float rightCamX = camera.transform.position.x + xSpace;
        float leftCamX = camera.transform.position.x - xSpace;
        //
        /*     Debug.Log((maxY - topCamY) + " : " + (minY - botCamY) + " : " + (xSpace));*/
        camera.transform.DOLocalMove(new Vector3(Mathf.Clamp(destination.x, minX - leftCamX, maxX - rightCamX), Mathf.Clamp(destination.y, minY - botCamY, maxY - topCamY), camera.transform.position.z), smoothness);
    }

    public void Focus(Vector2 destination, float xOffset, float size = 0)
    {
        size = Mathf.Clamp(size, minSize, maxSize);
        SmoothZoom(size);
        destination.x += size * ratio * xOffset;
        destination.y -= CurrentYSpace *2 ;
        MoveTo(destination, smoothness);
        Debug.Log(destination);

    }
    

    public float _currentYSpace;
    public float CurrentYSpace => _currentYSpace;

}
