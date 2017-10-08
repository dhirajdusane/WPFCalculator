using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFCalculator
{
    public class ViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        #region InterfaceImplementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Constants
        private string[] digitArray = new string[] { "7", "4", "1", "8", "5", "2", "0", "9", "6", "3","." };
        private string[] signArray = new string[] { "/", "x", "-", "+","=" };
        private string[][] displayArray = new string[][]
        {
            new string[]{"7","4","1","." },
            new string[]{"8","5","2","0" },
            new string[]{"9","6","3","=" },
            new string[]{"/","x","-","+" },
        } ;
        #endregion

        #region ViewPropertiesMethods
        private List<List<DisplayElement>> displayList;
        public List<List<DisplayElement>> DisplayList
        {
            get { return displayList; }
            set { displayList = value; NotifyPropertyChanged(); }
        }

        private string screenText;
        public string ScreenText
        {
            get { return screenText; }
            set { screenText = value; NotifyPropertyChanged(); }
        }

        public ICommand BtnClick { get { return new DelegateEvent(HandleButtonClick); } }

        public ICommand ClearClick { get { return new DelegateEvent(Reset); } }

        private void Reset()
        {
            ScreenText = "0";
            ResetStringBuffer();
            commandQueue.Clear();
        }

        public ViewModel()
        {
            DisplayList = new List<List<DisplayElement>>();
            FillDisplayList();
            Reset();
        }

        public void FillDisplayList()
        {
            foreach (var item in displayArray)
            {
                var subList = new List<DisplayElement>();
                DisplayList.Add(subList);
                foreach (var childItem in item)
                {
                    subList.Add(new DisplayElement() { DisplayContent = childItem });
                }
            }
        }

        #endregion

        #region Logic
        private string operand = string.Empty;
        private Queue<string> commandQueue = new Queue<string>();
        private string stringBuffer = string.Empty;

        public void HandleButtonClick(object obj)
        {
            string newValue = obj as string;
            newValue = newValue.ToLowerInvariant();

            if (digitArray.Contains(newValue))
            {
                stringBuffer += newValue;
                ScreenText = stringBuffer;
            }
            else if (signArray.Contains(newValue))
            {
                ProcessSign();
                operand = newValue;
            }
        }

        private void ProcessSign()
        {
            if (!string.IsNullOrEmpty(stringBuffer))
                commandQueue.Enqueue(stringBuffer);
            ResetStringBuffer();

            if (commandQueue.Count > 1)
                CalculateQueue(commandQueue);
        }

        private void CalculateQueue(Queue<string> commandQueue)
        {
            double left = 0, right = 0, result = 0;
            string value = commandQueue.Dequeue();
            double.TryParse(value, out left);
            value = commandQueue.Dequeue();
            double.TryParse(value, out right);

            switch (operand.ToLower())
            {
                case "+":
                    result = left + right;
                    break;
                case "-":
                    result = left - right;
                    break;
                case "x":
                    result = left * right;
                    break;
                case "/":
                    result = left / right;
                    break;
                case "=":
                    result = right;
                    break;
                default:
                    throw new NotImplementedException();
            }

            string resultString = result.ToString();
            commandQueue.Enqueue(resultString);
            UpdateScreen(resultString);
            ResetStringBuffer();
            operand = string.Empty;
        }

        private void ResetStringBuffer()
        {
            stringBuffer = string.Empty;
        }

        private void UpdateScreen(string appendText)
        {
            stringBuffer += appendText;
            ScreenText = stringBuffer;
        }
        #endregion
    }
}