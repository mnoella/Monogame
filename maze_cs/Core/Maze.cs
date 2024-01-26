using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using maze_cs.Core;
using Microsoft.Xna.Framework.Content;

namespace maze_cs.Core;

public class Maze : GameObject
{
    private ContentManager _content;

    public Color[] ColorTab { get; set; }

    private Color _collisionColor;
    public Color CollisionColor
    {
        get { return _collisionColor; }
    }

    private Vector2 _exitPosition;
    public Vector2 ExitPosition
    {
        get { return _exitPosition; }
        set { _exitPosition = value; }
    }

    
    private Texture2D _exitFlagTexture;
    public Texture2D ExitFlagTexture
    {
        get { return _exitFlagTexture; }
        set { _exitFlagTexture = value; }
    }

    public Maze(Color collisionColor, ContentManager _content) 
    {
        _collisionColor = collisionColor;
        this._content = _content;
    }

    public void setExitPosition(Vector2 position)
    {
        _exitPosition = position;
    }

    public bool IsCollisionAtPoint(int x, int y)
    {
        if (x < 0 || x >= Texture.Width || y < 0 || y >= Texture.Height)
        {
            return true;
        }

        return ColorTab[y * Texture.Width + x] == _collisionColor;
    }

    public void Resize(int newWidth, int newHeight)
    {
        Texture = _content.Load<Texture2D>("maze"); 
        ColorTab = new Color[Texture.Width * Texture.Height];

        Texture.GetData<Color>(ColorTab);
        
    }


}