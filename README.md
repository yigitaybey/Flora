# 🌿 Flora: The Earth Wants It Back

[![Unity](https://img.shields.io/badge/Unity-6000.0-black?logo=unity)](https://unity.com/)
[![RevenueCat](https://img.shields.io/badge/RevenueCat-Shipaton_2026-e74c3c?logo=revenuecat)](https://www.revenuecat.com/)
[![Platform](https://img.shields.io/badge/Platform-Android_%7C_iOS-green?logo=android)](https://play.google.com/store/apps/details?id=com.budabigame.flora)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

> *"In a ruined world where nature evolved to hunt back, a lone botanist uses the earth's own ancient seeds not to destroy, but to heal."*

**Flora: The Earth Wants It Back** is an isometric 3D low-poly Roguelite Bullet Haven + Base Reclamation game built for **RevenueCat Shipaton 2026**.

---

## 🎮 Play & Downloads
* 📱 **Google Play Store:** [Download on Google Play](https://play.google.com/store/apps/details?id=com.budabigame.flora)
* 📦 **Direct APK Download:** [GitHub Releases v1.0](https://github.com/yigitaybey/Flora/releases)
* 🌐 **Devpost Project Submission:** [Flora on Devpost](https://devpost.com/)

---

## 📖 Story & Lore: The Chlorophyll Awakening

In **2041**, atmospheric greenhouse gases crossed an irreversible tipping point. The oceans warmed, forests turned into net carbon emitters, and sunlight was choked out by a toxic yellowish-green haze. 

By **2047**, extreme evolutionary pressure triggered the **"Chlorophyll Awakening"**: plant species mutated into predatory organisms capable of rapid motor locomotion and predatory hunting. Conventional firearms were useless—bullets cannot pierce raw chlorophyll or cleanse toxic spores.

Enter **Sylva**, an eco-resilient botanist surviving at an abandoned radio tower outpost. Equipped with **Pure Seeds** preserved from pre-crisis seed vaults, Sylva ventures into contaminated wilderness zones. Injecting mutated flora with pure botanical DNA neutralizes their aggression, restoring nature and transforming poisoned wastelands back into blooming green ecosystems.

---

## ⚔️ Key Gameplay Features

* 🌾 **20-Wave Survival Gauntlet:** Face dynamic swarms of mutated flora and fauna (*Moss, SporeHead, Ivy, Wolfey*) powered by zero-alloc NavMesh pathfinding.
* 🌲 **Colossal Boss Encounters:** Battle **Chinar (The Ent)** in an epic arena showdown requiring strategic positioning and weapon mastery.
* 🔫 **Dynamic 4-Weapon Arsenal:**
  * **Pollen Injector:** Rapid-fire single-target botanical darts.
  * **Flamethrower:** Conical high-temperature area denial.
  * **Orbiting Flying Axe:** Rotating perimeter blade defense.
  * **UV Lamp:** Continuous holy-purple aura dealing damage-over-time with slow effects.
* 🃏 **RNG Upgrade Deck:** Harvest dropped seeds to level up and choose from 3 randomized upgrade cards with custom 3D-rendered icon assets (`UpgradeManager.cs`).
* 🌅 **Visual Terraforming Payoff:** Clearing a contaminated zone instantly purges the toxic yellow-green smog, replacing it with a radiant blue sky and lush vegetation.

---

## 💰 RevenueCat Shipaton 2026 Integration

Flora is designed specifically around RevenueCat's hackathon tracks, combining meaningful social impact with ethical player monetization:

### 🕊️ Peace Prize Category: "Plant a Tree" IAP
* Integrated via `RevenueCatManager.cs` using the official `com.revenuecat.purchases-unity` SDK.
* Players can purchase the **"Plant a Tree" package ($2.99)** to fund real-world reforestation efforts while earning in-game Core Seeds.
* Every purchase directly supports planting certified saplings in real-world degraded habitats.

### 🐱 Catvertising Category: Ethical Rewarded Telemetry
* Zero forced, interruptive pop-up ads!
* When players finish a run (Victory or Defeat), they can voluntarily watch an opt-in **"Double Your Harvest"** rewarded video (`AdManager.cs`).
* Each ad impression is bridged in real-time to RevenueCat's `AdTracker.TrackAdRevenue` API (`purchases.AdTracker.TrackAdRevenue`), consolidating subscription, IAP, and ad monetization telemetry in one dashboard.

### 🛡️ Judge-Friendly Simulation Mode
* The game includes an automatic fallback and simulation mode in `RevenueCatManager.cs` and `AdManager.cs`, allowing hackathon judges to test all store packages, tree-planting flows, and 2x loot rewarded cycles seamlessly without needing credit cards or getting stuck offline.

---

## 🛠️ Tech Stack & Mobile Optimization

| Layer | Technology |
|---|---|
| **Engine** | Unity 6 (6000.0.x) |
| **Language** | C# (.NET Standard 2.1) |
| **Monetization** | RevenueCat SDK (`com.revenuecat.purchases-unity: 9.11.1`) |
| **Ad Network** | Unity Ads (`com.unity.ads: 4.4.2`) |
| **3D Art & Assets** | Custom Low-Poly 3D models in Blender |
| **Performance** | Object Pooling (`EnemyPool`, `ProjectilePool`, `SeedPool`), `sqrMagnitude` math |
| **Compression** | LZ4HC Android App Bundle (`.aab`) packaging (<190 MB threshold) |

---

## 👥 The Flora Team

* **Yiğit Aybey** — Engine Integrator & Project Lead (Unity architecture, UI implementation, Play Store build & deployment)
* **Yusuf Özbakır** — Game & Art Director (Game design, narrative lore, enemy mechanics, and audio)
* **Yusuf Yüksekbağ** — 3D & 2D Artist (Low-poly character & environment models, textures, UI design)
* **Antigravity AI** — Senior Unity & RevenueCat Developer (C# software architecture, optimization, and RevenueCat integration)

---

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
