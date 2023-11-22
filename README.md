# Fade Form

Let me see if I follow.

> i have a panel in my form

[![sample panel][1]][1]
___

> there is form named form1 opened inside a panel

```
public partial class MainForm : Form
{
    .
    .
    .
    Form1 form1 { get; } = new Form1
    {
        StartPosition = FormStartPosition.Manual,
    };
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        form1.Location = panel1.PointToScreen(new Point(panel1.Right - 370, 50));
        form1.Show(this);
    }
}
```
[![form inside panel][2]][2]
___

> i want to add this fade form over this form1 so that the **control of form1 should be partially visible** and when this happened

[![animation][3]][3]

**Fade Form**
```
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
            Bounds = parent.RectangleToScreen(parent.ClientRectangle);
                
            // Animate
            for (float f = 0; f <= TARGET_OPACITY; f += 0.05F)
            {
                Opacity = Math.Pow(f, 2);
                await Task.Delay(TimeSpan.FromSeconds(0.01));
                if (!Visible) break; // If form hides during animation.
            }
        }
    }
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        e.Cancel = true;
        Hide();
    }
}
```
___

**Main Form**

```
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
        Disposed += (sender, e) => FadeForm.Dispose();
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
```
___
> i want to open a focus form of which the link i just shared

[![open link][4]][4]

My crystal ball gets fuzzy at this point about what you want to do next but hopefully this gives you a starting point.


  [1]: https://i.stack.imgur.com/r2VUi.png
  [2]: https://i.stack.imgur.com/SKKTj.png
  [3]: https://i.stack.imgur.com/hbmd7.png
  [4]: https://i.stack.imgur.com/DF7DM.png