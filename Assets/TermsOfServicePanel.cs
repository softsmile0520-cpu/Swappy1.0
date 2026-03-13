using System;
using UnityEngine;
using UnityEngine.UI;

public class TermsOfServicePanel : MonoBehaviour
{
    public const string TermsAcceptedKey = "TermsAccepted";

    private Button agreeButton;
    private Button declineButton;
    private Action onAccepted;
    private Action onDeclined;

    /// <summary>
    /// Call after instantiating the panel. Sets callbacks and wires buttons.
    /// </summary>
    public void Setup(Action onAcceptedCallback, Action onDeclinedCallback = null)
    {
        onAccepted = onAcceptedCallback;
        onDeclined = onDeclinedCallback;

        var buttons = GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            if (btn.gameObject.name.Contains("I AGREE") || btn.gameObject.name == "I AGREE")
            {
                agreeButton = btn;
                agreeButton.onClick.RemoveAllListeners();
                agreeButton.onClick.AddListener(OnAgreeClicked);
            }
            else if (btn.gameObject.name.Contains("DECLINE") || btn.gameObject.name == "DECLINE")
            {
                declineButton = btn;
                declineButton.onClick.RemoveAllListeners();
                declineButton.onClick.AddListener(OnDeclineClicked);
            }
        }

        if (agreeButton == null)
            Debug.LogWarning("TermsOfServicePanel: 'I AGREE' button not found.");
        if (declineButton == null)
            Debug.LogWarning("TermsOfServicePanel: 'DECLINE' button not found.");
    }

    private void OnAgreeClicked()
    {
        PlayerPrefs.SetInt(TermsAcceptedKey, 1);
        PlayerPrefs.Save();
        onAccepted?.Invoke();
        Destroy(gameObject);
    }

    private void OnDeclineClicked()
    {
        onDeclined?.Invoke();
        Destroy(gameObject);
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
