using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Entities.Logical;

namespace TheGame.Entities.Drawable
{
    internal class Player : DrawableGameComponent
    {
        private readonly SpriteBatch _spriteBatch;
        public Body Body { get; set; }

        public Player(Game game, World world) : base(game)
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Body = BodyFactory.CreateRectangle(world, 16f, 16f, 1f);
            Body.BodyType = BodyType.Dynamic;
        }

        public override void Initialize()
        {
            InputHandler.Instance[ActionControlls.Right].OnDown += Player_OnDown;
            base.Initialize();
        }

        private void Player_OnDown(GameTime gameTime)
        {
            Body.ApplyForce(new Vector2(1f, 0));
        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();



            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
