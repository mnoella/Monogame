using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using maze_cs.Core;


namespace maze_cs.Core;

public static class Collision
{

	public enum Direction
	{	
		NONE = -1,
		LEFT = 0,
		RIGHT = 1,
		TOP = 2,
		BOTTOM = 3
	}

	
	
	//Determine si le hero est en collision, dans ce cas il faut changer d'orientation
	// Si la couleur est diff�rente d ela couleur de collision enregistr�e dans le labyrinthe, il n'y a pas de collision
	// On renvoie alors une valeur bool�enne, false si on ne touch epas, true si on entre en collision avec le mur
	public static bool Collided(GameObject gameObject, Maze maze)
	{
        return IsCollisionAtPoint(gameObject, maze);
    }


    private static bool IsCollisionAtPoint(GameObject gameObject, Maze maze)
    {
        Vector2 newPosition = gameObject.Position;

        // V�rifier les collisions avec les bords de la fen�tre d'ex�cution
        if (newPosition.X < 0 || newPosition.Y < 0 || newPosition.X + gameObject.FrameWidth > Game1.WindowWidth || newPosition.Y + gameObject.FrameHeight > Game1.WindowHeight)
        {
            return true;
        }

        // V�rifier les collisions avec le labyrinthe
        int x = (int)newPosition.X;
        int y = (int)newPosition.Y;

        for (int i = 0; i < gameObject.FrameWidth; i++)
        {
            for (int j = 0; j < gameObject.FrameHeight; j++)
            {
                if (maze.IsCollisionAtPoint(x + i, y + j))
                {
                    return true;
                }
            }
        }

        return false;
    }
}