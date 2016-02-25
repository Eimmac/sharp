using Microsoft.Xna.Framework;
using TheGame.Entities.Drawable;
using TheGame.Entities.Logical;

namespace TheGame
{
    internal class MainGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        public MainGame()
        {
            //We need to create graphics device manager, because game won't work without graphics
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            //Folder where we will save our game assets (Pictures, music, etc..)
            Content.RootDirectory = "Content";

            //TODO: make our own pointer graphics
            IsMouseVisible = true;

            Window.Title = "Outstanding Game Name";
        }

        protected override void Initialize()
        {
            InputHandler.CreateInstance(this);
            Components.Add(InputHandler.Instance);

            //TODO: loading map for test purposes
            Components.Add(new Map(this, "TestMap.tmx"));

            //Exit game on exit action
            InputHandler.Instance[ActionControlls.Exit].OnPressed += gt => {Exit();};
            
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
