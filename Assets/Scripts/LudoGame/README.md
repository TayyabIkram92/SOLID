# Unity 2D Ludo Game

## Overview
This is a 2D Ludo game implementation in Unity that demonstrates SOLID principles in game development. The game features 1 human player and 3 AI opponents with varying difficulty levels.

## SOLID Principles Implementation

### Single Responsibility Principle (S)
Each class has a single responsibility:
- `GameManager`: Manages game state and turn flow
- `BoardManager`: Handles board layout and position tracking
- `DiceController`: Controls dice rolling logic
- `TokenManager`: Manages token creation and state
- `UIManager`: Handles UI elements and interactions

### Open/Closed Principle (O)
The code is designed to be extended without modification:
- Abstract `Player` class can be extended for different player types
- `AIStrategy` pattern allows adding new AI difficulties without changing existing code

### Liskov Substitution Principle (L)
Derived classes can replace base classes:
- All `Player` implementations work interchangeably in the game system
- Different `AIStrategy` implementations can be swapped without breaking functionality

### Interface Segregation Principle (I)
Focused interfaces prevent classes from implementing unnecessary methods:
- `IDiceRoller`: Dice-specific functionality
- `IBoardRenderer`: Board rendering operations
- `IGameRules`: Game rule validations and queries
- `ITimerSystem`: Timer management
- `IAudioSystem`: Audio playback

### Dependency Inversion Principle (D)
High-level modules depend on abstractions:
- `GameManager` depends on interfaces like `IDiceRoller`, `IGameRules`, etc.
- Components are injected through Unity Inspector or constructor injection

## Project Structure

```
Scripts/
├── Core/
│   ├── GameManager.cs
│   ├── GameState.cs
│   └── GameSettings.cs
├── Board/
│   ├── BoardManager.cs
│   ├── BoardRenderer.cs
│   ├── IBoardRenderer.cs
│   └── BoardData.cs
├── Players/
│   ├── Player.cs (abstract)
│   ├── HumanPlayer.cs
│   └── AIPlayer.cs
├── AI/
│   ├── AIStrategy.cs (abstract)
│   ├── EasyAIStrategy.cs
│   ├── MediumAIStrategy.cs
│   └── HardAIStrategy.cs
├── Tokens/
│   ├── Token.cs
│   ├── TokenManager.cs
│   └── TokenColor.cs (enum)
├── Dice/
│   ├── IDiceRoller.cs
│   └── DiceController.cs
├── Rules/
│   ├── IGameRules.cs
│   ├── LudoRules.cs
│   ├── MoveValidator.cs
│   └── Move.cs
├── Systems/
│   ├── ITimerSystem.cs
│   ├── TimerSystem.cs
│   ├── IAudioSystem.cs
│   └── AudioSystem.cs
└── UI/
    ├── UIManager.cs
    ├── GameUI.cs
    └── VictoryScreen.cs
```

## Game Rules

### Movement Rules
- Roll 6 to bring a token from home to starting position
- Move tokens clockwise by dice value
- Rolling a 6 gives an extra turn (max 3 consecutive 6s)
- Landing on an opponent's single token sends it home
- Safe positions protect tokens from being killed
- 2+ tokens on the same position cannot be killed
- First player to get all 4 tokens home wins

### AI Difficulties
- **Easy AI**: Makes random moves
- **Medium AI**: Prioritizes killing opponents and bringing tokens out
- **Hard AI**: Makes strategic decisions, including blocking opponents

## Setup Instructions

1. Import the scripts into your Unity project
2. Create empty GameObjects for each manager:
   - GameManager
   - BoardManager
   - DiceController
   - TokenManager
   - UIManager
3. Attach the corresponding scripts to each GameObject
4. Create UI elements (buttons, text) and link them in the Inspector
5. Create ScriptableObjects for BoardData and GameSettings
6. Set up the references between components in the Inspector

## Extensibility

This implementation is designed to be easily extended:

- Add new AI difficulties by creating new AIStrategy classes
- Implement different board layouts by modifying BoardData
- Create new game rule variants by extending IGameRules
