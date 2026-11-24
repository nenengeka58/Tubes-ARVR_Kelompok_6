using UnityEngine;
using UnityEngine.Profiling;

public class ARPerformanceMonitor : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private GUIStyle style;
    private Rect rect;

    void Start()
    {
        // Mengunci target frame rate di 60 FPS agar stabil di Android
        Application.targetFrameRate = 60;

        int w = Screen.width, h = Screen.height;
        rect = new Rect(20, 20, w * 0.6f, h * 0.15f);

        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = Mathf.Max(24, h * 2 / 100);
        style.normal.textColor = Color.green;
    }

    void Update()
    {
        // Menghitung rata-rata waktu antar frame (smoothing)
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        float ramMB = Profiler.GetTotalAllocatedMemoryLong() / (1024f * 1024f);

        // Ubah warna teks otomatis berdasarkan performa FPS
        if (fps >= 45f) style.normal.textColor = Color.green;
        else if (fps >= 25f) style.normal.textColor = Color.yellow;
        else style.normal.textColor = Color.red;

        string text = string.Format(
            "AR Performance Monitor\nFPS: {0:0.} ({1:0.0} ms)\nRAM: {2:0.0} MB",
            fps, msec, ramMB
        );

        // Kotak latar belakang semi-transparan agar tulisan jelas terbaca
        GUI.Box(new Rect(10, 10, 380, 110), "");
        GUI.Label(rect, text, style);
    }
}