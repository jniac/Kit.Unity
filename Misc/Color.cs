using UnityEngine;

namespace Kit.Unity
{
    public static partial class Misc
    {
        public static Color Color(string hex)
        {
            if (hex.Substring(0, 1) == "#")
                hex = hex.Substring(1);

            int v = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            float r = 0, g = 0, b = 0, a = 1;

            if (hex.Length == 3)
            {
                r = (float)(v >> 8) / 0xf;
                g = (float)((v >> 4) & 0xf) / 0xf;
                b = (float)(v & 0xf) / 0xf;
            }

            if (hex.Length == 4)
            {
                r = (float)(v >> 12) / 0xf;
                g = (float)((v >> 8) & 0xf) / 0xf;
                b = (float)((v >> 4) & 0xf) / 0xf;
                a = (float)(v & 0xf) / 0xf;
            }

            if (hex.Length == 6)
            {
                r = (float)(v >> 16) / 0xff;
                g = (float)((v >> 8) & 0xff) / 0xff;
                b = (float)(v & 0xff) / 0xff;
            }

            if (hex.Length == 8)
            {
                r = (float)(v >> 24) / 0xff;
                g = (float)((v >> 16) & 0xff) / 0xff;
                b = (float)((v >> 8) & 0xff) / 0xff;
                a = (float)(v & 0xff) / 0xff;
            }

            return new Color(r, g, b, a);
        }

        public static Color WithAlpha(Color color, float alpha) =>
            new Color(color.r, color.g, color.b, alpha);
    }
}
