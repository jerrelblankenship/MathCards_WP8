using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MathBlaster.Presenters;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MathBlaster.Screen
{
    public interface IProblemPage
    {
        string ApplicationTitleText { get; set; }
        string LevelTitleText { get; set; }
        string Number1Value { get; set; }
        string Number2Value { get; set; }
        string DisplayResultValue { get; set; }
        string CurrentQuestionNumber { get; set; }
        string TotalQuestionNumber { get; set; }
        string ScoreTotal { get; set; }
        string OperandValue { get; set; }
        string BackButtonValue { get; set; }
        string EqualSignValue { get; set; }
        string CorrectAnswerValue { get; set; }
        Brush DisplayResultColor { get; set; }
        ImageSource ResultImage { get; set; }
        Visibility NextButtonVisibility { get; set; }
        Visibility SubmitButtonVisibility { get; set; }
        Visibility KeypadVisibility { get; set; }
        Popup QuizCompletePopup { get; set; }
    }

    public partial class ProblemPage : PhoneApplicationPage, IProblemPage
    {
        private readonly ProblemPagePresenter presenter;
        private readonly VibrateController vc = VibrateController.Default;
        
        #region Implementation of IProblemPage

        public string ApplicationTitleText
        {
            get { return ApplicationTitle.Text; }
            set { ApplicationTitle.Text = value; }
        }

        public string LevelTitleText
        {
            get { return LevelTitle.Text; }
            set { LevelTitle.Text = value; }
        }

        public string ScoreTotal
        {
            get { return scoreTotal.Text; }
            set { scoreTotal.Text = value; }
        }

        public string EqualSignValue
        {
            get { return EqualSign.Text; }
            set { EqualSign.Text = value; }
        }

        public string CorrectAnswerValue
        {
            get { return CorrectAnswer.Text; }
            set { CorrectAnswer.Text = value; }
        }

        public Brush DisplayResultColor
        {
            get { return DisplayResult.Foreground; }
            set { DisplayResult.Foreground = value; }
        }

        public string OperandValue
        {
            get { return Operand.Text; }
            set { Operand.Text = value; }
        }

        public string BackButtonValue
        {
            get { return BackValue.Content.ToString(); }
            set { BackValue.Content = value; }
        }

        public ImageSource ResultImage
        {
            get { return Result.Source; }
            set { Result.Source = value; }
        }

        public string Number1Value
        {
            get { return Number1.Text; }
            set { Number1.Text = value; }
        }

        public string Number2Value
        {
            get { return Number2.Text; }
            set { Number2.Text = value; }
        }

        public string DisplayResultValue
        {
            get { return DisplayResult.Text; }
            set { DisplayResult.Text = value; }
        }

        public string CurrentQuestionNumber
        {
            get { return currentQuestionNumber.Text; }
            set { currentQuestionNumber.Text = value; }
        }

        public string TotalQuestionNumber
        {
            get { return totalQuestionNumber.Text; }
            set { totalQuestionNumber.Text = value; }
        }

        public Visibility NextButtonVisibility
        {
            get { return ClickButton.Visibility; }
            set { ClickButton.Visibility = value; }
        }

        public Visibility SubmitButtonVisibility
        {
            get { return SubmitButton.Visibility; }
            set { SubmitButton.Visibility = value; }
        }

        public Visibility KeypadVisibility
        {
            get { return keyPad.Visibility; }
            set { keyPad.Visibility = value; }
        }

        public Popup QuizCompletePopup { get; set; }

        #endregion

        public ProblemPage()
        {
            InitializeComponent();

            presenter = new ProblemPagePresenter(this, new AppSettingsStorage());

            BackValue.Content = "\xD5";
            presenter.SetupPage();
            presenter.SetProblemNumbers();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State.ContainsKey("MidQuiz"))
            {
                presenter.LoadState();
            }
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            presenter.SaveState();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrEmpty(DisplayResult.Text))
            {
                var result = presenter.SubmitAnswer();
                if (!result)
                    vc.Start(TimeSpan.FromMilliseconds(100));

                keyPad.IsHitTestVisible = false;
            }
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            var currentValue = DisplayResult.Text;
            var button = sender as Button;

            if (button != null && DisplayResult.Text.Length < 4)
            {
                DisplayResult.Text = currentValue + button.Content;
            }
            else if (DisplayResult.Text.Length >= 4)
            {
                vc.Start(TimeSpan.FromMilliseconds(100));
            }
        }

        private void NextProblemButton_Click(object sender, RoutedEventArgs e)
        {
            var quizIsNotFinished = presenter.SetupForNextProblem();
            if (quizIsNotFinished)
            {
                presenter.SetProblemNumbers();
                keyPad.IsHitTestVisible = true;
            }
            else
            {
                LaunchQuizCompletePopup();
            }
        }

        private void LaunchQuizCompletePopup()
        {
            LayoutRoot.IsHitTestVisible = false;
            presenter.SetupPopup();
            QuizCompletePopup.Closed += Popup_Closed;
        }

        void Popup_Closed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screen/StartPage.xaml", UriKind.Relative));
        }

        private void BackValue_Click(object sender, RoutedEventArgs e)
        {
            DisplayResult.Text = DisplayResult.Text.BackSpace();
        }

        private void GestureListener_Hold(object sender, GestureEventArgs e)
        {
            DisplayResult.Text = string.Empty;
        }

        private void AnswerButton_Click(object sender, EventArgs e)
        {
            if (keyPad.IsHitTestVisible)
            {
                presenter.AnswerQuestion();
                keyPad.IsHitTestVisible = false;
                 vc.Start(TimeSpan.FromMilliseconds(100));
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (QuizCompletePopup != null)
            {
                if (QuizCompletePopup.IsOpen)
                {
                    QuizCompletePopup.IsOpen = false; // Hide the popup
                    e.Cancel = true; // Cancel subsequent BackKey navigation
                    LayoutRoot.IsHitTestVisible = true; // Restore mouse hit on main page
                }
            }

            base.OnBackKeyPress(e); // Call base
        }
    }
}