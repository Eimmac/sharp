using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheGame
{
    class MainGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        public MainGame()
        {
            //We need to create graphics device manager, because game won't work without graphics
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            //Folder where we will save our game assets (Pictures, music, etc..)
            Content.RootDirectory = "Content";

            Window.Title = "Outstanding Game Name";
        }

        protected override void Initialize()
        {
            //TODO: initialization
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //TODO: load content
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            //TODO: Unload unmanaged content
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            //TODO: update logic
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //TODO: draw logic
            GraphicsDevice.Clear(Color.CornflowerBlue); //Because I like Cornflower Blue
            base.Draw(gameTime);
        }
    }
}
