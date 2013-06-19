using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Devices;

namespace MathBlaster.UserControls
{
    public partial class QuizCompleteScreen : UserControl
    {
        private readonly VibrateController vc = VibrateController.Default;

        public QuizCompleteScreen(decimal numberOfCorrectAnswers, decimal numberOfProblems)
        {
            InitializeComponent();
            scoreTotal.Text = string.Format("{0} / {1}", numberOfCorrectAnswers, numberOfProblems);

            var percent = Convert.ToInt32((numberOfCorrectAnswers / numberOfProblems) * 100);
            percentScoreTotal.Text = string.Format("{0} %", percent);
        }

        private void FinishedButton_Click(object sender, RoutedEventArgs e)
        {
            vc.Start(TimeSpan.FromMilliseconds(100));
            var p = this.Parent as Popup;
            p.IsOpen = false;
        }
    }
}
