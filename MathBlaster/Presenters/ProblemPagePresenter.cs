﻿using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MathBlaster.Screen;
using MathBlaster.UserControls;
using Microsoft.Phone.Shell;

namespace MathBlaster.Presenters
{
    public class ProblemPagePresenter
    {
        private const string AdditionQuizTitle = "Addition Quiz";
        private const string SubtractionQuizTitle = "Subtraction Quiz";
        private const string MultiplicationQuizTitle = "Multiplication Quiz";
        private const string DivisionQuizTitle = "Division Quiz";

        private readonly IProblemPage problemPageView;
        private readonly AppSettingsStorage storageSettings;
        private string operandString;
        private decimal number1;
        private decimal number2;
        private readonly decimal numberOfProblems;
        private decimal numberOfCorrectAnswers;
        private decimal currentProblemNumber;
        private readonly Random randomProblemGenerator;
        private Brush previousBrush;

        public ProblemPagePresenter(IProblemPage view, AppSettingsStorage storage)
        {
            problemPageView = view;
            storageSettings = storage;

            randomProblemGenerator = new Random();
            numberOfProblems = storageSettings.NumberOfProblemsSetting;
            numberOfCorrectAnswers = 0;
            currentProblemNumber = 1;
        }

        public void SaveState()
        {
            PhoneApplicationService.Current.State["NumberOfCorrectAnswers"] = numberOfCorrectAnswers;
            PhoneApplicationService.Current.State["CurrentProblemNumber"] = currentProblemNumber;
            PhoneApplicationService.Current.State["Number1"] = problemPageView.Number1Value;
            PhoneApplicationService.Current.State["Number2"] = problemPageView.Number2Value;
        }

        public void LoadState()
        {
            if (PhoneApplicationService.Current.State.ContainsKey("NumberOfCorrectAnswers"))
                numberOfCorrectAnswers = (decimal) PhoneApplicationService.Current.State["NumberOfCorrectAnswers"];

            if (PhoneApplicationService.Current.State.ContainsKey("CurrentProblemNumber"))
                currentProblemNumber = (decimal) PhoneApplicationService.Current.State["CurrentProblemNumber"];

            if (PhoneApplicationService.Current.State.ContainsKey("Number1"))
                problemPageView.Number1Value = (string) PhoneApplicationService.Current.State["Number1"];

            if (PhoneApplicationService.Current.State.ContainsKey("Number2"))
                problemPageView.Number2Value = (string)PhoneApplicationService.Current.State["Number2"];

            SetScoreStats();
            PhoneApplicationService.Current.State.Clear();
        }

        public void SetupPage()
        {
            switch (storageSettings.OperationToPerformSetting)
            {
                case OperatorEnum.addition:
                    problemPageView.ApplicationTitleText = AdditionQuizTitle;
                    problemPageView.OperandValue = "\u002B";
                    operandString = "+";
                    break;
                case OperatorEnum.multiplication:
                    problemPageView.ApplicationTitleText = MultiplicationQuizTitle;
                    problemPageView.OperandValue = "\u00D7";
                    operandString = "*";
                    break;
                case OperatorEnum.subtraction:
                    problemPageView.ApplicationTitleText = SubtractionQuizTitle;
                    problemPageView.OperandValue = "\u2212";
                    operandString = "-";
                    break;
                case OperatorEnum.division:
                    problemPageView.ApplicationTitleText = DivisionQuizTitle;
                    problemPageView.OperandValue = "\u00F7";
                    operandString = "/";
                    break;
            }

            problemPageView.LevelTitleText = string.Format("Level: {0}", storageSettings.ProblemLevelSetting);
            problemPageView.EqualSignValue = "\u003D";

            SetScoreStats();
        }

        private void SetScoreStats()
        {
            problemPageView.CurrentQuestionNumber = currentProblemNumber.ToString();
            problemPageView.TotalQuestionNumber = numberOfProblems.ToString();
            problemPageView.ScoreTotal = string.Format("{0} / {1}", numberOfCorrectAnswers, numberOfProblems);
        }

        public void SetProblemNumbers()
        {
            var num1 = randomProblemGenerator.Next(10);
            var num2 = randomProblemGenerator.Next(10);

            // if subtraction test make sure num1 is larger or equal to num2
            if (storageSettings.OperationToPerformSetting == OperatorEnum.subtraction)
            {
                var validNumbers = num1 >= num2;

                while (!validNumbers)
                {
                    num1 = randomProblemGenerator.Next(10);
                    num2 = randomProblemGenerator.Next(10);
                    validNumbers = num1 >= num2;
                }
            }

            // if division test make sure num1 / num2 remainder equals 0
            if (storageSettings.OperationToPerformSetting == OperatorEnum.division)
            {
                var validNumbers = (num2 != 0) && (num1 % num2 == 0);

                while (!validNumbers)
                {
                    num1 = randomProblemGenerator.Next(10);
                    num2 = randomProblemGenerator.Next(10);
                    validNumbers = (num2 != 0) && (num1 % num2 == 0);
                }
            }

            if (storageSettings.ProblemLevelSetting > 1)
            {
                num1 = num1*10;

                if (storageSettings.ProblemLevelSetting == 3)
                {
                    num2 = num2*10;
                }
            }

            problemPageView.Number1Value = num1.ToString();
            problemPageView.Number2Value = num2.ToString();

            problemPageView.NextButtonVisibility = Visibility.Collapsed;
            problemPageView.SubmitButtonVisibility = Visibility.Visible;
        }

        public bool SubmitAnswer()
        {
            problemPageView.KeypadVisibility = Visibility.Collapsed;

            GetProblemNumbers();
            decimal submittedValue;
            var parseSuccessful = decimal.TryParse(problemPageView.DisplayResultValue, out submittedValue);

            if (parseSuccessful)
            {
                var calculatedResult = CalculateCorrectAnswer();

                return SetAnswerResponse(calculatedResult == submittedValue, false);
            }

            return SetAnswerResponse(false, true);
        }

        private bool SetAnswerResponse(bool correctAnswer, bool computerAnswer)
        {
            previousBrush = problemPageView.DisplayResultColor;
            var result = false;

            if (correctAnswer)
            {
                problemPageView.ResultImage = new BitmapImage(new Uri("../Resources/smileyRight.png", UriKind.Relative));
                problemPageView.DisplayResultColor = new SolidColorBrush(Colors.Green);
                numberOfCorrectAnswers++;
                result = true;
            }
            else
            {
                problemPageView.ResultImage = new BitmapImage(new Uri("../Resources/smileyWrong.png", UriKind.Relative));
                problemPageView.DisplayResultColor = new SolidColorBrush(Colors.Red);
                problemPageView.CorrectAnswerValue = string.Format("The correct answer is: {0}", CalculateCorrectAnswer());
            }

            if (computerAnswer)
            {
                problemPageView.DisplayResultValue = CalculateCorrectAnswer().ToString();
                problemPageView.CorrectAnswerValue = string.Format("The correct answer is: {0}", CalculateCorrectAnswer());
            }

            problemPageView.NextButtonVisibility = Visibility.Visible;
            problemPageView.SubmitButtonVisibility = Visibility.Collapsed;

            return result;
        }

        public bool AnswerQuestion()
        {
            problemPageView.KeypadVisibility = Visibility.Collapsed;

            GetProblemNumbers();

            return SetAnswerResponse(false, true);
        }

        private void GetProblemNumbers()
        {
            number1 = decimal.Parse(problemPageView.Number1Value);
            number2 = decimal.Parse(problemPageView.Number2Value);
        }

        public bool SetupForNextProblem()
        {
            var tempCurrentNumber = currentProblemNumber + 1;
            var quizIsNotFinished = true;

            if (tempCurrentNumber <= numberOfProblems)
            {
                problemPageView.DisplayResultValue = string.Empty;
                problemPageView.DisplayResultColor = previousBrush;
                problemPageView.ResultImage = null;
                currentProblemNumber++;
                problemPageView.KeypadVisibility = Visibility.Visible;
                problemPageView.CorrectAnswerValue = string.Empty;
            }
            else
            {
                quizIsNotFinished = false;
            }

            SetScoreStats();

            return quizIsNotFinished;
        }

        public void SetupPopup()
        {
            var pop = new Popup
            {
                Child = new QuizCompleteScreen(numberOfCorrectAnswers, numberOfProblems),
                IsOpen = true,
                VerticalOffset = 225,
                HorizontalOffset = 50
            };

            problemPageView.QuizCompletePopup = pop;
        }

        private decimal CalculateCorrectAnswer()
        {
            decimal result = 0;
            switch (operandString)
            {
                case "+":
                    result = number1 + number2;
                    break;
                case "*":
                    result = number1 * number2;
                    break;
                case "-":
                    result = number1 - number2;
                    break;
                case "/":
                    result = number1/number2;
                    break;
            }

            return result;
        }
    }
}
