using FarseerPhysics;
using FarseerPhysics.DebugView;
using Microsoft.Xna.Framework;
using TheGame.Entities.Drawable;
using TheGame.Entities.Logical;

namespace TheGame
{
    internal class MainGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
#if DEBUG
        private DebugViewXNA _debugView;
        private bool _showDebugView;
        private Matrix _debugProjection;
#endif

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
            Components.Add(InputHandler.CreateInstance(this));
            Components.Add(Camera2D.CreateInstance(this));

            //TODO: loading map for test purposes
            var map = new Map(this, "TestMap.tmx");
            Components.Add(map);
#if DEBUG
            _showDebugView = false;
            _debugProjection = Matrix.CreateOrthographicOffCenter(0f, ConvertUnits.ToSimUnits(GraphicsDevice.Viewport.Width), ConvertUnits.ToSimUnits(GraphicsDevice.Viewport.Height), 0f, 0f, 1f);
            _debugView = new DebugViewXNA(map.World)
            {
                DefaultShapeColor = Color.White,
                SleepingShapeColor = Color.LightGray
            };
            _debugView.LoadContent(GraphicsDevice, Content);
            _debugView.AppendFlags(DebugViewFlags.AABB);
            _debugView.AppendFlags(DebugViewFlags.CenterOfMass);
            _debugView.AppendFlags(DebugViewFlags.ContactNormals);
            _debugView.AppendFlags(DebugViewFlags.ContactPoints);
            _debugView.AppendFlags(DebugViewFlags.DebugPanel);
            _debugView.AppendFlags(DebugViewFlags.PerformanceGraph);
            _debugView.AppendFlags(DebugViewFlags.PolygonPoints);
            InputHandler.Instance[ActionControlls.DebugPhysics].OnPressed += delegate {
                _showDebugView = !_showDebugView;
            };
#endif
            //Exit game on exit action
            InputHandler.Instance[ActionControlls.Exit].OnPressed += (time, controlls) => { Exit(); };
            
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
#if DEBUG
            if (_showDebugView)
            {
                var c = Camera2D.Instance;
                var dview = Matrix.Identity *
                            Matrix.CreateTranslation(ConvertUnits.ToSimUnits(-c.Position.X), ConvertUnits.ToSimUnits(-c.Position.Y), 0) *
                            Matrix.CreateRotationZ(c.Rotation) *
                            Matrix.CreateTranslation(ConvertUnits.ToSimUnits(c.Origin.X), ConvertUnits.ToSimUnits(c.Origin.Y), 0) *
                            Matrix.CreateScale(c.Scale);
                _debugView.RenderDebugData(_debugProjection, dview);
            }
#endif
        }
    } 
}
