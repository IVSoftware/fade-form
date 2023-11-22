
namespace fade_form
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            foreach (var button in panel1.Controls.OfType<Button>())
            {
                button.MouseEnter += (sender, e) =>
                {
                    FadeForm.Hide();
                    FadeForm.Show(form1);
                    switch (button.Name) 
                    {
                        case nameof(button1): FadeForm.BackColor = Color.LightBlue;  break;
                        case nameof(button2): FadeForm.BackColor = Color.LightGreen; break;
                        case nameof(button3): FadeForm.BackColor = Color.LightCoral; break;
                    }
                };
            }
            Disposed += (sender, e) =>
            {
                FadeForm.Dispose();
                form1.Dispose();
            };
        }
        Form1 form1 { get; } = new Form1
        {
            StartPosition = FormStartPosition.Manual,
        };
        FadeForm FadeForm { get; } = new FadeForm();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            form1.Location = panel1.PointToScreen(new Point(panel1.Right - 370, 50));
            form1.Show(this);
        }
    }

    class FadeForm : Form
    {
        const float TARGET_OPACITY = 0.85F;
        public FadeForm()
        {
            BackColor = Color.DarkGray;
            FormBorderStyle = FormBorderStyle.None;
            Padding = new Padding(0, 0, 0, 20);
            var linkLabel = new LinkLabel
            {
                Dock = DockStyle.Bottom,
                Text = "https://www.google.com",
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 50,
            };
            linkLabel.LinkClicked += (sender, e) =>
            {
                MessageBox.Show($"Do something with {linkLabel.Text}");
            };
            Controls.Add(linkLabel);
        }
        public new async void Show(IWin32Window owner)
        {
            base.Show(owner);
            if (owner is Form parent)
            {
                localSubscribeParent();
                localTrackParent(this, EventArgs.Empty);
                
                // Animate
                for (float f = 0; f <= TARGET_OPACITY; f += 0.05F)
                {
                    Opacity = Math.Pow(f, 2);
                    await Task.Delay(TimeSpan.FromSeconds(0.01));
                    if (!Visible) break; // If form hides during animation.
                }
                void localSubscribeParent()
                {
                    parent.Move -= localTrackParent;
                    parent.SizeChanged -= localTrackParent;
                    parent.Move += localTrackParent;
                    parent.SizeChanged += localTrackParent;
                }
                void localTrackParent(object? sender, EventArgs e) =>
                    Bounds = parent.RectangleToScreen(parent.ClientRectangle);
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = true;
            Hide();
        }
    }
}
