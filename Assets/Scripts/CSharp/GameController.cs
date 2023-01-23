using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameController : Singleton<GameController>
{
    public List<GameObject> screens = new List<GameObject>();
    public RocketDatabase rocketbase;
    public LiftoffSceneManager liftoffManager;
    public VideoPlayer transition;
    public int CurrentIndex { get; private set; }
    public float transitionTime = 0.5f;

    private float transitionDuration = 0f;
    private bool inTransition = false;
    private Action operation;
    private float timer = 0f;
    private Webcam _webcam;
    public Webcam Webcam
    {
        get
        {
            if (_webcam == null)
            {
                _webcam = GameObject.Find("Webcam").GetComponent<Webcam>();
            }
            return _webcam;
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        foreach (var screen in screens)
        {
            screen.SetActive(true);
        }

        transitionDuration = (float)transition.clip.length;
    }

    private void Update()
    {
        if (inTransition)
        {
            timer += Time.deltaTime;
            if (timer > transitionTime)
            {
                operation.Invoke();
                inTransition = false;
            }
        }
    }

    void DoTransition(Action operation)
    {
        if (!inTransition)
        {
            this.operation = operation;
            inTransition = true;
            transition.Stop();
            transition.frame = 0;
            transition.Play();
            timer = 0f;
        } else Debug.LogWarning("A transition has been cancelled because another one is already playing!");
    }

    private void Start()
    {
        for (int i = 0; i < screens.Count; i++)
        {
            if (i != 0) screens[i].SetActive(false);
        }
    }

    public void Exit() => Application.Quit();

    public void NextScreen()
    {
        DoTransition(() =>
        {
            screens[CurrentIndex]?.SetActive(false);
            CurrentIndex++;
            screens[CurrentIndex]?.SetActive(true);
        });
    }

    public void PreviousScreen()
    {
        DoTransition(() =>
        {
            screens[CurrentIndex]?.SetActive(false);
            CurrentIndex--;
            screens[CurrentIndex]?.SetActive(true);
        });
    }

    public void JumpScreen(int index)
    {
        DoTransition(() =>
        {
            screens[CurrentIndex]?.SetActive(false);
            CurrentIndex = index;
            screens[CurrentIndex]?.SetActive(true);
        });
    }
}