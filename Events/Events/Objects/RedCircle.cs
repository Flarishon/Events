using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Objects
{
    class RedCircle : BaseObject
    {
        public Action<Player> OnPlayerOverlap;

        public float Radius = 20;
        private float growthRate = 0.5f;

        public RedCircle(float x, float y, float angle) : base(x, y, angle) { }

        public void Update()
        {
            Radius += growthRate;
        }

        public void Reset()
        {
            Radius = 20;
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(128, 255, 0, 0)), -Radius, -Radius, Radius * 2, Radius * 2);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-Radius, -Radius, Radius * 2, Radius * 2);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj is Player)
            {
                OnPlayerOverlap(obj as Player);
            }
        }
    }
}