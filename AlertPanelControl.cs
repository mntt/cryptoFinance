﻿using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class AlertPanelControl
    {      
        private System.Timers.Timer animationTimer = new System.Timers.Timer();
        private System.Timers.Timer disableAlertTimer = new System.Timers.Timer();
        private int priceAlertSeconds = 3;
        private Panel mainPanel { get; set; }
        private Label mainLabel { get; set; }
        private int x { get; set; }
        private int y { get; set; }
        private int width { get; set; }
        private int height { get; set; }
        private int labelWidth { get; set; }
        private int labelHeight { get; set; }

        public AlertPanelControl(Panel _mainPanel, Label _mainLabel, 
            int _x, int _y, int _width, int _height, int _labelWidth, int _labelHeight)
        {
            mainPanel = _mainPanel;
            mainLabel = _mainLabel;
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            labelWidth = _labelWidth;
            labelHeight = _labelHeight;
            SetTimersParameters();
        }

        private void SetTimersParameters()
        {
            animationTimer.Elapsed += new ElapsedEventHandler(MainTimer_Tick);
            animationTimer.Interval = 10;
            disableAlertTimer.Elapsed += new ElapsedEventHandler(SecondaryTimer_Tick);
            disableAlertTimer.Interval = 1000;
        }

        public void StartPanelAnimation(int chooseLabelText)
        {
            mainPanel.BorderStyle = BorderStyle.None;
            mainPanel.SetBounds(x, y, width, height);
            mainLabel.SetBounds(4, 4, labelWidth, labelHeight);

            if(chooseLabelText <= 15)
            {
                mainPanel.BackColor = Color.LightCoral;
                ErrorTexts(chooseLabelText);
            }
            
            if(chooseLabelText > 20 && chooseLabelText < 25)
            {
                mainPanel.BackColor = Color.LightGreen;
                SuccessTexts(chooseLabelText);
            }

            mainPanel.Visible = true;
            animationTimer.Start();
        }

        private void ErrorTexts(int chooseLabelText)
        {
            switch (chooseLabelText)
            {
                case 0:
                    mainLabel.Text = "Ryšio problemos.\nNepavyko atnaujinti visų kainų.";
                    break;
                case 1:
                    mainLabel.Text = "Sąrašo atnaujinimas atšauktas.";
                    break;
                case 2:
                    mainLabel.Text = "Ryšio problemos.\nDabartinė vertė paskaičiuota su senomis kainomis.";
                    break;
                case 3:
                    mainLabel.Text = "Formoje yra klaidų arba tuščių langelių.\nPataisykite ir bandykite dar kartą.";
                    break;
                case 4:
                    mainLabel.Text = "Įvestas kiekis didesnis nei\nmaksimali galima reikšmė.";
                    break;
                case 5:
                    mainLabel.Text = "Piniginė nerasta,\npasirinkite piniginę iš sąrašo.";
                    break;
                case 6:
                    mainLabel.Text = "Nepavyko užkrauti kainos.\nĮveskite kainą rankiniu būdu.";
                    break;
            }
        }

        private void SuccessTexts(int chooseLabelText)
        {
            switch (chooseLabelText)
            {
                case 21:
                    mainLabel.Text = "Kriptovaliutų sąrašas atnaujintas!";
                    break;
                case 22:
                    mainLabel.Text = "Duomenys sėkmingai išsaugoti!";
                    break;
                case 23:
                    mainLabel.Text = "Perkėlimas įvykdytas sėkmingai!";
                    break;
                case 24:
                    mainLabel.Text = "Nėra ryšio. Duomenys išsaugoti\nnaudojant senas kainas.";
                    break;
            }
        }

        private void MainTimer_Tick(object source, EventArgs e)
        {
            mainPanel.SetBounds(x, y, width, height);
            mainPanel.Visible = true;
            y++;

            if (y >= 3)
            {
                animationTimer.Stop();
                disableAlertTimer.Start();
            }
        }

        private void SecondaryTimer_Tick(object source, EventArgs e)
        {
            priceAlertSeconds--;

            if (priceAlertSeconds == 0)
            {
                disableAlertTimer.Stop();
                mainPanel.Visible = false;
            }
        }

    }
}
