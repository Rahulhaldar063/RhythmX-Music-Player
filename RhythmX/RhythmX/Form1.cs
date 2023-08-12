using System.Diagnostics;
using MediaPlayer;
using TagLib;
using WMPLib;
namespace RhythmX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[]? files, paths;

        private void hideSubMenu()
        {
            if (panelExplore.Visible == true)
                panelExplore.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hidePanel();
            panelPlaying.Visible = true;
            hideSubMenu();
        }
        private void btnExplore_Click(object sender, EventArgs e)
        {
            hidePanel();
            panelMusic.Visible = true;
            trackList.Visible = true;
            showSubMenu(panelExplore);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Music Files (*.mp3, *.wav)|*.mp3;*.wav";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                files = ofd.FileNames;
                paths = ofd.FileNames;
                for (int x = 0; x < files.Length; x++)
                {
                    trackList.Items.Add(files[x]);
                }
            }
            hideSubMenu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            hidePanel();
            panelFavourites.Visible = true;
            hideSubMenu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            hidePanel();
            panelEqualizer.Visible = true;
            hideSubMenu();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            hidePanel();
            panelHelp.Visible = true;
            hideSubMenu();
        }

        private void panelPlaying_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void hidePanel()
        {
            if (panelMusic.Visible == true)
                panelMusic.Visible = false;
            if (panelPlaying.Visible == true)
                panelPlaying.Visible = false;
            if (panelEqualizer.Visible == true)
                panelEqualizer.Visible = false;
            if (panelFavourites.Visible == true)
                panelFavourites.Visible = false;
            if (panelHelp.Visible == true)
                panelHelp.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            startTime.Font = new Font("DigifaceWide", 10, FontStyle.Regular);
            endTime.Font = new Font("DigifaceWide", 10, FontStyle.Regular);
            lblVolume.Font = new Font("DigifaceWide", 9, FontStyle.Regular);
        }

        WindowsMediaPlayer Player = new WindowsMediaPlayer();
        private void PlayFile(String url)
        {
            //Player = new WindowsMediaPlayer();
            Player.PlayStateChange +=
                new _WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
            Player.MediaError +=
                new _WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
            Player.URL = url;
            Player.controls.play();
        }

        private void Player_PlayStateChange(int NewState)
        {
            if ((WMPPlayState)NewState == WMPPlayState.wmppsStopped)
            {
                this.Close();
            }

        }
        private void Player_MediaError(object pMediaObject)
        {
            MessageBox.Show("Cannot play media file.");
            this.Close();
        }

        private void trackList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Player.URL = paths?[trackList.SelectedIndex];
                Player.controls.play();
                pictureBox1.Enabled = Player.status.ToLower().Contains("connecting");
                try
                {
                    var file = TagLib.File.Create(paths?[trackList.SelectedIndex]);
                    IPicture? picture = file.Tag.Pictures.FirstOrDefault();
                    if (picture != null)
                    {
                        byte[] data = picture.Data.Data;
                        Image albumArt;
                        using (var stream = new System.IO.MemoryStream(data))
                        {
                            albumArt = Image.FromStream(stream);
                        }
                        pictureBox2.Image = albumArt;
                        pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                        string title = file.Tag.Title;
                        string artist = file.Tag.FirstPerformer;
                        string album = file.Tag.Album;
                        string name = $"{title} - {artist} | {album}";

                        if (!string.IsNullOrEmpty(title))
                        {
                            lblTitle.Text = title;
                        }
                        else
                        {
                            lblTitle.Text = "Unknown Title";
                        }

                        if (!string.IsNullOrEmpty(artist))
                        {
                            lblArtist.Text = artist;
                        }
                        else
                        {
                            lblArtist.Text = "Unknown Artist";
                        }

                        if (!string.IsNullOrEmpty(album))
                        {
                            lblAlbum.Text = album;
                        }
                        else
                        {
                            lblAlbum.Text = "Unknown Album";
                        }

                        if (!string.IsNullOrEmpty(name))
                        {
                            lblHeader.Text = name;
                        }
                        else
                        {
                            lblHeader.Text = "Unknown Album";
                        }
                    }
                }
                catch { }
            }
            catch (Exception) { MessageBox.Show("Select a music from the list", "RhythmX", MessageBoxButtons.OK); }
            btnFav.Enabled = true;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (Player.status.ToLower().Contains("playing"))
                Player.controls.pause();
            else
                Player.controls.play();
            pictureBox1.Enabled = Player.status.ToLower().Contains("playing");
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (trackList.SelectedIndex < trackList.Items.Count - 1)
            {
                trackList.SelectedIndex = trackList.SelectedIndex + 1;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (trackList.SelectedIndex > 0)
            {
                trackList.SelectedIndex = trackList.SelectedIndex - 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Player.controls.stop();
            pictureBox1.Enabled = Player.status.ToLower().Contains("playing");
        }

        private void btnSound_Click(object sender, EventArgs e)
        {
            if (panelSound.Visible == false)
            {
                panelSound.Visible = true;
            }
            else
            {
                panelSound.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                startTime.Text = Player.controls.currentPositionString;
                endTime.Text = Player.controls.currentItem.durationString;

                slider.Maximum = (int)Player.controls.currentItem.duration;
                slider.Value = (int)Player.controls.currentPosition;
                try
                {
                    if (((int)Player.controls.currentItem.duration) - 2 == (int)Player.controls.currentPosition)
                    {
                        if (trackList.SelectedIndex < trackList.Items.Count - 1)
                        {
                            trackList.SelectedIndex = trackList.SelectedIndex + 1;
                            timer1.Start();
                        }
                        else
                        {
                            trackList.SelectedIndex = 0;
                            timer1.Start();
                        }
                    }
                }
                catch (Exception) { timer1.Stop(); }
            }
        }

        private void colorSlider1_Scroll(object sender, ScrollEventArgs e)
        {
            Player.settings.volume = (int)colorSlider1.Value;
            lblVolume.Text = colorSlider1.Value.ToString() + "%";
        }

        private void lblVolume_Click(object sender, EventArgs e)
        {

        }

        private void slider_MouseDown(object sender, MouseEventArgs e)
        {
            Player.controls.currentPosition = Player.currentMedia.duration * e.X / slider.Width;
        }

        private void btnFav_Click(object sender, EventArgs e)
        {
            btnFav.Enabled = false;
            favouriteList.Items.Add(trackList.SelectedItem);
            btnSave.Enabled = true;
            btnLoad.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            StreamWriter save = new StreamWriter(@"C:\Users\Rahul Haldar\Desktop\Projects\RhythmX\RhythmX\Storage\Favourites.txt");
            foreach (var item in favouriteList.Items)
            {
                save.WriteLine(item.ToString());
                this.Refresh();
            }
            MessageBox.Show("Saved to Favourite List.", "RhythmX", MessageBoxButtons.OK, MessageBoxIcon.Information);
            save.Close();
            favouriteList.Items.Clear();
            btnSave.Enabled = false;
            btnLoad.Enabled = true;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (StreamReader read = new StreamReader(@"C:\Users\Rahul Haldar\Desktop\Projects\RhythmX\RhythmX\Storage\Favourites.txt"))
            {
                string line;
                while ((line = read.ReadLine()) != null)
                {
                    favouriteList.Items.Add(line);
                }
            }
            btnLoad.Enabled = false;
        }

        private void favouriteList_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                Player.URL = favouriteList.SelectedItem.ToString();
                Player.controls.play();
                pictureBox1.Enabled = Player.status.ToLower().Contains("connecting");
                try
                {
                    var file = TagLib.File.Create(favouriteList.SelectedItem.ToString());
                    IPicture? picture = file.Tag.Pictures.FirstOrDefault();
                    if (picture != null)
                    {
                        byte[] data = picture.Data.Data;
                        Image albumArt;
                        using (var stream = new System.IO.MemoryStream(data))
                        {
                            albumArt = Image.FromStream(stream);
                        }
                        pictureBox2.Image = albumArt;
                        pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                        string title = file.Tag.Title;
                        string artist = file.Tag.FirstPerformer;
                        string album = file.Tag.Album;
                        string name = $"{title} - {artist} | {album}";

                        if (!string.IsNullOrEmpty(title))
                        {
                            lblTitle.Text = title;
                        }
                        else
                        {
                            lblTitle.Text = "Unknown Title";
                        }

                        if (!string.IsNullOrEmpty(artist))
                        {
                            lblArtist.Text = artist;
                        }
                        else
                        {
                            lblArtist.Text = "Unknown Artist";
                        }

                        if (!string.IsNullOrEmpty(album))
                        {
                            lblAlbum.Text = album;
                        }
                        else
                        {
                            lblAlbum.Text = "Unknown Album";
                        }

                        if (!string.IsNullOrEmpty(name))
                        {
                            lblHeader.Text = name;
                        }
                        else
                        {
                            lblHeader.Text = "Unknown Album";
                        }
                    }
                }
                catch { }
            }
            catch (Exception) { MessageBox.Show("Select a music from the list", "RhythmX", MessageBoxButtons.OK); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (favouriteList.SelectedItems.Count > 0)
            {
                favouriteList.Items.Remove(favouriteList.SelectedItems[0]);
            }
        }

        private void panelHelp_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.facebook.com/rahul.haldar.940641?mibextid=ZbWKwL";
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception)
            {
                MessageBox.Show("There is an Error to open the link.");
            }
        }
    }
}