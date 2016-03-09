using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Entities.Logical;

namespace TheGame.Entities.Drawable
{
    internal class Player : Entity
    {
        private readonly SpriteBatch _spriteBatch;
        public Body Body { get; set; }
        private Texture2D _sprite;

        public Player(Game game, World world) : base(game)
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //TODO: create body from texture
            Body = BodyFactory.CreateRectangle(world, 16f, 16f, 1f);
            Body.BodyType = BodyType.Dynamic;
            Position = Vector2.Zero;
        }

        public override void Initialize()
        {
            InputHandler.Instance[ActionControlls.Right].OnDown += Player_OnDown;
            base.Initialize();
        }

        private void Player_OnDown(GameTime gameTime)
        {
            Body.ApplyForce(new Vector2(32f, 0));
        }

        protected override void LoadContent()
        {
            _sprite = Game.Content.Load<Texture2D>("Textures/Player.png");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Position = Body.Position;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(Camera2D.Instance);

            _spriteBatch.Draw(_sprite, Position, Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
