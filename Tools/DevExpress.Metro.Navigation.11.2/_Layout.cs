namespace DevExpress.Metro.Navigation {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Text;
    using System.Windows.Forms;
    using DevExpress.XtraEditors;
    using DevExpress.Metro.Navigation.Properties;
    public partial class _Layout : DevExpress.XtraEditors.XtraUserControl, IChrome {
        public _Layout() {
            InitializeComponent();
            if (LookAndFeel.ActiveSkinName == "Metropolis") {
                this.back.Image = Resources.ArrowBlack;
            } else {
                this.back.Image = Resources.ArrowNormal;
            }
            this.back.MouseClick += Back_MouseClick;
            this.back.MouseHover += Back_MouseHover;
            this.back.MouseLeave += Back_MouseLeave;
        }

        Control _previous;

        void IChrome.Navigate(Control previous) {
            _previous = previous;
        }

        public override string Text {
            get {
                return this.title.Text;
            }
            set {
                this.title.Text = value;
            }
        }

        void Back_MouseLeave(object sender, EventArgs e) {
            if (LookAndFeel.ActiveSkinName == "Metropolis") {
                this.back.Image = Resources.ArrowBlack;
            } else {
                this.back.Image = Resources.ArrowNormal;
            }
        }

        void Back_MouseHover(object sender, EventArgs e) {
            if (LookAndFeel.ActiveSkinName == "Metropolis") {
                this.back.Image = Resources.ArrowNormal;
            } else {
                this.back.Image = Resources.ArrowHover;
            }
        }

        void Back_MouseClick(object sender, MouseEventArgs e) {
            this.Parent.GoTo(_previous, null);
        }

        private void _Layout_Load(object sender, EventArgs e)
        {

        }

    }
}
