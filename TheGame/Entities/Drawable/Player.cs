using System;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Entities.Animation;//
using TheGame.Entities.Logical;

namespace TheGame.Entities.Drawable
{
    internal sealed class Player : Entity
    {
        private enum Directions
        {
            Up,
            Down,
            Left,
            Right
        }
        private Directions _direction;
        private readonly SpriteBatch _spriteBatch;
        //TODO: remove hardcoded force
        private static int Force = 100;
        //TODO: remove harcoded size
        private static readonly Vector2 Size = new Vector2(64f);
        public Body Body { get; set; }
        private Texture2D _sprite;
        PlayerAnimation frame = new PlayerAnimation();        //
        private bool ChangeFrame = false;                   //
        private readonly World _world;
        private Vector2 _origin;
        private string cher = cherArray[kelintas];
        private static int kelintas = 0;
        private static string[] cherArray = {"bite", "burundukas", "peleda", "jautis", "krabas" };
        private bool pakeistas = false;
        

        public Player(Game game, World world, Vector2 position) : base(game)
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Position = position;
            _world = world;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(120f), ConvertUnits.ToSimUnits(120f), 1f);   // 69,30
            Body.Mass = 250;
            Position = position;
            Body.Position = ConvertUnits.ToSimUnits(Position);
        }

        public override void Initialize()
        {
            InputHandler.Instance[ActionControlls.Right].OnDown += OnActionDown;
            InputHandler.Instance[ActionControlls.Left].OnDown += OnActionDown;
            InputHandler.Instance[ActionControlls.Jump].OnDown += OnActionDown;
            InputHandler.Instance[ActionControlls.Down].OnDown += OnActionDown;

            InputHandler.Instance[ActionControlls.Right].OnReleased += OnReleased;
            InputHandler.Instance[ActionControlls.Left].OnReleased += OnReleased;
            InputHandler.Instance[ActionControlls.Jump].OnReleased += OnReleased;
            InputHandler.Instance[ActionControlls.Shoot].OnReleased += OnReleased;
            base.Initialize();
        }

        public void pakeistiCher1()

        {
            
           
            if (pakeistas == false)
            {
                if (kelintas < 4) kelintas++;

                else kelintas = 0;
                cher = cherArray[kelintas];
            }
           // pakeistas = true;

        }

      

        private void OnActionDown(GameTime gameTime, ActionControlls action)
        {
            var force = Vector2.Zero;
            switch (action)
            {
                case ActionControlls.Down:
                    force = new Vector2(0, 1f);
                    _direction = Directions.Down;
                    break;
                case ActionControlls.Left:
                    force = new Vector2(-1f, 0);
                    _direction = Directions.Left;
                    break;
                case ActionControlls.Right:
                    force = new Vector2(1f, 0);   //Force*gameTime.ElapsedGameTime.Milliseconds/1000f
                    _direction = Directions.Right;
                    break;
                case ActionControlls.Jump:
                    force = new Vector2(0, -3f);
                    //_direction = Directions.Up;
                    break;
            }
            ChangeFrame = true;
            Body.ApplyLinearImpulse(force, Body.Position);
        }


        private void OnReleased(GameTime gameTime, ActionControlls action)
        {
            var force = Vector2.Zero;
            switch (action)
            {
                case ActionControlls.Right:
                    ChangeFrame = false;
                    break;
                case ActionControlls.Left:
                    ChangeFrame = false;
                    break;
                case ActionControlls.Jump:
                    ChangeFrame = false;
                    break;
                case ActionControlls.Shoot:
                    Console.Write("shoooooot");
                    pakeistiCher1();
                    break;



            }
            
        }


        

        protected override void LoadContent()
        {
            //TODO: remove hardcoded 
            // _sprite = Game.Content.Load<Texture2D>("Textures/Entities/Bite/bitės.0000.png");
            _sprite = Game.Content.Load<Texture2D>(frame.GetFrame(ChangeFrame,cher));

            //Creating body from texture
            var data = new uint[_sprite.Width*_sprite.Height];
            _sprite.GetData(data);
            var textureVertices = PolygonTools.CreatePolygon(data, _sprite.Width, false);
            var centroind = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroind);
            _origin = -centroind;
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);
            var list = Triangulate.ConvexPartition(textureVertices, TriangulationAlgorithm.Bayazit);
            
            //scale:
            var scale = new Vector2(ConvertUnits.ToSimUnits(1)) * Size.X / _sprite.Width;
            foreach (var vertices in list)
            {
                vertices.Scale(ref scale);
            }
            Body = BodyFactory.CreateCompoundPolygon(_world, list, 1f, BodyType.Dynamic);

            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 15;
            Body.Position = ConvertUnits.ToSimUnits(Position);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Position = ConvertUnits.ToDisplayUnits(Body.Position);
            _sprite = Game.Content.Load<Texture2D>(frame.GetFrame(ChangeFrame,cher));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(Camera2D.Instance);
            //_spriteBatch.Draw(_sprite, new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y), _sprite.Bounds, Color.White, Body.Rotation, _origin, /*_direction == Directions.Left ? SpriteEffects.FlipHorizontally : */SpriteEffects.None, 0);
              _spriteBatch.Draw(_sprite, new Rectangle((int)Position.X, (int)Position.Y,  (int) Size.X, (int ) Size.Y), _sprite.Bounds, Color.White, 0f, _origin, _direction == Directions.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
