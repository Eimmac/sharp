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

        public Player(Game game, World world, Vector2 position) : base(game)
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //TODO: create body from texture
            Body = BodyFactory.CreateRectangle(world, 64f, 64f, 1f);
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 80;
            Position = position;
            Body.Position = Position;
        }
        //TODO: perdaryti kad viena metoda galėtumėm naudot visom knopkem
        public override void Initialize()
        {
            InputHandler.Instance[ActionControlls.Right].OnDown += Player_OnDown;
            InputHandler.Instance[ActionControlls.Left].OnDown += Player_OnLeftDown;
             InputHandler.Instance[ActionControlls.Jump].OnPressed += Player_OnUpPressed;
            InputHandler.Instance[ActionControlls.Down].OnDown += Player_OnDownDown;

            base.Initialize();
        }

        private void Player_OnDown(GameTime gameTime)
        {
            //Body.ApplyForce(new Vector2(32f, 0));
            Body.ApplyLinearImpulse(new Vector2(50,0), new Vector2(0, 0));
        }

        private void Player_OnLeftDown(GameTime gameTime)
        {
            Body.ApplyLinearImpulse(new Vector2(-50,0), new Vector2(0, 0));
        }

        private void Player_OnUpPressed(GameTime gameTime)
        {
            Body.ApplyLinearImpulse(new Vector2(0, -100), new Vector2(0, 0));
        }

        private void Player_OnDownDown(GameTime gameTime)
        {
            Body.ApplyLinearImpulse(new Vector2(0, 50), new Vector2(0, 0));
        }

        protected override void LoadContent()
        {
            _sprite = Game.Content.Load<Texture2D>("Textures/Entities/Bite/bitės.0000.png");
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

            _spriteBatch.Draw(_sprite, new Rectangle((int)Position.X - 16, (int)Position.Y - 16, 64, 64), _sprite.Bounds,Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
