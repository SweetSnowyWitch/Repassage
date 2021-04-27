using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Repassage
{
    class GameModel
    {
        public GameModel()
        {
            
        }

        public void Start(Form gameForm)
        {
            var lettersTable = new PictureBox();
            var textbox = new PictureBox();

            textbox.Image = Resources.Textbox;
            textbox.Visible = false;
            textbox.BackColor = Color.Transparent;
            textbox.Location = new Point(500, 820);
            textbox.ClientSize = Resources.Textbox.Size;
            gameForm.Controls.Add(textbox);

            lettersTable.Image = Resources.Table;
            lettersTable.BackColor = Color.Transparent;
            lettersTable.Location = new Point(0, 550);
            lettersTable.ClientSize = Resources.Table.Size;
            lettersTable.Click += (sender, args) => textbox.Visible = true;
            gameForm.Controls.Add(lettersTable);
        }

        public void UpdateLetters(int week, int route)
        {

        }
    }
}
