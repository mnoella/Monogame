using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using maze_cs.Core;

namespace maze_cs;

public class Game1 : Game
{   
    
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    public const int WINDOW_WIDTH = 384;
    public const int WINDOW_HEIGHT = 384;

    Maze maze;
    Hero hero;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        

        graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        maze = new Maze(new Color(0, 128, 248));
        hero = new Hero(0, 14, 17, maze);
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        maze.Texture = Content.Load<Texture2D>("maze");
        maze.Position = new Vector2(0,0);

        hero.Texture = Content.Load<Texture2D>("hero");
        hero.Position = new Vector2(30, 30);
        
        // Creation du tableau déclaré dans la classe Maze, en tableau 2D
        maze.colorTab = new Color[maze.Texture.Width * maze.Texture.Height];
        // Initialise le tableau grace a la methode GetData de la classe Texture
        // Elle va récupérer les infos de chaque pixel et les stocker à l'endroit adéquat => Detaction de collision
        maze.Texture.GetData<Color>(maze.colorTab);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

        hero.Move(Keyboard.GetState());
        hero.UpdateFrame(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin();
            maze.Draw(spriteBatch);
            hero.DrawAnimation(spriteBatch);
        spriteBatch.End();
        
        base.Draw(gameTime);
    }
}