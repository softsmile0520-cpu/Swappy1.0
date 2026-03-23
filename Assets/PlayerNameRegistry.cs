using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks which account email owns a display name so we can detect duplicates.
/// Uses Owner keys plus a registered-email list and legacy <c>email + "_playerName"</c> checks.
/// </summary>
public static class PlayerNameRegistry
{
    private const string KeyPrefix = "PlayerNameOwner_";
    private const string RegisteredEmailsKey = "PlayerNameRegistry_Emails";

    public static string Normalize(string playerName)
    {
        if (string.IsNullOrEmpty(playerName)) return "";
        return playerName.Trim().ToLowerInvariant();
    }

    private static string OwnerKey(string normalized) => KeyPrefix + normalized;

    /// <summary>Returns the email that owns this display name via Owner_* key, or empty.</summary>
    public static string GetOwnerEmailForDisplayName(string displayName)
    {
        string n = Normalize(displayName);
        if (n.Length == 0) return "";
        if (!PlayerPrefs.HasKey(OwnerKey(n))) return "";
        return PlayerPrefs.GetString(OwnerKey(n), "").Trim();
    }

    /// <summary>Finds an account email whose saved <c>email + "_playerName"</c> matches (legacy / list scan).</summary>
    public static string FindEmailWithStoredPlayerName(string displayName)
    {
        string n = Normalize(displayName);
        if (n.Length == 0) return "";
        foreach (string email in GetRegisteredEmails())
        {
            if (string.IsNullOrEmpty(email)) continue;
            string stored = PlayerPrefs.GetString(email + "_playerName", "");
            if (Normalize(stored) == n)
                return email.Trim();
        }
        return "";
    }

    /// <summary>Adds email to the list (deduped) so we can scan legacy <c>_playerName</c> entries.</summary>
    public static void AppendRegisteredEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return;
        email = email.Trim();
        string list = PlayerPrefs.GetString(RegisteredEmailsKey, "");
        if (string.IsNullOrEmpty(list))
        {
            PlayerPrefs.SetString(RegisteredEmailsKey, email);
            return;
        }
        foreach (string part in list.Split('|'))
        {
            if (string.Equals(part.Trim(), email, StringComparison.OrdinalIgnoreCase))
                return;
        }
        PlayerPrefs.SetString(RegisteredEmailsKey, list + "|" + email);
    }

    /// <summary>Seed list from known prefs (existing installs before registry existed).</summary>
    public static void BootstrapRegisteredEmailList()
    {
        AppendRegisteredEmail(PlayerPrefs.GetString("lastLoginEmail", ""));
        AppendRegisteredEmail(PlayerPrefs.GetString("rememberedEmail", ""));
    }

    private static IEnumerable<string> GetRegisteredEmails()
    {
        string list = PlayerPrefs.GetString(RegisteredEmailsKey, "");
        if (string.IsNullOrEmpty(list)) yield break;
        foreach (string raw in list.Split('|'))
        {
            string e = raw.Trim();
            if (e.Length > 0) yield return e;
        }
    }

    /// <summary>True if any saved account (other than <paramref name="myEmail"/>) uses this display name.</summary>
    private static bool LegacyAnyOtherAccountHasPlayerName(string proposedName, string myEmail)
    {
        string n = Normalize(proposedName);
        if (n.Length == 0) return false;

        foreach (string email in GetRegisteredEmails())
        {
            if (string.IsNullOrEmpty(email)) continue;
            if (!string.IsNullOrEmpty(myEmail) &&
                string.Equals(email, myEmail, StringComparison.OrdinalIgnoreCase))
                continue;

            string stored = PlayerPrefs.GetString(email + "_playerName", "");
            if (Normalize(stored) == n)
                return true;
        }

        return false;
    }

    /// <summary>Call after signup or when syncing login so this email owns this display name.</summary>
    public static void RegisterNameForEmail(string email, string playerName)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(playerName)) return;
        string n = Normalize(playerName);
        if (n.Length == 0) return;
        PlayerPrefs.SetString(OwnerKey(n), email);
        AppendRegisteredEmail(email);
    }

    public static void UnregisterName(string playerName)
    {
        string n = Normalize(playerName);
        if (n.Length == 0) return;
        PlayerPrefs.DeleteKey(OwnerKey(n));
    }

    public static void ChangeRegisteredName(string email, string oldPlayerName, string newPlayerName)
    {
        UnregisterName(oldPlayerName);
        RegisterNameForEmail(email, newPlayerName);
    }

    /// <summary>True if another account already uses this name.</summary>
    public static bool IsNameTakenByAnother(string proposedName, string myEmail)
    {
        string n = Normalize(proposedName);
        if (n.Length == 0) return false;

        if (PlayerPrefs.HasKey(OwnerKey(n)))
        {
            string owner = PlayerPrefs.GetString(OwnerKey(n));
            if (string.IsNullOrEmpty(myEmail))
                return true;
            if (!string.Equals(owner, myEmail, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        if (LegacyAnyOtherAccountHasPlayerName(proposedName, myEmail))
            return true;

        return false;
    }

    /// <summary>Rebuild Owner_* keys from email list + stored player names (fixes gaps after updates).</summary>
    public static void RebuildOwnerKeysFromEmailList()
    {
        BootstrapRegisteredEmailList();
        foreach (string email in GetRegisteredEmails())
        {
            string pn = PlayerPrefs.GetString(email + "_playerName", "");
            if (!string.IsNullOrEmpty(pn))
                RegisterNameForEmail(email, pn);
        }
    }
}
