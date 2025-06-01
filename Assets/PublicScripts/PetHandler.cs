using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PetHandler : MonoBehaviour
{
    [TextArea] public string comment = "더 이상 손 댈 것이 없으므로 닫아두세요.";
    [HideInInspector] public PetAnimator petAnimator;
    [HideInInspector] public PetMovement petMovement;
    [HideInInspector] public PetDragAndDrop petDragAndDrop;

    void Awake()
    {
        SetHandler(ref petAnimator);
        SetHandler(ref petMovement);
        SetHandler(ref petDragAndDrop);
    }

    /// <summary>
    /// IHandler 인터페이스를 상속하는 오브젝트에 대해
    /// WinionHandler를 할당하는 함수입니다.
    /// </summary>
    /// <param name="scriptType"> 핸들러를 부여할 오브젝트 </param>
    public void SetHandler<T>(ref T scriptType) where T : IHandler
    {
        if (scriptType == null)
        {
            scriptType = GetComponent<T>();
        }

        if (scriptType == null)
        {
            Debug.LogError($"{gameObject.name}에 스크립트를 달아야합니다.");
        }

        scriptType.petHandler = this;
    }

}
