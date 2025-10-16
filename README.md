<div align="center">

# 🕹️ Alien Escape 🕹️

</div>

A 2D infinite runner made to learn Unity's Object Pooling system.

[Play on itch.io](https://manuelggarcia.itch.io/alien-escape)

## 🚀 What I Implemented

- **Object Pooling**: Obstacles are recycled instead of constantly created/destroyed for better performance
- **Event-Driven Architecture**: Systems communicate through C# events (GameStarted, GameOver, ScoreChanged)
- **Interface-Based Design**: Used `IInputHandler` and `IScoreSystem` for decoupled components
- **New Input System**: Multiple input methods (mouse, keyboard, touch)
- **Component-Based Structure**: Each system is a separate, focused component
- **Persistent High Score**: Saves using PlayerPrefs

## 🛠️ Technical Stack

- Unity 6000.0.50f1
- C# with component-based architecture
- New Input System
- TextMeshPro

## 🎵 Credits

Music: [Lofi World Vol. 1 - 7 Free Music Tracks](https://assetstore.unity.com/packages/p/lofi-world-vol-1-7-free-music-tracks-214014)
