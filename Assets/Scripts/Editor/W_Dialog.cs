using UnityEditor;
using System;
using UnityEngine;

public class W_Dialog : EditorWindow
{

    private static Action m_body;
    private static Action m_confirmCallback;
    private static Action m_cancelCallback;
    private static string m_confirmText;
    private static string m_cancelText;

    public static void ShowWindow(string title, Action body, string confirmText = "OK", string cancelText = "Cancel", Action confirmCallback = null, Action cancelCallback = null)
    {
        var window = GetWindow<W_Dialog>(title);

        m_body = body;

        m_confirmText = confirmText;

        m_cancelText = cancelText;

        m_confirmCallback = confirmCallback;

        m_cancelCallback = cancelCallback;

        window.minSize = new(400, 200);

        window.maxSize = new(400, 200);

        window.position = new(Screen.width - 200, Screen.height - 100, 400, 200);

        window.ShowModalUtility();
    }

    void OnGUI()
    {
        var skinBox = GUI.skin.box;

        var skinButton = GUI.skin.button;

        GUILayout.BeginVertical(skinBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        GUILayout.BeginHorizontal(skinBox, GUILayout.Height(150f), GUILayout.ExpandWidth(true));

        m_body?.Invoke();

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(skinBox, GUILayout.Height(35f), GUILayout.ExpandWidth(true));

        GUILayout.FlexibleSpace();

        if (!m_confirmText.IsEmpty())
            if (GUILayout.Button(m_confirmText, skinButton, GUILayout.Width(60f), GUILayout.ExpandHeight(true)))
            {
                m_confirmCallback?.Invoke();

                Close();
            }

        if (!m_cancelText.IsEmpty())
            if (GUILayout.Button(m_cancelText, skinButton, GUILayout.Width(60f), GUILayout.ExpandHeight(true)))
            {
                m_cancelCallback?.Invoke();

                Close();
            }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

}
