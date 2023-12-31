using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using maze_cs.Core;


namespace maze_cs.Core;

public class Maze : GameObject
{
    public Color[] colorTab;

    private Color _collisionColor;
    public Color collisionColor
    {
        get { return _collisionColor; }
    }

    public Maze(Color collisionColor) 
    {
        _collisionColor = collisionColor;
    }
}