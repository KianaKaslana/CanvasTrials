using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CanvasTrials
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SetupColorDictionary();
        }

        private void SetupColorDictionary()
        {
            _colorDictionary = new Dictionary<string, Color>
            {
                { "Valid", Colors.Blue },
                { "Missing", Colors.Red }
            };
        }

        private void CreateItems(int eps, int[] missingEps)
        {
            ProgressStack.Children.Clear();
            var percentagePerSlice = 1 / (double)eps;

            var entries = new List<Block>();
            for (var r = 0; r < eps; r++)
            {
                var type = missingEps.Contains(r) ? "Missing" : "Valid";
                if (entries.LastOrDefault()?.BlockType == type)
                {
                    entries.Last().Width += percentagePerSlice;
                }
                else
                {
                    entries.Add(new Block(type, _colorDictionary, percentagePerSlice));
                }
            }

            entries.ForEach(x => x.AddRectangleToPanel(ProgressStack));
            CreateTooltip(eps, missingEps);
        }

        private void CreateTooltip(int eps, int[] missingEps)
        {
            ProgressStack.ToolTip = $"Total Episodes:\t{eps}\r\nMissing Episodes:\t{string.Join(",", missingEps)}";
        }
        
        private Dictionary<string, Color> _colorDictionary;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var eps = 12;
            var missingEps = new[] { 3, 6, 7 };
            CreateItems(eps, missingEps);
        }

        /// <summary>
        /// Contains details for setting up entries
        /// </summary>
        private class Block
        {
            /// <summary>
            /// Construct a complete block
            /// </summary>
            /// <param name="blockType">The type of Block</param>
            /// <param name="colorDictionary">Dictionary containing colors. Keys should contian an entry for every type associated with a color</param>
            /// <param name="width">The width as a percentage (0 to 1)</param>
            public Block(string blockType, Dictionary<string, Color> colorDictionary, double width)
            {
                BlockType = blockType;
                Width = width;
                _colorDictionary = colorDictionary;
            }

            /// <summary>
            /// The type of the block.
            /// This value is used to determine the color of the entry
            /// </summary>
            public string BlockType { get; }

            /// <summary>
            /// Retrieve the color of the entry
            /// </summary>
            public Color GetColor => _colorDictionary[BlockType];

            /// <summary>
            /// The width of the entry. This should be set as a percentage (value between 0 and 1)
            /// </summary>
            public double Width { get; set; }

            /// <summary>
            /// Add the Block to a Panel as a rectangle
            /// </summary>
            /// <param name="panel"></param>
            public void AddRectangleToPanel(Panel panel)
            {
                var rect = new Rectangle
                {
                    Width = Width * panel.RenderSize.Width,
                    Height = double.NaN,
                    Fill = new SolidColorBrush(GetColor),
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                panel.Children.Add(rect);
            }

            /// <summary>
            /// Dictionary containing valid colors
            /// </summary>
            private readonly Dictionary<string, Color> _colorDictionary;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var eps = 12;
            var missingEps = new[] { 3, 7 };
            CreateItems(eps, missingEps);
        }
    }
}