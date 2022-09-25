using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using TMPro;
using System;

public class Manager_Dialog : Singleton<Manager_Dialog>
{

    [SerializeField] private UI_FadeEffect _fadeEffect;
    public bool IsShowing => _fadeEffect.IsShowing || _fadeEffect.IsFading;
    
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private TextMeshProUGUI _text;

    private int m_line = -1;
    private string[] m_text = null;
    private bool m_fastRead = false;
    private Action m_callback = null;
    private Coroutine m_coroutineWriteText = null;

    public void Setup(LocalizedString key, Action callback = null)
    {
        _fadeEffect.FadeIn();

        _text.text = "";

        m_line = 0;

        m_text = key.GetLocalizedString().Split("\n");

        m_fastRead = false;

        m_callback = callback;

        if (m_coroutineWriteText != null)
            StopCoroutine(m_coroutineWriteText);

        m_coroutineWriteText = StartCoroutine(WriteText());
    }

    public void NextLine()
    {
        if (m_coroutineWriteText != null)
        {
            m_fastRead = true;

            return;
        }

        if (m_line < m_text.Length - 1)
        {
            m_line++;

            m_coroutineWriteText = StartCoroutine(WriteText());
        }
        else
            Hide();
    }

    public IEnumerator WriteText()
    {
        string line = m_text[m_line];

        m_fastRead = false;

        if (m_line != 0)
            _text.text += "\n";

        _scroll.verticalNormalizedPosition = 0f;

        yield return new WaitForSeconds(0.25f);

        foreach (var item in line)
        {
            for (int i = 0; i < (m_fastRead ? 1 : 3); i++)
                yield return new WaitForFixedUpdate();

            _text.text += item;

            _scroll.verticalNormalizedPosition = 0f;
        }

        if (m_line <= 0 && m_text.Length > 1)
        {
            m_line++;

            yield return WriteText();
        }

        m_coroutineWriteText = null;
    }

    private void Hide()
    {
        m_line = -1;

        _fadeEffect.FadeOut(() => 
        {
            m_text = null;

            m_callback?.Invoke();
        });
    }

}
