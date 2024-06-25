using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverrides : MonoBehaviour
{
    [SerializeField] private GameObject character = null;
    [SerializeField] private SO_AnimationType[] soAnimationTypeArray = null;

    private Dictionary<AnimationClip, SO_AnimationType> animaionTypeDictionaryByAnimation;
    private Dictionary<string,SO_AnimationType> animaionTypeDictionaryByCompositeAttributeKey;


    private void Start()
    {
        //Initialize animation type dictionary keyed by animation clip
        animaionTypeDictionaryByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();

        foreach(SO_AnimationType item in soAnimationTypeArray)
        {
            animaionTypeDictionaryByAnimation.Add(item.animationClip, item);
        }

        //Initialize animation type dictionary keyed by string
        animaionTypeDictionaryByCompositeAttributeKey = new Dictionary<string,SO_AnimationType>();
        foreach(SO_AnimationType item in soAnimationTypeArray)
        {
            string key = item.characterPart.ToString() + item.partVariantColor.ToString() +
                item.partVariantType.ToString() + item.animationName.ToString();
            //Debug.Log(key);
            animaionTypeDictionaryByCompositeAttributeKey.Add(key, item);
        }
    }

    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterAttributesList)
    {
        foreach(CharacterAttribute characterAttribute in characterAttributesList)
        {
            Animator currentAnimator = null;
            List<KeyValuePair<AnimationClip, AnimationClip>> animsKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            string animatorSOAssetName = characterAttribute.characterPart.ToString();

            
            Animator[] animatorArray = character.GetComponentsInChildren<Animator>();

            foreach(Animator animator in animatorArray)
            {
                //Debug.Log(animator.name);
                //Debug.Log(animatorSOAssetName);
                if(animator.name == animatorSOAssetName)
                {
                    //Debug.Log("Enter");
                    currentAnimator = animator;
                    break;
                }
            }

            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);
            List<AnimationClip> animationList = new List<AnimationClip>(aoc.animationClips);

            foreach(AnimationClip animationClip in animationList)
            {
                SO_AnimationType so_AnimationType;
                bool foundAnimation = animaionTypeDictionaryByAnimation.TryGetValue(animationClip, out so_AnimationType);

                if (foundAnimation)
                {
                    string key = characterAttribute.characterPart.ToString() +
                        characterAttribute.partVariantColor.ToString() +
                        characterAttribute.partVariantType.ToString() +
                        so_AnimationType.animationName.ToString();

                    SO_AnimationType swapSO_AnimationType;
                    //Debug.Log(key);//ArmnonecarrywalkDown
                    bool foundSwapAnimation = animaionTypeDictionaryByCompositeAttributeKey.TryGetValue(key, out swapSO_AnimationType);

                    if (foundSwapAnimation)
                    {
                        AnimationClip swapAnimationClip = swapSO_AnimationType.animationClip;

                        animsKeyValuePairList.Add(new KeyValuePair<AnimationClip,AnimationClip>(animationClip,swapAnimationClip));


                    }
                }
            }

            aoc.ApplyOverrides(animsKeyValuePairList);
            currentAnimator.runtimeAnimatorController = aoc;
        }
    }
}
