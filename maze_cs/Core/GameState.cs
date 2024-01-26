using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

[Serializable]
public class GameState
{
    public int PlayerLives { get; set; }
    public float GameTimeRemaining { get; set; }
    public int HeroScore { get; set; }
    public bool GameStarted { get; set; }
    public Vector2 HeroPosition { get; set; }
}

