using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Entities.Drawable
{
    internal class Player : DrawableGameComponent
    {
        private readonly SpriteBatch _spriteBatch;

        public Player(Game game) : base(game)
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
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
