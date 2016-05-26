using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Entities.Animation;
using TheGame.Entities.Logical;

namespace TheGame.Entities.Drawable
{
    internal class Enemy : Entity
    {
        private enum directions
        {
            up,
            down,
            left,
            right
        }
        private directions direction;
        private readonly SpriteBatch _spriteBatch;
        private const int force = 10;
        public Body Body { get; set; }
        private Texture2D _sprite;
        PlayerAnimation frame = new PlayerAnimation();
        private bool ChangeFrame = false;

        public Enemy(Game game, World world, Vector2 position) : base(game)
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //TODO: create body from texture
            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(69f), ConvertUnits.ToSimUnits(30f), 1f);
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 250;
            Position = position;
            Body.Position = ConvertUnits.ToSimUnits(Position);
        }
        //TODO: perdaryti kad viena metoda galėtumėm naudot visom knopkem
        public override void Initialize()
        {
            InputHandler.Instance[ActionControlls.Right].OnDown += Player_OnRightDown;
            InputHandler.Instance[ActionControlls.Left].OnDown += Player_OnLeftDown;
            InputHandler.Instance[ActionControlls.Jump].OnDown += Player_OnUpPressed;
            InputHandler.Instance[ActionControlls.Down].OnDown += Player_OnDownDown;

            InputHandler.Instance[ActionControlls.Right].OnReleased += Player_OnReleasedRight;
            InputHandler.Instance[ActionControlls.Left].OnReleased += Player_OnReleasedLeft;
            InputHandler.Instance[ActionControlls.Jump].OnReleased += Player_OnReleasedUp;



            base.Initialize();
        }



        private void Player_OnRightDown(GameTime gameTime)
        {
            //Body.ApplyForce(new Vector2(32f, 0));
            Body.ApplyLinearImpulse(new Vector2(force, 1), Body.LocalCenter);
            direction = directions.down;
            ChangeFrame = true;
        }

        private void Player_OnLeftDown(GameTime gameTime)
        {
            Body.ApplyLinearImpulse(new Vector2(-force, 1), Body.LocalCenter);
            direction = directions.left;
            ChangeFrame = true;
        }

        private void Player_OnUpPressed(GameTime gameTime)
        {
            Body.ApplyLinearImpulse(new Vector2(0, -force), Body.LocalCenter);
            direction = directions.up;
            ChangeFrame = true;
        }

        private void Player_OnDownDown(GameTime gameTime)
        {
            Body.ApplyLinearImpulse(new Vector2(0, force), Body.LocalCenter);
            direction = directions.right;


        }


        private void Player_OnReleasedRight(GameTime gameTime)
        {
            ChangeFrame = false;
        }

        private void Player_OnReleasedLeft(GameTime gameTime)
        {
            ChangeFrame = false;
        }
        private void Player_OnReleasedUp(GameTime gameTime)
        {
            ChangeFrame = false;
        }

        protected override void LoadContent()
        {

            //_sprite = Game.Content.Load<Texture2D>("Textures/Entities/Bite/bitės.0000.png");
            Console.WriteLine(frame.GetFrame(ChangeFrame));

            _sprite = Game.Content.Load<Texture2D>(frame.GetFrame(ChangeFrame));


            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Position = ConvertUnits.ToDisplayUnits(Body.Position);
            _sprite = Game.Content.Load<Texture2D>(frame.GetFrame(ChangeFrame));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(Camera2D.Instance);

            _spriteBatch.Draw(_sprite, new Rectangle((int)Position.X, (int)Position.Y, 64, 64), _sprite.Bounds, Color.White, 0f, new Vector2(1000, 1000), direction == directions.left ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            //_spriteBatch.Draw(_sprite, Position, null, Color.White, Body.Rotation, new Vector2(1000,1000), 0.01f, SpriteEffects.None, 0f);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
