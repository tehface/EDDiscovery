﻿/*
 * Copyright © 2019 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EliteDangerousCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class CommanderForm : ExtendedControls.DraggableForm
    {
        public CommanderForm()
        {
            InitializeComponent();
            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyDialog(this);
            panelTop.Visible = panelTop.Enabled = !winborder;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip,this);
        }

        private void CommanderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void Init(bool enablecmdredit)
        {
            textBoxBorderCmdr.Enabled = enablecmdredit;
        }

        public void Init(EDCommander cmdr, bool enablecmdredit)
        {
            textBoxBorderCmdr.Text = cmdr.Name;
            textBoxBorderCmdr.Enabled = enablecmdredit;
            textBoxBorderJournal.Text = cmdr.JournalDir;
            textBoxBorderEDSMName.Text = cmdr.EdsmName;
            textBoxBorderEDSMAPI.Text = cmdr.EDSMAPIKey;
            checkBoxCustomEDSMFrom.Checked = cmdr.SyncFromEdsm;
            checkBoxCustomEDSMTo.Checked = cmdr.SyncToEdsm;
            checkBoxCustomEDDNTo.Checked = cmdr.SyncToEddn;
            checkBoxEGOSync.Checked = cmdr.SyncToEGO;
            textBoxEGOName.Text = cmdr.EGOName;
            textBoxEGOAPI.Text = cmdr.EGOAPIKey;
            textBoxBorderInaraAPIKey.Text = cmdr.InaraAPIKey;
            textBoxBorderInaraName.Text = cmdr.InaraName;
            checkBoxCustomInara.Checked = cmdr.SyncToInara;

            extTextBoxAutoCompleteHomeSystem.Text = cmdr.HomeSystem;
            extTextBoxAutoCompleteHomeSystem.SetAutoCompletor(EliteDangerousCore.DB.SystemCache.ReturnSystemAutoCompleteList, true);

            textBoxDefaultZoom.ValueNoChange = cmdr.MapZoom;

            bool selectionCentre = cmdr.MapCentreOnSelection;
            radioButtonHistorySelection.Checked = selectionCentre;
            radioButtonCentreHome.Checked = !selectionCentre;

            panel_defaultmapcolor.BackColor = System.Drawing.Color.FromArgb(cmdr.MapColour);
            panel_defaultmapcolor.Click += Panel_defaultmapcolor_Click;
        }

        public bool Update(EDCommander cmdr)
        {
            bool update = cmdr.JournalDir != textBoxBorderJournal.Text ||                   // changing these means need to resync system and start up stuff
                          cmdr.EdsmName != textBoxBorderEDSMName.Text ||
                          cmdr.EDSMAPIKey != textBoxBorderEDSMAPI.Text ||
                          cmdr.SyncFromEdsm != checkBoxCustomEDSMFrom.Checked ||
                          cmdr.SyncToEdsm != checkBoxCustomEDSMTo.Checked;

            cmdr.Name = textBoxBorderCmdr.Text;
            cmdr.JournalDir = textBoxBorderJournal.Text;
            cmdr.EdsmName = textBoxBorderEDSMName.Text;
            cmdr.EDSMAPIKey = textBoxBorderEDSMAPI.Text;
            cmdr.SyncFromEdsm = checkBoxCustomEDSMFrom.Checked;
            cmdr.SyncToEdsm = checkBoxCustomEDSMTo.Checked;
            cmdr.SyncToEddn = checkBoxCustomEDDNTo.Checked;
            cmdr.SyncToEGO = checkBoxEGOSync.Checked;
            cmdr.EGOName = textBoxEGOName.Text;
            cmdr.EGOAPIKey = textBoxEGOAPI.Text;
            cmdr.InaraAPIKey = textBoxBorderInaraAPIKey.Text;
            cmdr.InaraName = textBoxBorderInaraName.Text;
            cmdr.SyncToInara = checkBoxCustomInara.Checked;
            cmdr.HomeSystem = extTextBoxAutoCompleteHomeSystem.Text;
            cmdr.MapZoom = float.TryParse(textBoxDefaultZoom.Text, out float res) ? res : 1.0f;
            cmdr.MapCentreOnSelection = radioButtonHistorySelection.Checked;
            cmdr.MapColour = panel_defaultmapcolor.BackColor.ToArgb();

            return update;
        }


        public bool Valid { get { return textBoxBorderCmdr.Text != ""; } }
        public string CommanderName { get { return textBoxBorderCmdr.Text; } }

        #region UI

        private void Panel_defaultmapcolor_Click(object sender, EventArgs e)
        {
            ColorDialog mapColorDialog = new ColorDialog();
            mapColorDialog.AllowFullOpen = true;
            mapColorDialog.FullOpen = true;
            mapColorDialog.Color = panel_defaultmapcolor.BackColor;
            if (mapColorDialog.ShowDialog(FindForm()) == DialogResult.OK)
            {
                panel_defaultmapcolor.BackColor = mapColorDialog.Color;
            }
        }

        private void buttonExtBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select folder where Journal*.log files are stored by Frontier in".T(EDTx.CommanderForm_LF);

            if (fbd.ShowDialog(this) == DialogResult.OK)
                textBoxBorderJournal.Text = fbd.SelectedPath;

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion

        #region Window Control

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }


        #endregion

       
    }
}
