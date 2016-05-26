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
    internal static class MapLayers
    {
        public const string CollisionLayer = "collision";
    }

    internal static class MapProperties
    {
        public const string CollisionFriction = "friction";
    }

    internal class Map : DrawableGameComponent
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly TmxMap _tmxMap;
        private bool _stepPhysics = true;
        public World World { get; set; }

        public Map(Game game, string mapName) : base(game)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            //TODO: load gravity from map settings
            _tmxMap = TmxMap.LoadTmxMap(Game, mapName);
            World = new World(new Vector2(GC.GravityX, GC.GravityY));
        }

        protected override void LoadContent()
        {
            LoadCollistions();

            //TODO: Load starting bodies to the world
            var playerPos = new Vector2(0, 0);
            if (_tmxMap.ObjectGroups != null)
            {
                var objectGroup = _tmxMap.ObjectGroups.SingleOrDefault(og => og.Name == "Objects");
                if (objectGroup != null && objectGroup.Objects != null)
                {
                    var playerInfo = objectGroup.Objects.SingleOrDefault(o => o.Name == "Player");
                    if (playerInfo != null)
                    {
                        playerPos = new Vector2(playerInfo.X, playerInfo.Y);
                    }
                }
            }

            var player = new Player(Game, World, playerPos);
            Game.Components.Add(player);

            Camera2D.Instance.Focus = player;
            base.LoadContent();
        }

        private void LoadCollistions()
        {
            //Adding polyline collisions
            var collisionLayers = _tmxMap.ObjectGroups.Where(og => og.Name.ToLower() == MapLayers.CollisionLayer).ToList();
            if (collisionLayers.Any())
            {
                foreach (var collisionLayer in collisionLayers.Where(ol => ol.Objects != null))
                {
                    foreach (var collisionObject in collisionLayer.Objects)
                    {
                        if (collisionObject.PolyLines != null && collisionObject.PolyLines.Any())
                        {
                            var dvector = new Vector2(collisionObject.X, collisionObject.Y);
                            var ground = new Body(World)
                            {
                                BodyType = BodyType.Static
                            };
                            foreach (var polyLine in collisionObject.PolyLines)
                            {
                                for (int i = 0; i < polyLine.VectorPoints.Count - 1; ++i)
                                {
                                    var startPoint = ConvertUnits.ToSimUnits(polyLine.VectorPoints[i] + dvector);
                                    var endPoint = ConvertUnits.ToSimUnits(polyLine.VectorPoints[i + 1] + dvector);
                                    FixtureFactory.AttachEdge(startPoint, endPoint, ground);
                                    if (i + 1 == polyLine.VectorPoints.Count)
                                    {
                                        startPoint = endPoint;
                                        endPoint = ConvertUnits.ToSimUnits(polyLine.VectorPoints.First() + dvector);
                                        FixtureFactory.AttachEdge(startPoint, endPoint, ground);
                                    }
                                }
                            }
                            //Get Friction
                            if (collisionObject.Properties != null)
                            {
                                var frictionProp =
                                    collisionObject.Properties.SingleOrDefault(p => p.Name.ToLower() == MapProperties.CollisionFriction);
                                if (frictionProp != null)
                                {
                                    ground.Friction = frictionProp.GetFloatValue();
                                }
                                else
                                {
                                    ground.Friction = 0.6f;
                                }
                            }
                        }
                        //TODO: proccess other objects
                    }
                }
            }
            //TODO: load collision/objects to world from map
            //foreach (var layer in _tmxMap.Layers)
            //{
            //    for (var y = 0; y < layer.Height; ++y)
            //    {
            //        for (var x = 0; x < layer.Width; ++x)
            //        {
            //            var tileId = layer.Tiles[x, y];
            //            //TODO: get if tile is collidable
            //            if (tileId == 0) continue;
            //            var body = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(32f), ConvertUnits.ToSimUnits(32f), 1f, ConvertUnits.ToSimUnits(new Vector2(x * _tmxMap.TileWidth, y * _tmxMap.TileHeight)));
            //            body.BodyType = BodyType.Static;
            //            World.BodyList.Add(body);
            //        }
            //    }
            //}
        }

        public override void Update(GameTime gameTime)
        {
            if (_stepPhysics)
            {
                World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f))); //Step at minimum of 1/30 of a second (30Hz)
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(Camera2D.Instance);
            Game.GraphicsDevice.Clear(_tmxMap.GetBackgroundColor());

            //Draw background
            var bgs = _tmxMap.ObjectGroups.SingleOrDefault(bg => bg.Name.ToLower() == "background");

            if (bgs != null)
            {
                foreach (var bg in bgs.Objects)
                {
                    if (bg.GID == 0) continue;
                    var bgimg = _tmxMap.GetTile(bg.GID);
                    _spriteBatch.Draw(bgimg.Texture, new Rectangle((int)bg.X, (int)bg.Y - bgimg.Rectangle.Height, bgimg.Rectangle.Width, bgimg.Rectangle.Height), bgimg.Rectangle, Color.White);
                }
            }

            foreach (var layer in _tmxMap.Layers)
            {
                for (var y = 0; y < layer.Height; ++y)
                {
                    for (var x = 0; x < layer.Width; ++x)
                    {
                        var tileId = layer.Tiles[x, y];

                        if (tileId == 0) continue;

                        var tile = _tmxMap.GetTile(tileId);

                        //TODO: check if this helps for fps
                        var rectangle = new Rectangle(x * _tmxMap.TileWidth, y * _tmxMap.TileHeight, _tmxMap.TileWidth, _tmxMap.TileHeight);
                        //if(!Camera2D.Instance.IsInView(rectangle)) continue;

                        //TODO: transparency, color change, animation
                        _spriteBatch.Draw(tile.Texture, rectangle, tile.Rectangle, Color.White);
                    }
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
