using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Animation", menuName = "Custom Animation/Animation")]
public class CustomAnimation : ScriptableObject
{
    [SerializeField] public string Key = "AnimationName";
    [SerializeField] public float frameSpeed = 1f;
    [SerializeField] public Sprite[] sprites;

}