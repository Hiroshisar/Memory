# 🧠 Memory Game

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![MAUI](https://img.shields.io/badge/MAUI-Cross%20Platform-512BD4?logo=visualstudio&logoColor=white)](https://learn.microsoft.com/dotnet/maui/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Android%20%7C%20iOS-blue)]()
[![License](https://img.shields.io/badge/License-Non--Commercial-orange)](LICENSE)

A simple **Memory Game** built with **.NET MAUI**.  
Flip the cards, find the pairs, and challenge your memory! 🃏

---

## 🎮 Features
- Dynamic **4x4 grid** with 16 cards.
- Cards are initially hidden → tap to reveal.
- **Automatic pair check**:
  - ✔️ If they match → they stay visible.
  - ❌ If not → they flip back after a short delay.
- Scoring system:
  - `Score` increases when you find a pair.
  - `Tries` counts the number of attempts.
- **Victory message** at the end of the game.
- **Reset button** to start over.

---

## ⚙️ Requirements
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **.NET MAUI** workload installed.
- **.NET 8.0** or higher.
- Supported platforms:
  - 🖥️ Windows 10/11 (WinUI 3)
  - 📱 Android / iOS (optional, for mobile testing)

---

## 🚀 Getting Started
Clone the repository and run the game:

```bash
git clone https://github.com/Hiroshisar/Memory.git
cd Memory
```

Open the solution in Visual Studio 2022, choose your target platform, and press F5.

---

📂 Project Structure

```bash
Memory/
 ├── Resources/
 │   ├── Images/        # Card images (front and back)
 │   ├── Fonts/         # Optional fonts
 ├── MainPage.xaml      # Main page layout
 ├── MainPage.xaml.cs   # Game logic
 ├── App.xaml           # Global settings
 └── README.md          # Documentation
```

---

📝 Notes
Images must be placed in Resources/Images.
File names must be lowercase, without spaces, and with valid characters (a-z, 0-9, _).
To add new cards, simply place new PNG files in the folder and update the references in the code.

---

📖 License
This project is released under a Custom Non-Commercial License (based on MIT). ✅ Free for personal, educational, and non-commercial use ❌ Commercial use, resale, or redistribution for profit is strictly prohibited
See the LICENSE file for details.
