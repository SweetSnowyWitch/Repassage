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
            var characterText = new PictureBox();

            characterText.Image = Resources.Textbox;
            characterText.Visible = false;
            characterText.BackColor = Color.Transparent;
            characterText.Location = new Point(500, 820);
            characterText.ClientSize = Resources.Textbox.Size;
            gameForm.Controls.Add(characterText);

            var letter = new TextBox();

            letter.Visible = false;
            letter.WordWrap = true;
            letter.Multiline = true;
            letter.ReadOnly = true;
            letter.Text = "Тестовый текст";
            letter.Font = new Font("Honey ScriptCYR", 48);
            letter.BackColor = Color.AntiqueWhite;
            letter.Location = new Point(450, 100);
            letter.ScrollBars = ScrollBars.Vertical;
            letter.ClientSize = new Size(1000, 800);
            letter.Click += (sender, args) => letter.Visible = false;
            gameForm.Controls.Add(letter);

            var lettersTable = new PictureBox();

            lettersTable.Image = Resources.Table;
            lettersTable.BackColor = Color.Transparent;
            lettersTable.Location = new Point(0, 550);
            lettersTable.ClientSize = Resources.Table.Size;
            lettersTable.Click += (sender, args) => letter.Visible = true;
            gameForm.Controls.Add(lettersTable);
        }

        public void UpdateLetters(int week, int route)
        {

        }
    }
}
