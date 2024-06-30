using System.Text.RegularExpressions;

namespace _1_word_game
{
    internal class Validator
    {
        private string _input = string.Empty;
        private string _inputError = string.Empty;
        public Validator() { }
        public void UpdateInput(string input)
        {
            _input = input;
        }

        public void ValidationInputOriginalWord()
        {
            if (string.IsNullOrEmpty(_input))
            {
                _inputError = "Input is empty! Please try again";
                return;
            }
            if (_input.Length < 8)
            {
                _inputError = "The word is too short! Please try again.";
                return;
            }
            if (_input.Length > 30)
            {
                _inputError = "The word is too long! Please try again.";
                return;
            }
            if (Regex.IsMatch(_input, @"[^a-zA-Z]"))
            {
                _inputError = "The word should contain only letters! Please try again.";
                return;
            }
            _inputError = string.Empty;
        }

        public void ValidationPlayerWord(string originalWord, string[] allWords)
        {
            if (string.IsNullOrEmpty(_input))
            {
                _inputError = "Empty input! Plaese try again.";
                return;
            }

            string testWord = _input.ToLower();

            foreach (string w in allWords)
            {
                if (testWord == w)
                {
                    _inputError = $"The word '{_input}' is already exist! Please try again.";
                    return;
                }
            }

            foreach (char c in originalWord.ToLower())
            {
                int index = testWord.IndexOf(c);

                if (index >= 0)
                {
                    testWord = testWord.Remove(index, 1);
                }
            }
            if (!string.IsNullOrEmpty(testWord))
            {
                _inputError = $"The word '{_input}' cannot be create from original word! Please try again.";
                return;
            }

            _inputError = string.Empty;
        }

        public string GetInput()
        {
            return _input;
        }

        public bool IsError()
        { 
            return !string.IsNullOrEmpty(_inputError);
        }

        public string InputError()
        {
            return _inputError;
        }

        public bool IsInputCommand()
        {
            return _input.StartsWith("/");
        }
    }
}
