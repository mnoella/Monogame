using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using maze_cs.Core;

namespace maze_cs
{

    public class Game1 : Game
    {


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch; 

        public const int WindowWidth = 900;
        public const int WindowHeight = 600;

        private bool _gameStarted = false;
        private Texture2D _backgroundTexture;
        private Song _backgroundMusic;
        private SpriteFont _font;

        //private string _saveFilePath = "gameState.txt";

        Maze maze;
        Hero hero;

        List<Monsters> monsters = new List<Monsters>();
        Random random = new Random();

        public int PlayerLives = 3;
        private Texture2D _lifeTexture;
        private Texture2D _gameOverTexture;

        private Rectangle _yesButton = new Rectangle(600, 500, 100, 50);
        private Rectangle _noButton = new Rectangle(600, 500, 100, 50);

        public const float TotalTime = 60f;
        private float _gameTimeRemaining;

        private  float _scoreTimer = 0f;
        public const float ScoreInterval = 10f;

        private int _bestScore = 0;
        private int _heroScore = 0;
        private int _finalScore = 0;

        private bool _gameOver = false;
        private int _savedBestScore;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;

            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            maze = new Maze(new Color(0, 128, 248), Content);
            hero = new Hero(1, 10, 10, maze);

            _gameTimeRemaining = TotalTime;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTexture = Content.Load<Texture2D>("bg");
            _backgroundMusic = Content.Load<Song>("stranger-things-124008");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic);
            _font = Content.Load<SpriteFont>("Roboto");

            maze.Texture = Content.Load<Texture2D>("maze");
            maze.Position = new Vector2(0, 0);
            maze.ColorTab = new Color[maze.Texture.Width * maze.Texture.Height];
            maze.Texture.GetData<Color>(maze.ColorTab);

            hero.Texture = Content.Load<Texture2D>("hero");
            hero.Position = new Vector2(5, 50);

            _gameOverTexture = Content.Load<Texture2D>("GameOver");

            maze.ExitFlagTexture = Content.Load<Texture2D>("flag");
            maze.ExitPosition = new Vector2(578, 570);

            _lifeTexture = Content.Load<Texture2D>("lives");

            List<Texture2D> monsterTextures = new List<Texture2D>
            {
                Content.Load<Texture2D>("Bmonster"),
                Content.Load<Texture2D>("Rmonster"),
                Content.Load<Texture2D>("Gmonster"),
                Content.Load<Texture2D>("Ymonster")
            };

            int pathWidth = 33;

            for (int i = 0; i < 10; i++)
            {
                float monsterSize = 18;
                Vector2 monsterPosition = GetRandomPathPosition(pathWidth, monsterSize);
                monsters.Add(new Monsters(monsterTextures[i % monsterTextures.Count], monsterPosition, new Vector2(monsterSize, monsterSize)));
            }

            //Lire le meilleur score depuis le fichier texte bestScore.txt
            _savedBestScore = ReadBestScore();

            IsMouseVisible = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //float pathWidth = 33;
            Rectangle heroRect = new Rectangle((int)hero.Position.X, (int)hero.Position.Y, hero.FrameWidth, hero.FrameHeight);

            if (!_gameOver)
            {
                _gameTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Bonus en fonction du temps
            float timeBonus = 0;
            const float interval = 20f;
            float elapsedTime = TotalTime - _gameTimeRemaining;

            if (elapsedTime >= interval)
            {
                timeBonus += ((int)(elapsedTime / interval) - (int)((elapsedTime - gameTime.ElapsedGameTime.TotalSeconds) / interval));
            }

            _heroScore += 3 * (int)timeBonus;


            if (!_gameStarted)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    _gameStarted = true;
                }
            }
            else
            {
                maze.Resize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                hero.Move(Keyboard.GetState());
                hero.UpdateFrame(gameTime);

                foreach (Monsters monster1 in monsters)
                {
                    foreach (Monsters monster2 in monsters)
                    {
                        if (monster1 != monster2)
                        {
                            float distance = Vector2.Distance(monster1.Position, monster2.Position);
                            float minDistance = 50.0f;

                            if (distance < minDistance)
                            {
                                Vector2 moveVector = Vector2.Normalize(monster1.Position - monster2.Position) * (minDistance - distance) / 2;
                                monster1.Position += moveVector;
                                monster2.Position -= moveVector;
                            }
                        }
                    }

                    monster1.Position = Vector2.Clamp(monster1.Position, Vector2.Zero, new Vector2(maze.Texture.Width - monster1.Size.X, maze.Texture.Height - monster1.Size.Y));
                }

                foreach (Monsters monster in monsters)
                {
                    Vector2 currentPosition = monster.Position;
                    currentPosition.Y += (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 0.02f;
                    currentPosition.Y = MathHelper.Clamp(currentPosition.Y, 0, maze.Texture.Height - monster.Size.Y);
                    monster.Position = currentPosition;

                    Rectangle monsterRect = new Rectangle((int)monster.Position.X, (int)monster.Position.Y, (int)monster.Size.X, (int)monster.Size.Y);

                    if (heroRect.Intersects(monsterRect))
                    {
                        PlayerLives--;
                        hero.Position = new Vector2(5, 50);
                    }
                }

                _gameTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_gameTimeRemaining <= 0)
                {
                    if (_heroScore > _bestScore)
                    {
                        _bestScore = _heroScore; 
                        WriteBestScore(_bestScore);
                    }
                    
                    
                }
            }

            if(_gameStarted && !_gameOver)
            {
                _scoreTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_scoreTimer >= ScoreInterval)
                {
                    // Augmente le score de 3 points toutes les 10 secondes
                    _heroScore += 3;
                    _scoreTimer = 0f; // Réinitialise le timer
                }
            }

            base.Update(gameTime);
        }

        

        private void DisplayLives()
        {
            if(PlayerLives > 0)
            {
                const int heartSpacing = 30;
                Vector2 heartPosition = new Vector2(WindowWidth - 150, 80);

                for (int i = 0; i < PlayerLives; i++)
                {
                    _spriteBatch.Draw(_lifeTexture, new Rectangle((int)heartPosition.X + i * heartSpacing, (int)heartPosition.Y, 25, 25), Color.White);
                }

                _spriteBatch.DrawString(_font, $"Lives", new Vector2(750, 60), Color.White);
            }
            
        }

        private Vector2 GetRandomPathPosition(int corridorWidth, float monsterSize)
        {
            float randomX, randomY;

            do
            {
                randomX = random.Next(0, maze.Texture.Width - corridorWidth);
                randomY = random.Next(0, maze.Texture.Height - corridorWidth);
            } while (!IsValidPosition(randomX, randomY, corridorWidth, monsterSize));

            return new Vector2(randomX, randomY);
        }

        private Vector2 GetRandomCoinPathPosition(int corridorWidth, int coinWidth, int coinHeight)
        {
            float randomX, randomY;

            int maxAttempts = 100; 

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                randomX = random.Next(0, maze.Texture.Width - corridorWidth);
                randomY = random.Next(0, maze.Texture.Height - corridorWidth);

                if (IsCoinValidPosition(randomX, randomY, corridorWidth, coinWidth, coinHeight))
                {
                    return new Vector2(randomX, randomY);
                }
            }
            return Vector2.Zero; ;
        }

        private bool IsValidPosition(float x, float y, int corridorWidth, float monsterSize)
        {
            int startX = (int)x;
            int startY = (int)y;
            int endX = (int)(x + corridorWidth);
            int endY = (int)(y + corridorWidth);

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    if (!IsPositionInWhitePath(i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsCoinValidPosition(float x, float y, int corridorWidth, int coinWidth, int coinHeight)
        {
            int startX = (int)x;
            int startY = (int)y;
            int endX = (int)(x + corridorWidth);
            int endY = (int)(y + corridorWidth);

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    if (!IsPositionInWhitePath(i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    

        private bool IsPositionInWhitePath(int x, int y)
        {
            return maze.ColorTab[y * maze.Texture.Width + x] == Color.White;
        }

        private void StartGame()
        {
            _gameStarted = true;
        }

        
        //Ecris le meilleur score dans un fichier texte
        static void WriteBestScore(int bestScore){
            using StreamWriter sw = new StreamWriter("bestScore.txt");
            sw.Write(bestScore);
        }

        //Accéder au meilleur score
        static int ReadBestScore()
        {
            if(File.Exists("bestScore.txt"))
            {
                using StreamReader sr = new StreamReader("bestScore.txt");
                if(int.TryParse(sr.ReadLine(), out int bestScore))
                {
                    return bestScore;
                }
            }
            return 0;
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            Rectangle heroRect = new Rectangle((int)hero.Position.X, (int)hero.Position.Y, hero.FrameWidth, hero.FrameHeight);

            if (!_gameStarted)
            {
                _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, WindowWidth, WindowHeight), Color.White);
                string start = "PRESS [SPACE] TO START";
                Vector2 textSize = _font.MeasureString(start);
                Vector2 textPosition = new Vector2((WindowWidth - textSize.X * 1.5f) / 2, (WindowHeight - textSize.Y * 1.5f) / 2);

                _spriteBatch.DrawString(_font, start, textPosition, Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            }
            else
            {
                DisplayLives();
                Rectangle flagRect = new Rectangle((int)maze.ExitPosition.X, (int)maze.ExitPosition.Y, 30, 30);
                if(_gameTimeRemaining <= 0){
                    PlayerLives = 0;
                }
                if (PlayerLives <= 0 || _gameTimeRemaining <= 0)
                {
                    
                    _spriteBatch.Draw(_gameOverTexture, new Rectangle(0, 0, 600, 600), Color.White);
                    
                    _finalScore = _heroScore;

                    //afficher le score a la fin du jeu apres un Gameover
                    string scoreText = $"Your Score: {_finalScore}";
                    Vector2 scoreTextSize =_font.MeasureString(scoreText);
                    Vector2 scoreTextPosition = new Vector2(WindowWidth - 200, WindowHeight - 180);
                    _spriteBatch.DrawString(_font, scoreText, scoreTextPosition, Color.White);

                    // Afficher le meilleur Score
                    string bestScoreText = $"BestScore: {_savedBestScore}";
                    Vector2 bestScoreTextSize =_font.MeasureString(bestScoreText);
                    Vector2 bestScoreTextPosition = new Vector2(WindowWidth - 200, WindowHeight - 230);
                    _spriteBatch.DrawString(_font, bestScoreText, bestScoreTextPosition, Color.White);

                    string playAgain = "PLAY AGAIN ?";
                    string yes = "YES [Y]";
                    string no = "NO [N]";

                    Vector2 playAgainPos = new Vector2(WindowWidth - 200, WindowHeight - 90);
                    _spriteBatch.DrawString(_font, playAgain, playAgainPos, Color.White);

                    Vector2 yesPos = new Vector2(WindowWidth - 215, WindowHeight - 50);
                    _spriteBatch.DrawString(_font, yes, yesPos, Color.White);

                    Vector2 noPos = new Vector2(WindowWidth - 105, WindowHeight - 50);
                    _spriteBatch.DrawString(_font, no, noPos, Color.White);

                    KeyboardState keyboardState = Keyboard.GetState();

                    if (keyboardState.IsKeyDown(Keys.Y))
                    {
                        _gameStarted = true;
                        PlayerLives = 3;
                        _gameTimeRemaining = TotalTime;
                        _heroScore = 0;
                        hero.Position = new Vector2(5, 50);
                    }
                    if (keyboardState.IsKeyDown(Keys.N))
                    {
                        Exit();
                    }
                }

                else if (heroRect.Intersects(flagRect))
                {
                    _spriteBatch.DrawString(_font, "CONGRATULATIONS!\n\n YOU WIN!", new Vector2(400, 300), Color.White);
                    
                    
                    //Afficher le score a la fin du jeu réussi
                    string scoreText = $"Your Score: {_finalScore}";
                    Vector2 scoreTextSize =_font.MeasureString(scoreText);
                    Vector2 scoreTextPosition = new Vector2(400, 200);
                    _spriteBatch.DrawString(_font, scoreText, scoreTextPosition, Color.White);
                    
                    _finalScore = _heroScore;
                    // Afficher le meilleur Score
                    string bestScoreText = $"BestScore: {_savedBestScore}";
                    Vector2 bestScoreTextSize =_font.MeasureString(bestScoreText);
                    Vector2 bestScoreTextPosition = new Vector2(400, 100);
                    _spriteBatch.DrawString(_font, bestScoreText, bestScoreTextPosition, Color.White);

                }
                else
                {
                    maze.Draw(_spriteBatch);
                    hero.DrawAnimation(_spriteBatch);

                    foreach (Monsters monster in monsters)
                    {
                        if (IsPositionInWhitePath((int)monster.Position.X, (int)monster.Position.Y))
                        {
                            _spriteBatch.Draw(monster.Texture, new Rectangle((int)monster.Position.X, (int)monster.Position.Y, (int)monster.Size.X, (int)monster.Size.Y), Color.White);
                        }
                    }

                    if (IsPositionInWhitePath((int)maze.ExitPosition.X, (int)maze.ExitPosition.Y))
                    {
                        _spriteBatch.Draw(maze.ExitFlagTexture, new Rectangle((int)maze.ExitPosition.X, (int)maze.ExitPosition.Y, 20, 20), Color.White);
                    }

                    _spriteBatch.DrawString(_font, $"Time: {Math.Ceiling(_gameTimeRemaining)}", new Vector2(WindowWidth - 150, 40), Color.White);
                    
                    _spriteBatch.DrawString(_font, $"Score: {_heroScore}", new Vector2(WindowWidth - 150, 20), Color.White);
                    _spriteBatch.DrawString(_font, $"Lives: {PlayerLives}", new Vector2(WindowWidth - 150, 60), Color.White);
                    
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}