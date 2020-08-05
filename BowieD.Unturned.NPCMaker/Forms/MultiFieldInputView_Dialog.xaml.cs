using System;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for MultiFieldInputView_Dialog.xaml
    /// </summary>
    public partial class MultiFieldInputView_Dialog : Window
    {
        public MultiFieldInputView_Dialog()
        {
            InitializeComponent();
        }
        public MultiFieldInputView_Dialog(string[] defaultValues)
        {
            InitializeComponent();

            this.defaults = defaultValues;
        }

        private readonly string[] defaults;

        [Obsolete("Use ShowDialog", true)]
        public new void Show() { base.Show(); }
        [Obsolete("Use other overload of this method", true)]
        public new bool? ShowDialog()
        {
            throw new NotImplementedException();
        }
        public bool? ShowDialog(string[] messages, string caption)
        {
            return ShowDialog(messages, caption, new string[messages.Length]);
        }
        public bool? ShowDialog(string[] messages, string caption, string[] tooltips)
        {
            if (messages.Length != tooltips.Length)
            {
                throw new ArgumentException("Messages and tooltips must have same length");
            }

            string[] pDefaults = defaults ?? new string[messages.Length];

            if (messages.Length != pDefaults.Length)
                throw new ArgumentException("Messages and default values must have same length");

            Title = caption;

            _textboxes = new TextBox[messages.Length];

            for (int i = 0; i < messages.Length; i++)
            {
                Grid g = new Grid();
                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                g.ColumnDefinitions.Add(new ColumnDefinition());

                TextBlock tb = new TextBlock()
                {
                    MaxWidth = 200,
                    TextWrapping = TextWrapping.Wrap,
                    Text = messages[i]
                };
                Label l = new Label()
                {
                    Content = tb,
                    Margin = new Thickness(10),
                    VerticalAlignment = VerticalAlignment.Center
                };
                TextBox tx = new TextBox()
                {
                    Text = string.Empty,
                    ToolTip = tooltips[i],
                    VerticalAlignment = VerticalAlignment.Center,
                    MinWidth = 100,
                    Margin = new Thickness(10),
                    MaxWidth = 300,
                    TextWrapping = TextWrapping.Wrap
                };

                MahApps.Metro.Controls.TextBoxHelper.SetAutoWatermark(tx, true);
                MahApps.Metro.Controls.TextBoxHelper.SetWatermark(tx, pDefaults[i]);

                _textboxes[i] = tx;

                g.Children.Add(l);
                g.Children.Add(tx);

                Grid.SetColumn(l, 0);
                Grid.SetColumn(tx, 1);

                inputsPanel.Children.Add(g);
            }

            return base.ShowDialog();
        }

        private TextBox[] _textboxes;
        public string[] Values
        {
            get
            {
                string[] result = new string[_textboxes.Length];
                string[] pDefaults = defaults ?? new string[_textboxes.Length];

                for (int i = 0; i < _textboxes.Length; i++)
                {
                    if (string.IsNullOrEmpty(_textboxes[i].Text))
                        result[i] = pDefaults[i] ?? string.Empty;
                    else
                        result[i] = _textboxes[i].Text;
                }

                return result;
            }
            set
            {
                if (value.Length != _textboxes.Length)
                {
                    throw new ArgumentException("Invalid length");
                }

                for (int i = 0; i < value.Length; i++)
                {
                    _textboxes[i].Text = value[i];
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
