using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Picks a profile image from disk. NativeGallery does not open a dialog on Windows/Linux/macOS standalone
/// (callback is null); this class uses the Editor file dialog in the Editor and Win32 on Windows builds.
/// </summary>
public static class ProfileImageFilePicker
{
    /// <summary>Opens a platform file picker; invokes callback with a filesystem path or null if cancelled.</summary>
    public static void PickImageForProfile(Action<string> onPathChosen)
    {
        if (onPathChosen == null) return;

#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanelWithFilters(
            "Select image", "",
            new[] { "Image files", "png,jpg,jpeg,jpe,bmp,gif", "All files", "*" });
        onPathChosen(string.IsNullOrEmpty(path) ? null : path);
#elif UNITY_ANDROID || UNITY_IOS
        NativeGallery.GetImageFromGallery(
            (path) => onPathChosen(string.IsNullOrEmpty(path) ? null : path),
            "Select an image", "image/*");
#elif UNITY_STANDALONE_WIN
        string path = PickImagePathWindows();
        onPathChosen(path);
#elif UNITY_STANDALONE_OSX
        onPathChosen(PickImagePathMacOs());
#else
        Debug.LogWarning("ProfileImageFilePicker: no file dialog for this platform. Use Editor or Windows/macOS standalone.");
        onPathChosen(null);
#endif
    }

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class WinOpenFileName
    {
        public int structSize;
        public IntPtr dlgOwner;
        public IntPtr instance;
        public string filter;
        public string customFilter;
        public int maxCustFilter;
        public int filterIndex;
        public string file;
        public int maxFile;
        public string fileTitle;
        public int maxFileTitle;
        public string initialDir;
        public string title;
        public int flags;
        public short fileOffset;
        public short fileExtension;
        public string defExt;
        public IntPtr custData;
        public IntPtr hook;
        public string templateName;
        public IntPtr reservedPtr;
        public int reservedInt;
        public int flagsEx;
    }

    [DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GetOpenFileName([In, Out] WinOpenFileName ofn);

    private const int OFN_FILEMUSTEXIST = 0x00001000;
    private const int OFN_PATHMUSTEXIST = 0x00000800;
    private const int OFN_EXPLORER = 0x00080000;

    private static string PickImagePathWindows()
    {
        var ofn = new WinOpenFileName();
        ofn.structSize = Marshal.SizeOf(typeof(WinOpenFileName));
        ofn.filter = "Image files\0*.png;*.jpg;*.jpeg;*.bmp;*.gif\0All files\0*.*\0\0";
        ofn.file = new string(new char[512]);
        ofn.maxFile = ofn.file.Length;
        ofn.title = "Select an image";
        ofn.flags = OFN_EXPLORER | OFN_FILEMUSTEXIST | OFN_PATHMUSTEXIST;

        if (!GetOpenFileName(ofn))
            return null;

        string path = ofn.file.TrimEnd('\0');
        if (string.IsNullOrEmpty(path))
            return null;
        return File.Exists(path) ? path : null;
    }
#endif

#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
    private static string PickImagePathMacOs()
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "/usr/bin/osascript",
                Arguments = "-e 'POSIX path of (choose file)'",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            using (var p = System.Diagnostics.Process.Start(psi))
            {
                if (p == null) return null;
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                string path = output.Trim().TrimEnd('\r', '\n');
                return string.IsNullOrEmpty(path) || !File.Exists(path) ? null : path;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("ProfileImageFilePicker macOS: " + e.Message);
            return null;
        }
    }
#endif
}
