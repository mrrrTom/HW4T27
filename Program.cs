// Задача 27: Напишите программу, которая принимает на вход число и выдаёт сумму цифр в числе.
// 82 -> 10
// 9012 -> 12

namespace GBHW27
{
    public class ConsoleApp
    {
        public static void Main()
        {
            Console.WriteLine("Welcome to the digits summ finder! \nInsert your integer number:");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Sorry, program could not handle inserted value... Bye!");
            }

            //To find digits summ even for not numeric inputs change forceEvaluate to true and comment check
            var summFinder = new DigitsSummFinder(input, false);
            if (!summFinder.IsCSharpNumericInput())
            {
                Console.WriteLine("Sorry, program works only with numbers... Bye!");
                return;
            }
            
            Console.WriteLine(summFinder.GetSumm());
        }
    }

    public class DigitsSummFinder
    {
        private enum InputType
        {
            Undefinde,
            Numeric,
            NonNumeric
        }

        private const Int32 _asciiZero = (Int32)'0';
        private const int _separatorsSumm = ',' + '.';
        private const int _separatorMult = ',' * '.';
        private Dictionary<int, Func<int, int, bool>> _numericSpecificChars = new Dictionary<int, Func<int, int, bool>> {{'-', (int i, int l) => i == 0}, {_separatorMult, (int i, int l) => (i > 0 && i < l)}};
        private readonly string _input;
        private double _summ;
        private bool _forceEvaluate;
        private InputType _inputType;
        
        public DigitsSummFinder(string input, bool forceEvaluate)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
            
            _summ = 0;
            _input = input;
            _forceEvaluate = forceEvaluate;
            ProcessChars();
        }

        public bool IsCSharpNumericInput()
        {
            return _inputType == InputType.Numeric;
        }

        public double GetSumm()
        {
            return _summ;
        }

        private void ProcessChars()
        {
            _inputType = InputType.Numeric;
            for (var i = 0; i< _input.Length; i++)
            {
                var @char = _input[i];
                if (TryParseChar(@char, out var integer))
                {
                    checked
                    {
                        _summ += integer;
                    }
                }
                else
                {
                    if (_numericSpecificChars.TryGetValue(@char, out var check))
                    {
                        if (check(i, _input.Length))
                        {
                            _numericSpecificChars.Remove(@char);
                            continue;
                        }
                    }
                    else
                    {
                        @char = (char)((_separatorsSumm - @char) * @char);
                        if (_numericSpecificChars.TryGetValue(@char, out var check2))
                        {
                            if (check2(i, _input.Length))
                            {
                                _numericSpecificChars.Remove(@char);
                                continue;
                            }
                        }
                    }

                    _inputType = InputType.NonNumeric;
                    if (!_forceEvaluate) return;
                }
            }
        }

        private bool TryParseChar(char input, out Int32 result)
        {
            result = default(Int32);
            if (input >= '0' && input <= '9')
            {
                result = ToInt(input);
                return true;
            }

            return false;
        }

        private Int32 ToInt(char input)
        {
            return (Int32)input - _asciiZero;
        }
    }
}
