using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using maze_cs.Core;


namespace maze_cs.Core;

public class Hero : GameObject
{

    private Collision.Direction _collidedTheDirection; //Permet de changer de TheDirection tout en étant en collision dans une autre
    public Collision.Direction collidedTheDirection
    {
        get { return _collidedTheDirection; } 
        set { _collidedTheDirection = value;}
    }

	public Hero(int totalAnimationFrames, int FrameWidth, int FrameHeight, Maze maze)
		: base(totalAnimationFrames, FrameWidth, FrameHeight, maze)
	{
		//position de départ du personnage
		TheDirection = Collision.Direction.RIGHT;
        _collidedTheDirection = Collision.Direction.NONE;
	}

	// Gestion du clavier
	public void Move(KeyboardState state) 
	{
        Vector2 newPosition = Position;

        if (state.IsKeyDown(Keys.Up))
        {
            newPosition.Y -= 2;
        }

        if (state.IsKeyDown(Keys.Down))
        {
            newPosition.Y += 2;
        }

        if (state.IsKeyDown(Keys.Left))
        {
            newPosition.X -= 2;
        }

        if (state.IsKeyDown(Keys.Right))
        {
            newPosition.X += 2;
        }

        // V�rifier les collisions
        if (!CollisionWithMaze(newPosition, maze))
        {
            Position = newPosition;
        }
    }


    private bool CollisionWithMaze(Vector2 newPosition, Maze maze)
    {
    // Creer un rectangle representant la nouvelle position du h�ros
    Rectangle newRect = new Rectangle((int)newPosition.X, (int)newPosition.Y, FrameWidth, FrameHeight);

    // Vérifier les collisions avec les bords de la fenetre d'execution
    if (!newRect.Intersects(new Rectangle(0, 0, Game1.WindowWidth, Game1.WindowHeight)))
    {
        return true;
    }

    // Verifier les collisions avec le labyrinthe
    for (int i = 0; i < maze.Texture.Width; i++)
    {
        for (int j = 0; j < maze.Texture.Height; j++)
        {
            if (maze.ColorTab[j * maze.Texture.Width + i] != Color.White) // Considerer seulement les pixels non blancs comme des murs
            {
                Rectangle mazeRect = new Rectangle(i, j, 1, 1); // Rectangle représentant le mur dans le labyrinthe

                // Verifier la collision entre le rectangle du heros et le rectangle du mur
                if (newRect.Intersects(mazeRect))
                {
                    return true;
                }
            }
        }
    }

    return false;
    }
}