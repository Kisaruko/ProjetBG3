using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonDisplayer : MonoBehaviour
{
    public bool isActive;
    private Transform playerPos;
    public float heightFromPlayer;
    private SpriteRenderer sprite;
    public ParticleSystem particles;
    public GameObject bouncyCircle;
    private void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        transform.DOScale(0.15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if(isActive)
        {
            transform.position = playerPos.position + Vector3.up * heightFromPlayer;
        }
    }
   public void Appear()
    {
        isActive = true;
        sprite.enabled = true;
        particles.Play();
        if(bouncyCircle != null)
        {
            bouncyCircle.SetActive(true);
        }
    }
    public void Disappear()
    {
        isActive = false;
        sprite.enabled = false;
        particles.Stop();
        if (bouncyCircle != null)
        {
            bouncyCircle.SetActive(false);
        }
    }


}
