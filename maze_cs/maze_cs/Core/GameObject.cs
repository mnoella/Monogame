using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace projet_cs.Core;

public class GameObject
{

    public Vector2 Position;
    public Texture2D Texture;
    // Rectangle permettant de d�finir la zone de l'image � afficher
    public Rectangle Source;

    // Dur�e depuis laquelle l'image est � l'�cran
    public float time;
    // Dur�e de visibilit� d'une image
    public float frameTime = 0.1f;
    // Indice de l'image en cours
    public int frameIndex;

    public Maze maze;

    /**
    public enum framesIndex
    {
        RIGHT_1 = 0,
        RIGHT_2 = 1,
        DOWN_1 = 2,
        DOWN_2 = 3,
        LEFT_1 = 4,
        LEFT_2 = 5,
        UP_1 = 6,
        UP_2 = 7
    }
    */

    /**
    public enum Direction
    {
        LEFT = 0,
        RIGHT = 1,
        UP = 2, 
        DOWN = 3
    }*/

    //public Direction direction;

    public Collision.Direction direction;

    // Afin de pouvoir r�cup�rer/fournir les valeurs actuelles des propri�t�, on impl�mente des methodes get
    private int _totalFrames;
    public int totalFrames
    {
        get { return _totalFrames; } 
    }
    private int _frameWidth;
    public int frameWidth
    {
        get { return _frameWidth; }
    }
    private int _frameHeight;
    public int frameHeight
    {
        get { return _frameHeight; }
    }

    public GameObject() 
    {

    }
    
    // Nouveau constructeur afin de pouvoir initialiser les propri�t�s
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
        //Permet de calculer le temps pass� depuis notre derni�re mis � jour de l'afichage de notre sprite
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Si on a d�pass� le temps d'affichage, on passe au prochain indice de notre image
        while (time > frameTime)
        {
            //frameIndex++;
            time = 0f; // r�initialisation n�cessaire pour le prochain mis � jour
        }

        //Si indice d�passe le nombre de sprites dans notre collection, on repasse au premier
        //if (frameIndex > _totalFrames)
            //frameIndex = 0;

        // Calcul de la position du nouveau sprite � afficher en d�terminant sa position par rapport � l'indice en cours  
        Source = new Rectangle(
            0,
            0,
            frameWidth,
            frameHeight);
    }
}