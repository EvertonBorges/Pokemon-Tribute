using System;
using UnityEngine;

public class Manager_Transitions : Singleton<Manager_Transitions>
{

    [SerializeField] private UI_FadeEffect _blocker;
    public UI_FadeEffect Blocker => _blocker;

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);

        _blocker.HideForced();

        Manager_Events.GameManager.OnTransite += Transite;
    }

    private void Transite(bool fadeIn, Action callback)
    {
        if (fadeIn)
            _blocker.FadeIn(callback);
        else
            _blocker.FadeOut(callback);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Manager_Events.GameManager.OnTransite -= Transite;
    }

}
