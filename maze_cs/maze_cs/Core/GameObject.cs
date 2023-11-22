using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace projet_cs.Core;

public class GameObject
{

    public Vector2 Position;
    public Texture2D Texture;
    // Rectangle permettant de définir la zone de l'image à afficher
    public Rectangle Source;

    // Durée depuis laquelle l'image est à l'écran
    public float time;
    // Durée de visibilité d'une image
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

    // Afin de pouvoir récupérer/fournir les valeurs actuelles des propriété, on implémente des methodes get
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
        //Permet de calculer le temps passé depuis notre dernière mis à jour de l'afichage de notre sprite
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Si on a dépassé le temps d'affichage, on passe au prochain indice de notre image
        while (time > frameTime)
        {
            //frameIndex++;
            time = 0f; // réinitialisation nécessaire pour le prochain mis à jour
        }

        //Si indice dépasse le nombre de sprites dans notre collection, on repasse au premier
        //if (frameIndex > _totalFrames)
            //frameIndex = 0;

        // Calcul de la position du nouveau sprite à afficher en déterminant sa position par rapport à l'indice en cours  
        Source = new Rectangle(
            0,
            0,
            frameWidth,
            frameHeight);
    }
}