using System;
using Microsoft.Xna.Framework;

namespace TheGame.Entities.Drawable
{
    class Entity : DrawableGameComponent
    {
        public Vector2 Position { get; set; }

        public Entity(Game game) : base(game)
        {

        }
    }
}
