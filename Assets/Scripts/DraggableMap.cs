using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityCommon;

public class DraggableMap : MonoBehaviour, IBeginDragHandler, IDragHandler
{

    public float dragSmoothness = 0.1f;


    Vector2 nextPos;
    ZoomableCam camera;
    Collider2D collider;
    public float screenH = 1080;
    float camSize;
    float originFingerDistance;
    float doubleCamSize;
    float screenWidthDpi;
    float screenHeightDpi;



    private void Start()
    {
        camera = ZoomableCam.Instance;
        collider = GetComponent<Collider2D>();
        screenWidthDpi = camera.ratio / Screen.width;
        screenHeightDpi = Screen.height * Mathf.Sin(Mathf.Deg2Rad * (90 + camera.transform.localRotation.eulerAngles.x));
    }
    private void Update()
    {
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                return;
            }
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            { // start touch
                camSize = camera.camera.orthographicSize;
                originFingerDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                camera.ClampedSmoothZoom(camSize * originFingerDistance / (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude);

            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            if (!LevelGUI.Instance.canvas.IsRaycastBlocking(Input.mousePosition, camera.camera))
            {
                camera.ClampedSmoothZoom(camera.CurrentSize - Input.mouseScrollDelta.y / 1.2f);
            }
            
        }
        
    }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        nextPos = camera.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log(camera.position);
        doubleCamSize = camera.camera.orthographicSize * 2;
        nextPos = new Vector2(nextPos.x - eventData.delta.x * screenWidthDpi * doubleCamSize, nextPos.y - (eventData.delta.y / screenHeightDpi) * doubleCamSize);
        camera.MoveTo(nextPos, dragSmoothness);
    }
}
