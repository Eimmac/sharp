using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.TiledMax;

namespace TheGame.Entities.Drawable
{
    internal class Map : DrawableGameComponent
    {
        private readonly string _mapName;
        private readonly SpriteBatch _spriteBatch;
        private TmxMap _tmxMap;

        public Map(Game game, string mapName) : base(game)
        {
            _mapName = mapName;
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _tmxMap = TmxMap.LoadTmxMap(Game, _mapName);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            foreach (var layer in _tmxMap.Layers)
            {
                for (int y = 0; y < layer.Height; ++y)
                {
                    for (int x = 0; x < layer.Width; ++x)
                    {
                        var tileId = layer.Tiles[x, y];
                        if (tileId == 0)  continue;
                        var tile = _tmxMap.GetTile(tileId);
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
