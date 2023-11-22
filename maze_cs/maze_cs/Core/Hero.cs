using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using projet_cs.Core;


namespace projet_cs.Core;

public class Hero : GameObject
{

    private Collision.Direction _collidedDirection; //Permet de changer de direction tout en étant en collision dans une autre
    public Collision.Direction collidedDirection
    {
        get { return _collidedDirection; } 
        set { _collidedDirection = value;}
    }

	public Hero(int totalAnimationFrames, int frameWidth, int frameHeight, Maze maze)
		: base(totalAnimationFrames, frameWidth, frameHeight, maze)
	{
		//position de départ du personnage
		direction = Collision.Direction.RIGHT;
        //frameIndex = framesIndex.RIGHT_1;
        _collidedDirection = Collision.Direction.NONE;
	}

	// Gestion du clavier
	public void Move(KeyboardState state) 
	{
		if (state.IsKeyDown(Keys.Up)) 
		{
			direction = Collision.Direction.UP;
			if (!Collision.Collided(this, maze))
            {
                if (collidedDirection != Collision.Direction.UP)
                {
                    collidedDirection = Collision.Direction.NONE;
                    Position.Y -= 1;
                }
            }
		}

        if (state.IsKeyDown(Keys.Down))
        {
            direction = Collision.Direction.DOWN;
            if (!Collision.Collided(this, maze))
            {
                if(collidedDirection != Collision.Direction.DOWN)
                {
                    collidedDirection = Collision.Direction.NONE;
                    Position.Y += 1;
                }
            }
        }

        if (state.IsKeyDown(Keys.Left))
        {
            direction = Collision.Direction.LEFT;
            if (!Collision.Collided(this, maze))
            {
                if (collidedDirection != Collision.Direction.LEFT)
                {
                    collidedDirection = Collision.Direction.NONE;
                    Position.X -= 1;
                }
            }
            
        }

        // Si Collided renvoie false, y a pas de collision
        //CollidedDirection sert à detecter si la touche presser est différente par rapport 
        // à la direction où on a realisé notre récente collision
        //Si la touche est différente, on passe notre variable de collision à NONE puis on incremente notre position X
        // vu qu'on se dirige vers la droite
        if (state.IsKeyDown(Keys.Right))
        {
            direction = Collision.Direction.RIGHT;
            if (!Collision.Collided(this, maze))
            {
                if (collidedDirection != Collision.Direction.RIGHT)
                {
                    collidedDirection = Collision.Direction.NONE;
                    Position.X += 1;
                }
            }
        }
    }
}