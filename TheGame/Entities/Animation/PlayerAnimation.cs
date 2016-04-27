using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Entities.Logical;

namespace TheGame.Entities.Animation
{
    class PlayerAnimation
    {
       
        const int AnimationSpeed = 3; // animacijos greitis
        private int i = 0;
        private int j = 0;



        public string GetFrame(bool change)
        {
            if (change)
            {
                j++;
                if (j > AnimationSpeed)
                {
                    i++;
                    j = 0;
                }
                if (i > 11) i = 1;
                if (i > 9) return "Textures/Entities/Bite/bitės.00" + i + ".png";
                else return "Textures/Entities/Bite/bitės.000" + i + ".png";
            }
            return "Textures/Entities/Bite/bitės.0001.png";

        }

      
        
    }
}
