# Skripsie: Augmented Reality Annotation System for RFID Visualization

![Unity Version](https://img.shields.io/badge/Unity-2021.3.1f1-blue?logo=unity)
![.NET Version](https://img.shields.io/badge/.NET-6.0-blueviolet?logo=dotnet)

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Repository Structure](#repository-structure)
- [Contact](#contact)

## Overview

For my final year project (skripsie) at the [Department of Mechanical and Mechatronic Engineering at Stellenbosch University](https://www.mecheng.sun.ac.za), I created an augmented reality annotation system designed for the visualization of RFID data within a Smart Conveyor setup at the [Automation Laboratory](https://sites.google.com/view/mad-research-group/facilities/automation-lab?authuser=0). The system leverages [Unity3D](https://unity.com) and a "digital twin" communication abstraction layer to retrieve and overlay real-time RFID information directly onto physical conveyor objects, enhancing monitoring, diagnostics, and educational experiences.

## Features

- **Real-Time RFID Data Visualization:** Displays RFID tag data spatially in AR.
- **Augmented Reality Overlay:** Uses the Unity3D game development environment for immersive annotation.
- **Smart Conveyor Integration:** Tailored for integration with the [Automation Laboratory](https://sites.google.com/view/mad-research-group/facilities/automation-lab?authuser=0) environment of Stellenbosch University.
- **Customizable Visuals:** Easily adapt annotation styles and data presentation.
- **Modular C# Codebase:** Clean architecture for future expansion or integration.

## Technologies Used

- **[Unity3D](https://unity.com)** (C# scripting)
- **RFID hardware integration**
- **Siemens PLC** (Mechatronic system of the Smart Conveyor)
- **Digital Twin TCP/IP Communication Abstraction Layer** (Between conveyor PLC and AR system)
- **AR SDK: AR Foundation**
- **.NET 6.0** (for supporting C# projects)

## Getting Started

### Prerequisites

- Unity3D (recommend tested version, 2021.3.1f1)
- .NET 6.0 SDK
- Compatible RFID reader hardware and drivers
- AR-capable device (if deploying to mobile/headset)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/RobinAdam747/Skripsie.git
   ```
2. **Open in Unity:**
   - Open Unity Hub, select 'Add', and choose the cloned folder.
3. **Install dependencies:**
   - Import required Unity packages (e.g., AR Foundation, URP).
   - Install hardware SDKs as needed.
4. **Configure RFID Hardware:**
   - The hardware used in this project was from Bosch Rexroth, as well as a Siemens PLC
5. **Build and Run:**
   - Select target platform (PC, Android, HoloLens, etc.)
   - Press 'Play' in Unity, or build for deployment.

## Usage

- **Running in Editor:** Press Play; simulated or live RFID data will appear as AR annotations in the Unity scene.
- **Deploying to Device:** Build and install on supported AR device. Point at conveyor to see annotations.
- **Customizing Annotations:** Edit shader scripts in `/Assets/Shaders` and C# logic in `/Assets/Scripts`.

## Repository Structure

```
/Assets
  /Scripts      # C# source code
  /Shaders      # ShaderLab/HLSL shaders
  /Scenes       # Unity scenes
  /Prefabs      # Reusable objects
/Docs           # Documentation
...
```

## Contact

- **Maintainer:** [Adam](mailto:adam.sendzul@gmail.com)
- **Lab:** [MAD Research Group](https://sites.google.com/view/mad-research-group)
