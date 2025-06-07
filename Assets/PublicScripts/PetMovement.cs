using UnityEngine;

public class PetMovement : MonoBehaviour, IHandler
{
    public PetHandler petHandler { get; set; }
    public CharacterState currentCharacterState = CharacterState.Idle;
    public CharacterState lastCharacterState = CharacterState.None;
    public bool changeNextState = false;
    public bool canWalk = true;
    public bool isArrive = false;
    public bool isPickUp = false;
    public bool struggling = false;
    public float arriveTime = 0f;
    public float waitTime = 0f;
    public float moveSpeed = 1f;
    public Vector3 targetPosition;

    public bool isRecover = false;
    public float recoverTime = 0;

    private void Start() {
        canWalk = true;
    }

    private void Update() {

        if(isRecover)
        {
            recoverTime += Time.deltaTime;

            if(recoverTime > 1f)
            {
                isRecover = false;
                canWalk = false;
                isArrive = true;
                arriveTime = 0;
                waitTime = 1.5f;

                petHandler.petAnimator.PlayAnimation("Idle");
            }
        }

        if(petHandler.petDragAndDrop.isPickUp)
        {
            if((int)(petHandler.petDragAndDrop.PickUpTimer * 4) % 2 == 0)
            {
                struggling = true;
            }
            else
            {
                struggling = false;
            }

            petHandler.petAnimator.FlipSprite(struggling);

            if(!isPickUp)
            {
                isPickUp = true;
                petHandler.petAnimator.PlayAnimation("PickUp");
            }

            return;
        }
        else if(isPickUp)
        {
            isPickUp = false;
            canWalk = false;
            isArrive = true;
            arriveTime = 0;
            
            waitTime = Random.Range(3f, 5f);
        }

        if(canWalk == true)
        {
            if(targetPosition.x < transform.position.x)
            {
                petHandler.petAnimator.FlipSprite(false);
            }
            else
            {
                petHandler.petAnimator.FlipSprite(true);
            }

            // 이동할 방향과 거리를 구합니다.
            Vector3 direction = (targetPosition - transform.position).normalized;
            float distance = moveSpeed * Time.deltaTime;

            // 이동합니다.
            transform.Translate(direction * distance);

            // 일정 거리 이내에 도달했는지 확인하고 이동을 멈춥니다.
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                Debug.Log("도착");

                // 값을 초기화하고 기다려야하는 시간을 랜덤으로 지정
                waitTime = Random.Range(1f, 5f);
                canWalk = false;
                isArrive = true;

                // 대기 애니메이션
                petHandler.petAnimator.PlayAnimation("Idle");
            }
        }

        // 도착했다면 타이머를 시작합니다.
        if(isArrive == true)
        {
            arriveTime += Time.deltaTime;

            // 도착하고 나서 지난 시간이 기다려야하는 시간을 넘었다면
            if(arriveTime >= waitTime)
            {
                // 값을 초기화하고 다음 랜덤 좌표로 이동합니다.
                arriveTime = 0;
                isArrive = false;
                canWalk = true;
                targetPosition = GetRandomPosition();

                // 걷기 애니메이션
                petHandler.petAnimator.PlayAnimation("Walk");
            }
        }


        // 랜덤 상태 변경
        //SetRandomState();
    }

    void SetRandomState()
    {
        // 상태 변경이 불가능할 경우 return
        if(!changeNextState) return;

        // 랜덤 상태를 생성
        CharacterState randomState = GetRandomEnumValue();

        // 생성한 상태가 현재 상태와 같거나 랜덤상태가 None일 경우 재생성
        while(randomState == currentCharacterState || randomState == CharacterState.None)
        {
            randomState = GetRandomEnumValue();
        }

        // 현재 상태를 마지막 상태로 저장
        lastCharacterState = currentCharacterState;

        // 현재 상태를 랜덤 상태로 변경
        currentCharacterState = randomState;

        // 현재 상태 인덱스로 애니메이션 플레이
        petHandler.petAnimator.PlayAnimation("", (int)currentCharacterState);

        // 상태 변경 금지
        changeNextState = false;
    }

    public CharacterState GetRandomEnumValue()
    {
        // 랜덤 Enum을 반환
        var enumValues = System.Enum.GetValues(enumType:typeof(CharacterState));
        return (CharacterState) enumValues.GetValue(Random.Range(0, enumValues.Length));
    }

    Vector3 GetRandomPosition()
    {
        // 카메라의 화면 끝 좌표를 가져옵니다.
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        // 화면 끝에서의 최소 거리를 고려하여 랜덤한 좌표를 생성합니다.        
        // float distanceFromEdge = 1f; // 화면 끝에서의 최소 거리
        // float randomX = Random.Range(bottomLeft.x + distanceFromEdge, topRight.x - distanceFromEdge);
        // float randomY = Random.Range(bottomLeft.y + distanceFromEdge, topRight.y - distanceFromEdge);

        // 콜라이더의 크기를 고려하여 랜덤한 위치를 생성합니다.
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        float randomX = Random.Range(bottomLeft.x + boxCollider.size.x / 2, topRight.x - boxCollider.size.x / 2);
        float randomY = Random.Range(bottomLeft.y + boxCollider.size.y / 2, topRight.y - boxCollider.size.y / 2);

        // 랜덤 좌표의 월드 좌표를 출력합니다.
        Vector3 randomWorldPosition = new Vector3(randomX, randomY, 0f);

        return randomWorldPosition;
    }
}
