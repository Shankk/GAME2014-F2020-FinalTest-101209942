using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShrinkingPlatformController : MonoBehaviour
{
    public bool isActive;
    bool IsFloating = false;
    bool IsShrinking = false;
    bool IsUnShrinking = false;


    public PlayerBehaviour player;
    public AudioSource ShrinkAudio;
    public AudioClip PositiveShrink;
    public AudioClip NegativeShrink;

    private Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        ShrinkAudio = GetComponent<AudioSource>();
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(FloatEffect(transform.position, transform.position.y + 1.0f, 2.0f));
        if (isActive)
        {
            StartCoroutine(ShrinkEffect(transform.localScale.x, 0, 2.0f));
        }
        else
        {
            StartCoroutine(UnShrinkEffect(transform.localScale.x, 1, 2.0f));
        }
    }

    IEnumerator FloatEffect(Vector3 StartPos, float EndY ,float Duration)
    {
        if(!IsFloating)
        {
            IsFloating = true;
            var time = 0.0f;
        
            while(time < 1.0f)
            {
                transform.position = Vector3.Lerp(StartPos, new Vector3(transform.position.x, EndY, 0), time);
                time = time + Time.deltaTime / Duration;
                yield return null;
            }

            time = 0.0f;
            while (time < 1.0f)
            {
                transform.position = Vector3.Lerp(new Vector3(StartPos.x, EndY,StartPos.z), new Vector3(transform.position.x, StartPos.y, 0), time);
                time = time + Time.deltaTime / Duration;
                yield return null;
            }

            IsUnShrinking = false;
            IsFloating = false;
            yield return null;
        }
        
    }

    IEnumerator ShrinkEffect(float StartScaleX, float EndScaleX, float Duration)
    {
        if(!IsShrinking)
        {
            IsShrinking = true;

            ShrinkAudio.Stop();
            ShrinkAudio.clip = NegativeShrink;
            ShrinkAudio.Play();
            
            var time = 0.0f;
            while(time < 1.0f)
            {
                if(isActive)
                {
                    transform.localScale = Vector3.Lerp(new Vector3(StartScaleX, 1, 1), new Vector3(EndScaleX, 1, 1), time);
                    time = time + Time.deltaTime / Duration;
                    yield return null;
                }
                else
                {
                    time = 1f;
                    yield return null;
                }
            }

            IsShrinking = false;
            yield return null;
        }
    }

    IEnumerator UnShrinkEffect(float StartScaleX, float EndScaleX, float Duration)
    {
        if (!IsUnShrinking)
        {
            IsUnShrinking = true;

            if(StartScaleX < 0.9)
            {
                ShrinkAudio.Stop();
                ShrinkAudio.clip = PositiveShrink;
                ShrinkAudio.Play();
            }
            
            var time = 0.0f;
            while (time < 1.0f)
            {
                if (!isActive)
                {
                    transform.localScale = Vector3.Lerp(new Vector3(StartScaleX, 1, 1), new Vector3(EndScaleX, 1, 1), time);
                    time = time + Time.deltaTime / Duration;
                    yield return null;
                }
                else
                {
                    time = 1f;
                    yield return null;
                }
            }

            IsUnShrinking = false;
            yield return null;
        }
    }
}
