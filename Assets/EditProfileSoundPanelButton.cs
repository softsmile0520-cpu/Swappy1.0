using UnityEngine;

/// <summary>
/// Used on the SoundsPanel "Edit Profile" button so the prefab does not need a fragile
/// cross-reference to the root <see cref="SoundsPanel"/> component (avoids "missing script" on save).
/// </summary>
[DisallowMultipleComponent]
public class EditProfileSoundPanelButton : MonoBehaviour
{
    public void OnEditProfileClicked()
    {
        SettingPanel.ShowUI();
        var sounds = GetComponentInParent<SoundsPanel>();
        if (sounds != null)
            sounds.backPressed();
    }
}
