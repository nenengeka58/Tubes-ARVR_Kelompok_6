# Tugas Besar UAS: Pengembangan Aplikasi Augmented Reality Multi-Marker Kawasan Zonasi
**Kelompok 6 — Mata Kuliah Virtual Reality / Augmented Reality**  
Program Studi Teknik Informatika, Universitas Tadulako

Repository ini berisi proyek pengembangan aplikasi Augmented Reality (AR) multi-marker berbasis Android (`AR_Zonasi_Kelompok6.apk`) yang dirancang untuk menampilkan 6 model 3D kawasan zonasi secara ringan, akurat, dan stabil tanpa *lag* maupun *jitter* menggunakan metode optimalisasi *mesh* `gltfpack` dan *Device Pose Tracking*.

---

## Anggota Kelompok dan Pembagian Peran

| No | Nama Lengkap | NIM | Peran | Tanggung Jawab Utama |
| :--- | :--- | :--- | :--- | :--- |
| 1 | Dirli Dwi Putra | F55124022 | Project Leader & 3D Artist | Perumusan konsep kawasan zonasi, prapemrosesan dan optimalisasi 6 model `.glb` menggunakan Blender dan `gltfpack`, serta perancangan 6 *marker* unik |
| 2 | Neneng Eka Arya Komala | F55124003 | Lead Developer & AR Specialist | Perakitan *scene* `SampleScene.unity` pada Unity 6.6 (URP), integrasi Vuforia Engine dan *Device Pose Tracking*, serta penyelesaian kendala konfigurasi dan kompilasi Android |
| 3 | Atalya Victoria Dominic Kupagan | F55124028 | AR Developer, QA/Tester, & DevOps | Pengembangan skrip `ARPerformanceMonitor.cs`, proses *build* berkas `AR_Zonasi_Kelompok6.apk`, pengujian fungsionalitas *black-box*, pengukuran performa dan kuesioner SSQ pada Samsung Galaxy A35 5G, serta manajemen repository GitHub |

> **Catatan:** Penyusunan, penyuntingan, dan verifikasi akhir naskah jurnal dilakukan secara kolaboratif oleh seluruh anggota kelompok.

---

## Tools dan Spesifikasi Teknis

- **Game Engine:** Unity Editor 6.6 (`6000.6.9f1`) — Universal Render Pipeline (URP)
- **AR SDK:** Vuforia Engine (dengan *Image Target* dan *Device Pose Tracking* aktif)
- **Optimalisasi Aset 3D:** Blender dan `gltfpack` (CLI)
- **Bahasa Pemrograman:** C# (`ARPerformanceMonitor.cs` untuk pemantauan FPS, *frame time*, dan alokasi RAM secara *real-time*)
- **Konfigurasi Kompilasi Android:** OpenJDK `17.0.18+8`, Minimum API Level Android 10.0 (API Level 29), Scripting Backend `IL2CPP`, Target Architecture `ARM64`, JVM Heap `4096 MB`, dan Graphics API `OpenGLES3` (tanpa Vulkan)
- **Perangkat Uji:** Samsung Galaxy A35 5G (RAM 8 GB, Android 16)

---

## Daftar Model 3D dan Marker

Aplikasi ini menggunakan 6 *marker* unik berkontras tinggi yang telah diverifikasi pada Vuforia Target Manager untuk memanggil masing-masing model 3D kawasan zonasi:

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

- `/src` : Berisi kode sumber C# (`ARPerformanceMonitor.cs`) dan berkas *scene* Unity (`SampleScene.unity`).
- `/assets` : Berisi 6 berkas model 3D (`.glb`) hasil optimalisasi serta 6 berkas gambar *marker* (`.jpeg`).
- `/build` : Berisi berkas `link_apk.txt` yang memuat tautan Google Drive publik untuk mengunduh `AR_Zonasi_Kelompok6.apk` (~136 MB, melebihi batas unggah tunggal GitHub 100 MB).
- `/docs` : Berisi diagram arsitektur sistem, diagram alur kerja aplikasi, dan laporan/jurnal proyek (PDF).
- `/demo` : Berisi dokumentasi tangkapan layar pengujian pada perangkat Android dan tautan demonstrasi aplikasi.

---

## Arsitektur Sistem dan Alur Kerja Aplikasi

1. **Arsitektur Sistem Aplikasi AR Multi-Marker Optimalisasi Objek 3D**
   Arsitektur aplikasi dibagi menjadi **3 lapisan utama**:
   - **Lapisan 1 — Persiapan Aset & Marker (*Offline Pipeline*):** Audit skala, orientasi sumbu, dan *pivot* 6 model `.glb` di Blender, kompresi *mesh* & *node* menggunakan `gltfpack`, serta perancangan dan verifikasi *feature points* 6 *marker* `.jpeg` pada Vuforia Target Manager.
   - **Lapisan 2 — Engine AR & Kompilasi Android (Unity 6.6 URP + Vuforia Engine):** Perakitan 6 `ImageTarget` beserta *child* model `.glb` di `SampleScene.unity`, aktivasi *Device Pose Tracking*, integrasi `ARPerformanceMonitor.cs`, serta kompilasi *native* Android (`API 29`, `IL2CPP`, `ARM64`, `OpenGLES3`) menghasilkan `AR_Zonasi_Kelompok6.apk` (~136 MB).
   - **Lapisan 3 — Runtime & Evaluasi Android (Samsung Galaxy A35 5G):** Pemindaian 1 dari 6 *marker* pada jarak 20–40 cm dengan stabilisasi sensor IMU, *rendering* model 3D beserta *overlay* FPS, *frame time* (ms), dan RAM (MB), serta evaluasi performa (>=30 FPS), *black-box*, dan SSQ.

2. **Diagram Alur Kerja Aplikasi**
   - Membuka aplikasi `AR_Zonasi_Kelompok6.apk` dan memverifikasi izin kamera Android.
   - Inisialisasi Vuforia Engine (`ARCamera`, *Device Pose Tracking*, `OpenGLES3`) dan `ARPerformanceMonitor.cs`.
   - Siklus pemindaian kamera dan pencocokan 6 *marker* secara berulang.
   - Jika salah satu *marker* terdeteksi: *render* model 3D kawasan zonasi yang sesuai, kunci *pose* dengan *Device Pose Tracking*, dan perbarui *overlay* FPS/ms/RAM.
   - Jika *marker* tidak terdeteksi atau tertutup (*error handling*): sembunyikan objek 3D secara otomatis agar aplikasi tetap stabil hingga *marker* terlihat kembali.

---

## Ringkasan Hasil Pengujian (Samsung Galaxy A35 5G)

- **Waktu Deteksi Marker:** 0,8–1,1 detik untuk seluruh 6 *marker*.
- **Performa Frame Rate:** 60 FPS (16,7 ms) pada `Electical_Zone.glb`, `Gudang.glb`, `Kantor_dan_RS.glb`, dan `Industri1.glb`; 53 FPS (19,0 ms) pada `AgroIndustri.glb`; serta 30 FPS (33,3 ms) pada `Realestate.glb`.
- **Alokasi RAM:** 228,3–236,9 MB.
- **Pengujian Black-Box & SSQ:** Seluruh 5 skenario *black-box* berhasil tanpa *crash*, dengan rata-rata keluhan mata lelah 0,3, pusing/mual 0,0, gangguan objek bergoyang 0,0 (skala 0–3), dan skor kemudahan penggunaan 4,7 dari 5.

---

## Cara Menjalankan Aplikasi (Android)

1. Buka berkas `/build/link_apk.txt` dan unduh `AR_Zonasi_Kelompok6.apk` melalui tautan Google Drive yang tersedia.
2. Pindahkan berkas `.apk` ke perangkat Android (minimal Android 10.0 / API 29), lalu lakukan instalasi (izinkan *Install from Unknown Sources* jika diminta).
3. Buka aplikasi dan berikan izin akses **Kamera**.
4. Arahkan kamera ke salah satu dari 6 gambar *marker* di folder `/assets` pada jarak 20–40 cm dengan pencahayaan ruangan normal.
5. Model 3D kawasan zonasi akan tampil di atas *marker* beserta indikator performa (FPS, *frame time*, dan RAM) di pojok kiri atas layar.

---

## Cara Build dari Unity Editor

1. Pastikan **Unity Editor 6.6 (`6000.6.9f1`)** sudah terpasang beserta modul **Android Build Support** (`OpenJDK 17.0.18+8`, Android SDK & NDK Tools).
2. Buka proyek melalui Unity Hub, lalu buka *scene* utama di `Assets/Scenes/SampleScene.unity`.
3. Masuk ke menu **File > Build Profiles**, pilih **Android**, lalu klik **Switch Platform**.
4. Buka **Player Settings > Other Settings** dan pastikan pengaturan berikut sesuai:
   - **Auto Graphics API:** Nonaktifkan, hapus **Vulkan**, dan gunakan **OpenGLES3**.
   - **Minimum API Level:** Android 10.0 (API Level 29).
   - **Scripting Backend:** `IL2CPP`.
   - **Target Architectures:** Centang `ARM64`.
5. Klik **Build** untuk menghasilkan berkas `AR_Zonasi_Kelompok6.apk`.

---

## Sumber Aset dan Lisensi

- Seluruh model 3D kawasan zonasi (6 berkas `.glb`) bersumber dari repositori akademik mata kuliah Virtual Reality, Program Studi Teknik Informatika, Universitas Tadulako.
- *Library* AR menggunakan **Vuforia Engine** untuk keperluan akademik dan non-komersial.