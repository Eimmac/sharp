using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Entities.Logical;
using TheGame.TiledMax;
using System.Linq;
using FarseerPhysics;

namespace TheGame.Entities.Drawable
{
    internal class Map : DrawableGameComponent
    {
        private readonly string _mapName;
        private readonly SpriteBatch _spriteBatch;
        private TmxMap _tmxMap;
        private World _world;

        public Map(Game game, string mapName) : base(game)
        {
            _mapName = mapName;
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);

        }

        protected override void LoadContent()
        {
            //Can be in constructor, but left it here to be able to reload map in debug mode
            _tmxMap = TmxMap.LoadTmxMap(Game, _mapName);

            //TODO: load gravity from map settings
            _world = new World(new Vector2(GC.GravityX, GC.GravityY));

            //TODO: load collision/objects to world from map
            foreach (var layer in _tmxMap.Layers)
            {
                for (var y = 0; y < layer.Height; ++y)
                {
                    for (var x = 0; x < layer.Width; ++x)
                    {
                        var tileId = layer.Tiles[x, y];
                        //TODO: get if tile is collidable
                        if (tileId == 0) continue;
                        var body = BodyFactory.CreateRectangle(_world, ConvertUnits.ToSimUnits(32f), ConvertUnits.ToSimUnits(32f), 1f, ConvertUnits.ToSimUnits(new Vector2(x * _tmxMap.TileWidth, y * _tmxMap.TileHeight)));
                        body.BodyType = BodyType.Static;
                        _world.BodyList.Add(body);
                    }
                }
            }
            //TODO: Load starting bodies to the world
            var playerPos = new Vector2(0, 0);
            if(_tmxMap.ObjectGroups != null)
            {
                var objectGroup = _tmxMap.ObjectGroups.SingleOrDefault(og => og.Name == "Objects");
                if(objectGroup != null && objectGroup.Objects != null)
                {
                    var playerInfo = objectGroup.Objects.SingleOrDefault(o => o.Name == "Player");
                    if(playerInfo != null)
                    {
                        playerPos = new Vector2(playerInfo.X, playerInfo.Y);
                    }
                }
            }
            
            var player = new Player(Game, _world, playerPos);
            Game.Components.Add(player);

            Camera2D.Instance.Focus = player;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f))); //Step at minimum of 1/30 of a second (30Hz)

            //TODO: camera logic
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(Camera2D.Instance);
            Game.GraphicsDevice.Clear(_tmxMap.GetBackgroundColor());

            foreach (var layer in _tmxMap.Layers)
            {
                for (var y = 0; y < layer.Height; ++y)
                {
                    for (var x = 0; x < layer.Width; ++x)
                    {
                        var tileId = layer.Tiles[x, y];

                        if (tileId == 0) continue;

                        var tile = _tmxMap.GetTile(tileId);

                        //TODO: transparensy, color change, animation
                        _spriteBatch.Draw(tile.Texture,
                            new Rectangle(x * _tmxMap.TileWidth, y * _tmxMap.TileHeight, _tmxMap.TileWidth, _tmxMap.TileHeight),
                            tile.Rectangle,
                            Color.White);
                    }
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
