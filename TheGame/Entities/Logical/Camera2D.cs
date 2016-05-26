using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Entities.Drawable;

namespace TheGame.Entities.Logical
{
    internal class Camera2D : GameComponent
    {
        protected float ViewportHeight;
        protected float ViewportWidth;
        private Vector2 _position;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public Entity Focus { get; set; }
        public float MoveSpeed { get; set; }

#region singleton

        private Camera2D(Game game) : base(game)
        {
        }

        public static Camera2D CreateInstance(Game game)
        {
            if(Instance != null)
                throw new InvalidOperationException("Camera is already created");

            Instance = new Camera2D(game);

            return Instance;
        }

        public static Camera2D Instance { get; private set; }

        #endregion
        public override void Initialize()
        {
            ViewportWidth = Game.GraphicsDevice.Viewport.Width;
            ViewportHeight = Game.GraphicsDevice.Viewport.Height;

            ScreenCenter = new Vector2(ViewportWidth / 2, ViewportHeight / 2);
            Scale = 1;
            MoveSpeed = 5.25f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(Scale, Scale, Scale));

            Origin = ScreenCenter / Scale;

            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Focus != null)
            {
                _position.X += (Focus.Position.X - Position.X) * MoveSpeed * delta;
                _position.Y += (Focus.Position.Y - Position.Y) * MoveSpeed * delta;
            }
            base.Update(gameTime);
        }

        public bool IsInView(Rectangle rect)
        {
            //Check width
            if (rect.X + rect.Width < Position.X - Origin.X || rect.X > Position.X + Origin.X)
                return false;

            //Check height
            if (rect.Y + rect.Height < Position.Y - Origin.Y || rect.Y > Position.Y + Origin.Y)
                return false;

            return true;
        }
    }
}
