using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWithDragAndDrop : DragAndDrop
{
    public CustomMovement customMovement;
    public CustomAnimator customAnimator;

    public override void MouseClickEvent()
    {
        // 
        customMovement.canWalk = false;
        customMovement.isArrive = false;

        double currentClickTime = Service.GetCurrentTime();
        
        // 최초 클릭 이거나 마지막 클릭으로부터 60초를 지났을 때
        if(lastClickTime == 0 || (currentClickTime - lastClickTime) > 10)
        {
            lastClickTime = currentClickTime;
            ClickCount = 0;
            Debug.Log("마지막 클릭 갱신");
        }
    }
}
