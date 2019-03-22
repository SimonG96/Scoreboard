using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Scoreboard.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static string DEFAULT_NAME_LEFT = "Name_Left";
        public static string DEFAULT_NAME_RIGHT = "Name_Right";
        public static int DEFAULT_STEP_LEFT = 3;
        public static int DEFAULT_STEP_RIGHT = 2;

        private string _nameLeft;
        private string _nameRight;
        private string _scoreTextLeft;
        private string _scoreTextRight;
        private string _buttonTextLeft;
        private string _buttonTextRight;
        private int _scoreLeft;
        private int _scoreRight;
        private int _stepLeft;
        private int _stepRight;
        private bool _areGridSplittersEnabled = true;
        private Visibility _isMenuVisible = Visibility.Hidden;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainWindowViewModel()
        {
            NameLeft = DEFAULT_NAME_LEFT;
            NameRight = DEFAULT_NAME_RIGHT;
            ScoreLeft = 0;
            ScoreRight = 0;
            StepLeft = DEFAULT_STEP_LEFT;
            StepRight = DEFAULT_STEP_RIGHT;

            Steps = new List<Step>();
            UndoneSteps = new List<Step>();
        }

        public string NameLeft
        {
            get { return _nameLeft; }
            set
            {
                _nameLeft = value;
                RaisePropertyChanged(() => NameLeft);
            }
        }

        public string NameRight
        {
            get { return _nameRight; }
            set
            {
                _nameRight = value;
                RaisePropertyChanged(() => NameRight);
            }
        }

        public string ScoreTextLeft
        {
            get { return _scoreTextLeft; }
            set
            {
                _scoreTextLeft = value;
                RaisePropertyChanged(() => ScoreTextLeft);
            }
        }

        public string ScoreTextRight
        {
            get { return _scoreTextRight; }
            set
            {
                _scoreTextRight = value;
                RaisePropertyChanged(() => ScoreTextRight);
            }
        }

        public string ButtonTextLeft
        {
            get { return _buttonTextLeft; }
            set
            {
                _buttonTextLeft = value;
                RaisePropertyChanged(() => ButtonTextLeft);
            }
        }

        public string ButtonTextRight
        {
            get { return _buttonTextRight; }
            set
            {
                _buttonTextRight = value;
                RaisePropertyChanged(() => ButtonTextRight);
            }
        }

        public int ScoreLeft
        {
            get { return _scoreLeft; }
            set
            {
                _scoreLeft = value;
                ScoreTextLeft = _scoreLeft.ToString();
            }
        }

        public int ScoreRight
        {
            get { return _scoreRight; }
            set
            {
                _scoreRight = value;
                ScoreTextRight = _scoreRight.ToString();
            }
        }

        public int StepLeft
        {
            get { return _stepLeft; }
            set
            {
                _stepLeft = value;
                ButtonTextLeft = _stepLeft.ToString();
            }
        }

        public int StepRight
        {
            get { return _stepRight; }
            set
            {
                _stepRight = value;
                ButtonTextRight = _stepRight.ToString();
            }
        }

        public Visibility IsMenuVisible
        {
            get { return _isMenuVisible; }
            set
            {
                _isMenuVisible = value;
                RaisePropertyChanged(() => IsMenuVisible);
            }
        }

        public string ChangeEnableStateOfGridSplittersButtonText
        {
            get
            {
                if (AreGridSplittersEnabled)
                {
                    return "Disable Gridsplitters";
                }
                else
                {
                    return "Enable Gridsplitters";
                }
            }
        }

        public bool AreGridSplittersEnabled
        {
            get { return _areGridSplittersEnabled; }
            set
            {
                _areGridSplittersEnabled = value;
                RaisePropertyChanged(() => AreGridSplittersEnabled);
            }
        }

        public bool IsUndoEnabled
        {
            get
            {
                if (Steps.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsRedoEnabled
        {
            get
            {
                if (UndoneSteps.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<Step> Steps { get; set; }
        public List<Step> UndoneSteps { get; set; }


        #region Commands

        public ICommand AddPointsLeftCommand
        {
            get { return new RelayCommand(AddPointsLeft); }
        }

        public ICommand AddPointsRightCommand
        {
            get { return new RelayCommand(AddPointsRight); }
        }

        public ICommand ShowMenuCommand
        {
            get { return new RelayCommand(ShowMenu);}
        }

        public ICommand ChangeEnableStateOfGridSplittersCommand
        {
            get { return new RelayCommand(ChangeEnableStateOfGridSplitters); }
        }

        public ICommand UndoCommand
        {
            get { return new RelayCommand(UndoPreviousStep); }
        }

        public ICommand RedoCommand
        {
            get { return new RelayCommand(RedoStep);}
        }

        public ICommand ClearAllCommand
        {
            get {return new RelayCommand(ClearAll); }
        }

        #endregion Commands


        #region Methods

        private void AddPointsLeft()
        {
            ScoreLeft += StepLeft;
            AddStep(Side.left, StepLeft);
        }

        private void AddPointsRight()
        {
            ScoreRight += StepRight;
            AddStep(Side.right, StepRight);
        }

        private void AddStep(Side side, int stepSize, int stepSizeOtherSide = 0)
        {
            Steps.Add(new Step(side, stepSize, stepSizeOtherSide));
        }

        private void ShowMenu()
        {
            if (IsMenuVisible == Visibility.Visible)
            {
                IsMenuVisible = Visibility.Hidden;
            }
            else
            {
                IsMenuVisible = Visibility.Visible;
            }
        }

        private void UndoPreviousStep()
        {
            Step previousStep = Steps.LastOrDefault();

            if (previousStep != null)
            {
                switch (previousStep.Side)
                {
                    case Side.left:
                    {
                        ScoreLeft = ScoreLeft - previousStep.StepSize;
                        break;
                    }
                    case Side.right:
                    {
                        ScoreRight = ScoreRight - previousStep.StepSize;
                        break;
                    }
                    case Side.both:
                    {
                        ScoreLeft = ScoreLeft - previousStep.StepSize;
                        ScoreRight = ScoreRight - previousStep.StepSizeOtherSide;
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Wrong Side: " + previousStep.Side);
                        break;
                    }
                }

                UndoneSteps.Add(previousStep);
                Steps.Remove(previousStep);
            }
        }

        private void RedoStep()
        {
            Step undoneStep = UndoneSteps.LastOrDefault();

            if (undoneStep != null)
            {
                switch (undoneStep.Side)
                {
                    case Side.left:
                    {
                        ScoreLeft = ScoreLeft + undoneStep.StepSize;
                        break;
                    }
                    case Side.right:
                    {
                        ScoreRight = ScoreRight + undoneStep.StepSize;
                        break;
                    }
                    case Side.both:
                    {
                        ScoreLeft = ScoreLeft + undoneStep.StepSize;
                        ScoreRight = ScoreRight + undoneStep.StepSizeOtherSide;
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Wrong Side: " + undoneStep.Side);
                        break;
                    }
                }

                Steps.Add(undoneStep);
                UndoneSteps.Remove(undoneStep);
            }
        }

        private void ChangeEnableStateOfGridSplitters()
        {
            if (AreGridSplittersEnabled)
            {
                AreGridSplittersEnabled = false;
            }
            else
            {
                AreGridSplittersEnabled = true;
            }

            RaisePropertyChanged(() => ChangeEnableStateOfGridSplittersButtonText);
        }

        private void ClearAll()
        {
            AddStep(Side.both, -ScoreLeft, -ScoreRight);
            ScoreLeft = 0;
            ScoreRight = 0;
        }


        #endregion Methods
    }
}
