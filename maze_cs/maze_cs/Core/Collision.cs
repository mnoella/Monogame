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
		UP = 2,
		DOWN = 3
	}

	// Récupère la couleur d'un pixel à une certaine position
	private static Color GetColorAt(GameObject gameObject, Maze maze)
	{
		Color color = maze.collisionColor;

		if ((int)gameObject.Position.X >= 0 && (int)gameObject.Position.X < maze.Texture.Width && (int)gameObject.Position.Y >= 0 && (int)gameObject.Position.Y < maze.Texture.Height)
		{
			switch (gameObject.direction)
			{
				case Direction.RIGHT:
				{
					color = maze.colorTab[((int)gameObject.Position.X + gameObject.frameWidth) + ((int)gameObject.Position.Y + (gameObject.frameHeight / 2)) * maze.Texture.Width];
				}
				break;
				case Direction.LEFT:
				{
					color = maze.colorTab[(int)gameObject.Position.X + ((int)gameObject.Position.Y + (gameObject.frameHeight / 2)) * maze.Texture.Width];
				}
				break;
				case Direction.DOWN:
				{
					color = maze.colorTab[((int)gameObject.Position.X + (gameObject.frameWidth / 2)) + ((int)gameObject.Position.Y + gameObject.frameHeight)  * maze.Texture.Width];
				}
				break;
				case Direction.UP:
				{
					color = maze.colorTab[((int)gameObject.Position.X + (gameObject.frameWidth / 2)) + (int)gameObject.Position.Y * maze.Texture.Width];
				}
				break;

			}
		}
		return color;
	}

	// Si la couleur est différente d ela couleur de collision enregistrée dans le labyrinthe, il n'y a pas de collision
	// On renvoie alors une valeur booléenne, false si on ne touch epas, true si on entre en collision avec le mur
	public static bool Collided(GameObject gameObject, Maze maze)
	{
		bool b = false;
		Color color = GetColorAt(gameObject, maze);

		if (color != maze.collisionColor)
		{
			b = false;
		}
		else
			b = true;
		return b;
	}
}