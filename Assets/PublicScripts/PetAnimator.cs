using System.Collections;
using System.Linq;
using UnityEngine;

public class PetAnimator : MonoBehaviour, IHandler
{
    public PetHandler petHandler { get; set; }

    [TextArea]
    public string comment = "더 이상 손 댈 것이 없으므로 닫아두세요.";
    public SpriteRenderer spriteRenderer;
    public int FirstAnimationIndex = 0;
    public string FirstAnimationName = "";
    public int currentAnimationIndex = 0;
    public int currentSpriteIndex = 0;
    public float currentFrameSpeed = 0f;
    public bool interrupt = false;
    public CustomAnimationPack AnimationPack;

    void Start()
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        PlayAnimation(FirstAnimationName, FirstAnimationIndex);
        StartCoroutine("PlayAnimationCoroutine");
    }

    IEnumerator PlayAnimationCoroutine()
    {
        while(true)
        {
            PlayNextFrame();

            float waitTime = 0f;

            while(waitTime < currentFrameSpeed && !interrupt)
            {
                waitTime += Time.deltaTime;
                yield return null;
            }

            ControlInterrupt();

            yield return null;
        }
    }

    public void PlayAnimation(string AnimationName = "", int AnimationIndex = -1)
    {
        if(AnimationName == "" && AnimationIndex != -1)
        {
            SetCurrentState(0, AnimationIndex, AnimationPack.animations[AnimationIndex].frameSpeed);
            return;
        }
        
        if(AnimationIndex != -1 && AnimationPack.animations[AnimationIndex].Key.Equals(AnimationName))
        {
            SetCurrentState(0, AnimationIndex, AnimationPack.animations[AnimationIndex].frameSpeed);
            return;
        }


        foreach(var item in AnimationPack.animations.Select((value, index) => new {value, index}))
        {
            var animation = item.value;

            if(animation.Key.Equals(AnimationName))
            {
                SetCurrentState(0, item.index, animation.frameSpeed);
                return;
            }
        }
    }

    void SetCurrentState(int spriteIndex, int animationIndex, float frameSpeed)
    {
        interrupt = true;
        currentSpriteIndex = spriteIndex;
        currentAnimationIndex = animationIndex;
        currentFrameSpeed = frameSpeed;
    }

    void PlayNextFrame()
    {
        spriteRenderer.sprite = AnimationPack.animations[currentAnimationIndex].sprites[currentSpriteIndex];

        currentSpriteIndex += 1;

        if(currentSpriteIndex >= AnimationPack.animations[currentAnimationIndex].sprites.Length)
        {
            currentSpriteIndex = 0;
        }
    }

    void ControlInterrupt()
    {
        if(interrupt)
        {
            // 애니메이션 중간에 끼어들기가
            // 들어온다면 처리할 내용

            interrupt = false;
        }
    }

    public void FlipSprite(bool value)
    {
        spriteRenderer.flipX = value;
    }
}
