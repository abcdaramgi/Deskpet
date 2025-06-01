using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public bool isPickUp = false;
    public float PickUpTimer = 0f;
    public Vector3 mousePosition;

    public int ClickCount = 0;
    public double lastClickTime = 0;

    public Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    
    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag()
    {
        PickUpTimer += Time.deltaTime;
        
        if(PickUpTimer >= 0.15f)
        {
            isPickUp = true;
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        }
    }

    public virtual void MouseClickEvent()
    {

    }

    private void OnMouseUp()
    {
        // 드래그하지않고 클릭만 했을 경우
        if(!isPickUp)
        {
            MouseClickEvent();
        }

        isPickUp = false;
        PickUpTimer = 0;
    }
}
