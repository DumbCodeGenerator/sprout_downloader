using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Sprout_Downloader
{
    public partial class QualitySelector : Form
    {
        private readonly IEnumerable<string> _playlists;

        public QualitySelector(IEnumerable<string> playlists)
        {
            _playlists = playlists;
            InitializeComponent();
        }

        public int GetSelectedIndex()
        {
            return radioListBox1.SelectedIndex;
        }

        private void QualitySelector_Load(object sender, EventArgs e)
        {
            radioListBox1.Items.AddRange(_playlists.ToArray());
            radioListBox1.SetSelected(radioListBox1.Items.Count - 1, true);
        }
    }
}