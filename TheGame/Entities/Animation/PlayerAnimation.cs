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
        private int i = 1;
        private int j = 0;
        private string cher = "burundukas";


        public string GetFrame(bool change, string cher)
        {
            switch (cher)
            {
                case "bite":
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
                    return "Textures/Entities/Bite/bitės.0002.png";

                case "peleda":
                    if (change)
                    {
                        j++;
                        if (j > AnimationSpeed)
                        {
                            i++;
                            j = 0;
                        }
                        if (i > 12) i = 1;
                        if (i > 9) return "Textures/Entities/Peleda/peleda.00" + i + ".png";
                        else return "Textures/Entities/Peleda/peleda.000" + i + ".png";
                    }
                    return "Textures/Entities/Peleda/peleda.0002.png";

                case "burundukas":
                    if (change)
                    {
                        j++;
                        if (j > AnimationSpeed)
                        {
                            i++;
                            j = 0;
                        }
                        if (i > 12) i = 1;
                        if (i > 9) return "Textures/Entities/Burundukas/burunduk.00" + i + ".png";
                        else return "Textures/Entities/Burundukas/burunduk.000" + i + ".png";
                    }
                    return "Textures/Entities/Burundukas/burunduk.0002.png";

                case "jautis":
                    if (change)
                    {
                        j++;
                        if (j > AnimationSpeed)
                        {
                            i++;
                            j = 0;
                        }
                        if (i > 12) i = 1;
                        if (i > 9) return "Textures/Entities/Jautis/jautis.00" + i + ".png";
                        else return "Textures/Entities/Jautis/jautis.000" + i + ".png";
                    }
                    return "Textures/Entities/Jautis/jautis.0002.png";

                case "krabas":
                    if (change)
                    {
                        j++;
                        if (j > AnimationSpeed)
                        {
                            i++;
                            j = 0;
                        }
                        if (i > 12) i = 1;
                        if (i > 9) return "Textures/Entities/Krabas/krab.00" + i + ".png";
                        else return "Textures/Entities/Krabas/krab.000" + i + ".png";
                    }
                    return "Textures/Entities/Krabas/krab.0001.png";

            }




            return "Textures/Entities/Bite/bitės.00639.png";

        }
    }
}
