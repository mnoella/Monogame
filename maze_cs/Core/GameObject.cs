using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace maze_cs.Core;

public class GameObject
{

    public Maze maze;

    public Vector2 Position;
    public Texture2D Texture;
    // Rectangle permettant de d�finir la zone de l'image � afficher
    public Rectangle Source;

    // Dur�e depuis laquelle l'image est � l'�cran
    public float Time;
    // Dur�e de visibilit� d'une image
    public const float FrameTime = 0.1f;
    // Indice de l'image en cours
    public int FrameIndex;



    public Collision.Direction TheDirection;

    // Afin de pouvoir r�cup�rer/fournir les valeurs actuelles des propri�t�, on impl�mente des methodes get
    private int _totalFrames;
    public int TotalFrames
    {
        get { return _totalFrames; } 
    }
    private int _frameWidth;
    public int FrameWidth
    {
        get { return _frameWidth; }
    }
    private int _frameHeight;
    public int FrameHeight
    {
        get { return _frameHeight; }
    }

    public GameObject() 
    {

    }
    
    // Nouveau constructeur afin de pouvoir initialiser les propriétés
    public GameObject(int totalAnimationFrames, int frameWidth, int frameHeight, Maze maze)
    {
        _totalFrames = totalAnimationFrames;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        this.maze = maze;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
 
    }

    public void DrawAnimation(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Source, Color.White);
    }

    
    //
    public void UpdateFrame(GameTime gameTime)
    {
        // Calcul de la position du nouveau sprite � afficher en d�terminant sa position par rapport � l'indice en cours  
        this.Source = new Rectangle(
            0,
            0,
            _frameWidth,
            _frameHeight);
    }
}