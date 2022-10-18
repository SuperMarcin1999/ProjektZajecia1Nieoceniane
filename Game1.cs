using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Viewport = Microsoft.Xna.Framework.Graphics.Viewport;

namespace ProjektZajecia1Nieoceniane
{ 
    class DuchZywy
    {
        public Rectangle Rectangle { get; set; }
        public float CzasZycia { get; set; }
        public bool ZostalKlikniety { get; set; }
    }


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D backGround, duszek, duszekKliniety;
        private Random random;

        private float CZAS_POJAWIANIA_DUSZKA_V = 0.5f;

        private float czasPojawianiaDuszka;
        private int counter;
        private List<Rectangle> listaKlieknietychDuszkow;
        private List<DuchZywy> listaZywychDuszkow;
        private Rectangle rectTla;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            this.Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            counter = 0;
            random = new Random();
            czasPojawianiaDuszka = CZAS_POJAWIANIA_DUSZKA_V;
            listaKlieknietychDuszkow = new List<Rectangle>();
            listaZywychDuszkow = new List<DuchZywy>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            backGround = Content.Load<Texture2D>("street");
            duszek = Content.Load<Texture2D>("ghost");
            duszekKliniety = Content.Load<Texture2D>("ghost-foot");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //co 0.5s (CZAS_POJAWIANIA_DUSZKA_V) tworz nowego duszka
            if (czasPojawianiaDuszka == 0.0f)
            {
                Rectangle rectangleDuszka = new Rectangle(
                    random.Next(GraphicsDevice.Viewport.Width - 50),
                    random.Next(GraphicsDevice.Viewport.Height - 50),
                    50,
                    50);
                listaZywychDuszkow.Add(
                    new DuchZywy()
                    {
                        Rectangle = rectangleDuszka, CzasZycia = random.NextFloat(0,2)
                    });
                
                czasPojawianiaDuszka = CZAS_POJAWIANIA_DUSZKA_V;
            }

            //uplywanie czasu zycia duszkow zywych
            for (int i = 0; i < listaZywychDuszkow.Count; i++)
            {
                listaZywychDuszkow[i].CzasZycia -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (listaZywychDuszkow[i].CzasZycia <= 0)
                {
                    listaZywychDuszkow.RemoveAt(i);
                }
            }

            czasPojawianiaDuszka =
                MathHelper.Max(0, czasPojawianiaDuszka - (float) gameTime.ElapsedGameTime.TotalSeconds);

            //sprawdzenie klikniecie
            foreach (var duchZywy in listaZywychDuszkow)
            {
                if ((Mouse.GetState().LeftButton == ButtonState.Pressed) &&
                    (duchZywy.Rectangle.Contains(Mouse.GetState().Position)))
                {
                    if (!duchZywy.ZostalKlikniety)
                    {
                        counter++;
                        Window.Title = counter.ToString();
                        listaKlieknietychDuszkow.Add(duchZywy.Rectangle);
                        duchZywy.ZostalKlikniety = true;
                    }
                }
            }

            //tlo responsywne
            rectTla = new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(backGround, rectTla, Color.White);
            listaZywychDuszkow.ForEach(zywyDuch => _spriteBatch.Draw(duszek, zywyDuch.Rectangle, Color.White));
            listaKlieknietychDuszkow.ForEach(rect => _spriteBatch.Draw(duszekKliniety, rect, Color.White));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}