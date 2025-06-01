using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class CustomAnimation
{
    [SerializeField] public string Key = "AnimationName";
    [SerializeField] public float frameSpeed = 1f;
    [SerializeField] public Sprite[] sprites;
}


public class CustomAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int FirstAnimationIndex = 0;
    public string FirstAnimationName = "";
    public int currentAnimationIndex = 0;
    public int currentSpriteIndex = 0;
    public float currentFrameSpeed = 0f;
    public bool interrupt = false;
    public GameObject AnnoyingEffect;
    public bool isAnnoying = false;
    public float annoyingTime = 0f;
    public CustomAnimation[] animations;
    public GameObject HeartEffect;

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
            SetCurrentState(0, AnimationIndex, animations[AnimationIndex].frameSpeed);
            return;
        }
        
        if(AnimationIndex != -1 && animations[AnimationIndex].Key.Equals(AnimationName))
        {
            SetCurrentState(0, AnimationIndex, animations[AnimationIndex].frameSpeed);
            return;
        }


        foreach(var item in animations.Select((value, index) => new {value, index}))
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
        spriteRenderer.sprite = animations[currentAnimationIndex].sprites[currentSpriteIndex];

        currentSpriteIndex += 1;

        if(currentSpriteIndex >= animations[currentAnimationIndex].sprites.Length)
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


    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PlayAnimation("Idle", 0);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            PlayAnimation("Alert", 1);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            PlayAnimation("Prone", 2);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayAnimation("Walk", 3);
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            PlayAnimation("Jump", 4);
        }

        if(isAnnoying)
        {
            annoyingTime += Time.deltaTime;

            if(annoyingTime > 1f)
            {
                StopAnnoying();
            }
        }
    }

    public void StartAnnoying()
    {
        AnnoyingEffect.SetActive(true);
        isAnnoying = true;
        annoyingTime = 0f;
    }  
    public void StopAnnoying()
    {
        AnnoyingEffect.SetActive(false);
        isAnnoying = false;
        annoyingTime = 0f;
    }  

    public void StartHeartEffect()
    {
        Destroy(Instantiate(HeartEffect,transform), 1f);
    }
}
