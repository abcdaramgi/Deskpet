using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom AnimationPack", menuName = "Custom Animation/AnimationPack")]
public class CustomAnimationPack : ScriptableObject
{
    public CustomAnimation[] animations;
}