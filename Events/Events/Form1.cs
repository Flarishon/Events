using Events.Objects;
using System;

namespace Events
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        RedCircle redCircle;
        GreenCircle firstGreenCircle;
        GreenCircle secondGreenCircle;
        Random rnd = new Random();
        int score = 0;

        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            redCircle = new RedCircle(rnd.Next(20, pbMain.Width - 20), rnd.Next(20, pbMain.Height - 20), 0);

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnGreenCircleOverlap += (gc) =>
            {
                objects.Remove(gc);
                if (gc == firstGreenCircle)
                {
                    firstGreenCircle = null;
                    firstGreenCircle = new GreenCircle(rnd.Next(20, pbMain.Width - 20), rnd.Next(20, pbMain.Height - 20), 0);
                    objects.Add(firstGreenCircle);
                    score++;
                }   
                else if (gc == secondGreenCircle)
                {
                    secondGreenCircle = null;
                    secondGreenCircle = new GreenCircle(rnd.Next(20, pbMain.Width - 20), rnd.Next(20, pbMain.Height - 20), 0);
                    objects.Add(secondGreenCircle);
                    score++;
                }
                txtScore.Text = $"Очки: {score}";
            };

            redCircle.OnPlayerOverlap += (obj) =>
            {
                redCircle.Reset();
                redCircle.X = rnd.Next(20, pbMain.Width - 20);
                redCircle.Y = rnd.Next(20, pbMain.Height - 20);
                score--;
                txtScore.Text = $"Очки: {score}";
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            firstGreenCircle = new GreenCircle(rnd.Next(20, pbMain.Width - 20), rnd.Next(20, pbMain.Height - 20), 0);
            secondGreenCircle = new GreenCircle(rnd.Next(20, pbMain.Width - 20), rnd.Next(20, pbMain.Height - 20), 0);

            objects.Add(player);
            objects.Add(marker);
            objects.Add(firstGreenCircle);
            objects.Add(secondGreenCircle);
            objects.Add(redCircle);
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            updatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            if (redCircle != null)
            {
                redCircle.Update();
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
