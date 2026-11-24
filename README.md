# Tugas Besar UAS: Optimalisasi Objek 3D Augmented Reality Multi-Marker
**Kelompok 6 — Mata Kuliah Virtual Reality / Augmented Reality**

Repository ini berisi proyek pengembangan aplikasi Augmented Reality (AR) berbasis Android yang dirancang untuk menampilkan 6 model 3D kawasan zonasi secara optimal, stabil, tidak lag, dan tidak bergoyang menggunakan teknik optimasi aset 3D serta metode *image target tracking*.

---

## Anggota Kelompok dan Pembagian Peran

| No | Nama Lengkap | NIM | Peran | Tanggung Jawab Utama |
| :--- | :--- | :--- | :--- | :--- |
| 1 | Dirli Dwi Putra | [NIM] | Project Leader, 3D Artist, & UI/UX Marker | Koordinasi proyek, optimasi 6 model 3D (Blender & `gltfpack`), serta pembuatan dan verifikasi 6 marker unik (rating 5 bintang di Vuforia) |
| 2 | Neneng Eka Arya Komala | [F55124003] | Lead Developer & AR Specialist | Integrasi Unity 6 & Vuforia Engine (6 *Image Target* & *tracking*), implementasi script C# (`ARPerformanceMonitor.cs`), serta *build* `.apk` Android |
| 3 | Atalya Victoria Dominic Kupagan | [NIM] | QA/Tester, DevOps, Documentation, & Presenter | Pengujian performa `.apk` di Android (FPS, RAM, *error handling*), manajemen repository GitHub, penyusunan laporan PDF & slide presentasi, serta produksi video demo |

---

## Tools dan Spesifikasi Teknis

* **Game Engine:** Unity 6 (6000.6.9f1) — Universal Render Pipeline (URP)
* **AR SDK:** Vuforia Engine 11.4.4 (dengan *Device Pose Tracking* aktif)
* **Optimasi Aset 3D:** Blender dan `gltfpack`
* **Bahasa Pemrograman:** C# (`ARPerformanceMonitor.cs` untuk pemantauan FPS, *frame time*, dan penggunaan RAM secara *real-time*)
* **Target Platform:** Android (Minimum API Level 24 / Android 10.0 (API 29), Scripting Backend `IL2CPP`, Target Architecture `ARM64`)

---

## Daftar Model 3D dan Marker

Aplikasi ini menggunakan 6 marker unik yang telah diverifikasi pada Vuforia Target Manager (rating 5 bintang) untuk memanggil masing-masing model 3D zonasi:

| No | Nama Zonasi | Nama File Model 3D | Nama Marker di Database |
| :--- | :--- | :--- | :--- |
| 1 | Electrical Zone | `Electical_Zone.glb` | `marker_electrical` |
| 2 | Real Estate | `Realestate.glb` | `marker_realestate` |
| 3 | Gudang | `Gudang.glb` | `marker_gudang` |
| 4 | Kantor dan RS | `Kantor_dan_RS.glb` | `marker_kantor_rs` |
| 5 | Agroindustri | `AgroIndustri.glb` | `marker_agroindustri` |
| 6 | Industri | `Industri1.glb` | `marker_industri` |

---

## Struktur Repository

Struktur folder dalam repository ini disusun sebagai berikut:

* `/src` : Berisi kode sumber C# (`ARPerformanceMonitor.cs`) dan file konfigurasi *scene* Unity.
* `/assets` : Berisi 6 file model 3D (`.glb`) hasil optimasi serta 6 file gambar marker.
* `/build` : Berisi file instalasi aplikasi Android (`.apk`) atau tautan unduhan jika ukuran file melebihi batas unggah GitHub.
* `/docs` : Berisi diagram arsitektur sistem (`arsitektur_sistem_ar_multimarker.png` / `.svg`), diagram alur kerja (`diagram_alur_kerja_ar_multimarker.png` / `.svg`), laporan proyek (PDF), dan slide presentasi.
* `/demo` : Berisi tautan Google Drive video demonstrasi aplikasi dan tangkapan layar pengujian.

---

## Arsitektur Sistem dan Alur Kerja Aplikasi

Aplikasi Augmented Reality ini dirancang dengan standar arsitektur perangkat lunak berlapis (*Layered Architecture*) dan alur kerja standar ISO 5807:

1. **Arsitektur Sistem Aplikasi AR Multi-Marker & Optimalisasi Objek 3D** (`docs/arsitektur_sistem_ar_multimarker.png` dan `.svg`)
   * Mengintegrasikan 5 lapisan utama:
     - **Layer 1 (Presentation & UI):** Viewport AR, Real-time GUI HUD (`OnGUI`), dan status indikator adaptif (Hijau/Kuning/Merah).
     - **Layer 2 (Application Core & Logic):** Unity 6 URP Runtime (IL2CPP ARM64), `SampleScene.unity`, `DefaultObserverEventHandler`, dan script `ARPerformanceMonitor.cs` (Framerate lock 60 FPS).
     - **Layer 3 (AR & Tracking Subsystem):** Vuforia Engine 11.4.4, Natural Feature Tracking (NFT), 6-DoF Pose Solver, dan *Device Pose Tracking* (Extended Tracking & VIO anti-goyang).
     - **Layer 4 (3D Asset & Optimization Pipeline):** Dua tahap optimasi offline: Blender (Decimate polygon reduction 60-80%, mesh cleaning, texture atlas) dan `gltfpack` CLI (meshopt compression, attribute quantization 16-bit/8-bit, POT texture resizing).
     - **Layer 5 (Hardware & OS Platform):** Android OS (API 29+ / ARM64), sensor kamera CMOS RGB, sensor IMU, dan 6 marker fisik 5-bintang.

2. **Diagram Alur Kerja (Workflow / Activity)** (`docs/diagram_alur_kerja_ar_multimarker.png` dan `.svg`)
   * Memetakan siklus hidup end-to-end aplikasi:
     - Inisialisasi runtime dan verifikasi izin kamera (*Runtime Permission*).
     - Akuisisi stream citra kamera dan ekstraksi feature points secara real-time.
     - Percabangan deteksi marker (*Image Target Found / Lost*):
       * Jika marker terdeteksi: Panggilan model zonasi sesuai ID marker (`Electical_Zone`, `Realestate`, `Gudang`, `Kantor_dan_RS`, `AgroIndustri`, `Industri1`).
       * Jika tidak terdeteksi: Evaluasi *Device Pose Tracking* untuk mempertahankan posisi virtual atau menonaktifkan MeshRenderer (0% draw call) guna efisiensi baterai/GPU.
     - Perhitungan pose 6-DoF, rendering URP, dan kalkulasi performa real-time (`deltaTime` smoothing, kalkulasi FPS, frame latency ms, dan memori RAM MB).
     - Loop kontinu frame rate 60 FPS hingga aplikasi ditutup dengan aman.

---

## Cara Menjalankan Aplikasi (Android)

1. Unduh file `.apk` yang tersedia di dalam folder `/build`.
2. Pindahkan file `.apk` ke perangkat Android, lalu lakukan instalasi (izinkan opsi *Install from Unknown Sources* jika diminta oleh sistem).
3. Buka aplikasi dan berikan izin akses **Kamera**.
4. Arahkan kamera HP ke salah satu dari 6 gambar marker yang ada di folder `/assets` (dapat dicetak atau ditampilkan melalui layar perangkat lain).
5. Model 3D akan muncul di atas marker beserta informasi performa (FPS dan RAM) di pojok kiri atas layar.

---

## Cara Build dari Unity Editor

Jika ingin melakukan *build* ulang dari proyek Unity:

1. Pastikan **Unity 6 (6000.6.9f1)** sudah terpasang beserta modul **Android Build Support** (*OpenJDK*, *Android SDK & NDK Tools*).
2. Buka proyek ini melalui Unity Hub dan pastikan *scene* `SampleScene` terbuka.
3. Pastikan paket **Vuforia Engine 11.4.4** dan database marker `DatabaseKelompok6.unitypackage` sudah terimpor dengan benar.
4. Masuk ke menu **File > Build Profiles**, pilih profil **Android**, lalu klik **Switch Profile**.
5. Buka **Player Settings > Other Settings** dan pastikan pengaturan berikut sudah sesuai:
   * **Minimum API Level:** Android 10.0 (API 29) atau lebih tinggi.
   * **Scripting Backend:** `IL2CPP`.
   * **Target Architectures:** Centang `ARM64`.
6. Klik **Build** dan pilih lokasi penyimpanan untuk menghasilkan file `.apk`.

---

## Sumber Aset dan Lisensi

* Seluruh model 3D kawasan zonasi (6 file `.glb`) bersumber dari materi tugas besar yang disediakan oleh dosen pengampu mata kuliah.
* *Library* AR menggunakan **Vuforia Engine** versi Basic/Development untuk keperluan akademik dan non-komersial.