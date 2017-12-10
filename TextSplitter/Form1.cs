using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TextSplitter
{
    public partial class Form1 : Form
    {
        private int _numberSigns;       //количество знаков для деления
        private int _textLength;        //длинна исходного текста
        private string _textToSplit;    //исходный текст
        private int _numberOfStrings;   //количество возвращаемых строк

        private byte _testCase;         //номер тестового случая

        private string _expectedText;   //эталонный текст

        private string _splittedText;   //разрезанный текст

        public int NumberSigns
        {
            get { return _numberSigns; }
            set { _numberSigns = value; }
        }

        public string TextToSplit
        {
            get { return _textToSplit; }
            set { _textToSplit = value; }
        }

        public string ExpectedText
        {
            get { return _expectedText; }
            set { _expectedText = value; }
        }

        public string SplittedText
        {
            get { return _splittedText; }
            set { _splittedText = value; }
        }

        public Form1()
        {
            InitializeComponent();

            _testCase = 1;
            CreateTestCase1();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();

            _splittedText = string.Empty;

            _textToSplit = richTextBox1.Text;
            _textLength = _textToSplit.Length;

            //если длинна текста меньше или равна количеству знаков для деления
            //возвращаем исходный текст
            //иначе
            //производим деление текста
            if (_textLength <= _numberSigns)
            {
                richTextBox2.Text = _textToSplit;
            }
            else
            {
                //List<string> splittedText = Split();

                //foreach (string s in splittedText)
                //{
                //    richTextBox2.AppendText(string.Format("{0}\n", s));
                //}

                Split();
                richTextBox2.Text = _splittedText;
            }

            label1.Text = string.Format("Количество символов: {0}, строк {1}", _textLength, _numberOfStrings);
        }

        public string Split()
        {
            _textLength = _textToSplit.Length;
            //List<string> retText = new List<string>();      //список возвращаемых строк


            //если остаток от деления длины текста на кол-во знаков деления
            //не равен 0 - добавить 1 строку к кол-ву возвращаемых строк
            if (_textLength % _numberSigns != 0)
            {
                _numberOfStrings = _textLength / _numberSigns + 1;
            }
            else
            {
                _numberOfStrings = _textLength / _numberSigns;
            }

            int index = 0;                              //индекс текущего знака в исходной строке
            int border = index + _numberSigns;          //граница раздела строки

            //проход по каждой новой строке
            for (int i = 0; i < _numberOfStrings; i++)
            {
                StringBuilder sb = new StringBuilder();

                char nextLetter = '\0';
                char prevLetter = '\0';

                bool newLine = true;

                //посимвольный проход по исходному тексту
                for (int j = index; j < border; j++)
                {
                    if (j >= _textLength)
                    {
                        //retText.Add(sb.ToString());
                        _splittedText += sb + "\n";
                        break;
                    }

                    char letter = _textToSplit[j];      //текущий символ

                    //удаляем лишний пробел в начале строки
                    if (newLine && letter == ' ')
                    {
                        //увеличиваем значение границы строки
                        ++border;
                        continue;
                    }

                    index = j;
                    newLine = false;
                    sb.Append(letter);

                    //если разбиение посимвольное
                    //просто выводим строки по одному символу
                    if (_numberSigns == 1)
                    {
                        //retText.Add(sb.ToString());
                        _splittedText += sb + "\n";
                        index++;                                //переход индекса на следующий символ
                        border = index + _numberSigns;         //установка новой границы

                        break;
                    }

                    //проверяем, достиг ли курсор конца строки
                    if (index == border - 1)
                    {
                        //Если конец строки выпадает на точку или запятую
                        //проверяем не дата ли это
                        if (letter == '.' || letter == ',')
                        {
                            prevLetter = _textToSplit[index - 1];

                            if (index + 1 < _textLength)
                            {
                                nextLetter = _textToSplit[index + 1];
                            }

                            if (char.IsDigit(prevLetter) && char.IsDigit(nextLetter))
                            {
                                while (char.IsDigit(letter) || letter == '.' || letter == ',')
                                {
                                    letter = prevLetter;
                                    sb.Remove(sb.Length - 1, 1);
                                    --index;
                                    prevLetter = _textToSplit[index - 1];
                                }
                            }
                        }

                        //если строка заканчивается на цифру, открывающиеся кавычки,
                        //то знак переносится на следующую строку
                        while (char.IsDigit(letter) || letter == '«' || letter == '\"' || letter == '\'' || letter == '(')
                        {
                            prevLetter = _textToSplit[index - 1];

                            if (index + 1 < _textLength)
                            {
                                nextLetter = _textToSplit[index + 1];
                            }

                            //если рядом две цифры - ничего не удаляем
                            //если следующие знаки тоже цифры (т.е. дата и т.д )- ничего не удаляем
                            if (char.IsDigit(prevLetter))
                            {
                                while (char.IsDigit(nextLetter))
                                {
                                    sb.Append(nextLetter);
                                    ++index;
                                    nextLetter = _textToSplit[index + 1];
                                }
                                break;
                            }

                            //проверяем предыдущий знак
                            //если . или , (указывает на дату или дробное число)
                            //не разрываем их

                            if ((prevLetter == '.' || prevLetter == ',') && char.IsDigit(nextLetter) && char.IsDigit(letter))
                            {
                                //prevLetter = _textToSplit[index - 2];

                                while (char.IsDigit(letter) || letter == '.' || letter == ',')
                                {
                                    //--index;
                                    letter = prevLetter;
                                    sb.Remove(sb.Length - 1, 1);
                                    --index;
                                    prevLetter = _textToSplit[index];
                                }

                                //letter = prevLetter;
                                //sb.Remove(sb.Length - 2, 2);
                                //index -= 2;
                                continue;
                            }

                            //удаляем текущую букву
                            //откатываем индекс на -1
                            letter = prevLetter;
                            sb.Remove(sb.Length - 1, 1);
                            --index;
                        }

                        //перенос символов, если встречаются знаки препинания
                        if (index + 1 < _textLength)
                        {
                            nextLetter = _textToSplit[index + 1];
                            
                            if (char.IsPunctuation(nextLetter) && letter != ' ')
                            {
                                sb.Remove(sb.Length - 2, 2);
                                index -= 2;
                                nextLetter = _textToSplit[index + 1];

                                while (char.IsPunctuation(_textToSplit[index + 2]))
                                {
                                    sb.Remove(sb.Length - 1, 1);
                                    --index;
                                    nextLetter = _textToSplit[index + 1];
                                }
                            }
                        }

                        //если конец строки выпадает на предпоследнюю букву слова
                        //перенести букву на следующую строку
                        if (index + 2 < _textLength)
                        {
                            nextLetter = _textToSplit[index + 1];

                            if (char.IsLower(nextLetter) && _textToSplit[index + 2] == ' ' && letter != ' ' && !char.IsDigit(letter))
                            {
                                sb.Remove(sb.Length - 1, 1);
                                --index;
                            }
                        }

                        //проверка, чтобы одна буква не оставалась на предыдущей строке,
                        //за исключением союзов, предлогов и т.д.
                        if (index + 1 < _textLength)
                        {
                            nextLetter = _textToSplit[index + 1];

                            if (index-1 >= 0)
                            {
                                prevLetter = _textToSplit[index - 1];
                            }

                            if (prevLetter == ' ' && nextLetter != ' ' && _textToSplit[index] != ' ')
                            {
                                sb.Remove(sb.Length - 1, 1);
                                --index;
                            }
                        }

                        //расстановка переносов
                        if (_numberSigns > 4)
                        {
                            if (index + 1 < _textLength)
                            {
                                nextLetter = _textToSplit[index + 1];
                                prevLetter = _textToSplit[index - 1];

                                if (_textToSplit[index - 2] == ' ' && _textToSplit[index] != ' ' && nextLetter != ' ' && index+1 == border)
                                {
                                    sb.Remove(sb.Length - 2, 2);
                                    index -= 2;

                                }
                                else if (char.IsLower(nextLetter) && _textToSplit[index] != ' ')
                                {
                                    if (index + 1 == border)
                                    {
                                        sb.Remove(sb.Length - 1, 1);
                                        sb.Append('-');
                                        --index;
                                    }
                                    else
                                    {
                                        sb.Append('-');
                                    }
                                }
                            }
                        }
                        
                        //retText.Add(sb.ToString());
                        _splittedText += sb + "\n";
                        index++;                                //переход индекса на следующий символ
                        border = index + _numberSigns;         //установка новой границы

                        if (border > _textLength)
                        {
                            border = _textLength;               //установка максимальной границы
                        }

                        //если изначальное количество отведенных строк закончилось,
                        //а символы в тексте еще остались после всех манипуляций -
                        //добавить строку
                        if (i == _numberOfStrings - 1 && index < border)
                        {
                            _numberOfStrings++;
                        }

                        break;                                //переход на новую строку
                    }
                }
            }

            return _splittedText;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _numberSigns = (int)numericUpDown1.Value;
        }

        /// <summary>
        /// Генерация тестового случая
        /// </summary>
        private void CreateTestCase1()
        {
            _textToSplit = 
                "Александр Сергеевич Пушкин родился 26.05.1799 года (по старому стилю) в Москве в дворянской помещичьей семье.";
            _numberSigns = 10;

            _expectedText = "Александр \nСергеевич \nПушкин ро-\nдился\n26.05.1799\nгода (по \nстарому \nстилю) в \nМоскве в \nдворянской\nпомещичьей\nсемье.\n";

            richTextBox1.Text = _textToSplit;
            numericUpDown1.Value = _numberSigns;
            richTextBox3.Text = _expectedText;
        }

        private void CreateTestCase2()
        {
            _textToSplit =
                "В школьной программе число Пи округляют приблизительно до 3,14.";
            _numberSigns = 16;

            _expectedText = "В школьной прог-\nрамме число Пи \nокругляют прибл-\nизительно до\n3,14.\n"; ;
        }

        private void CreateTestCase3()
        {
            _textToSplit =
                "Мама мыла рамозаготовительную машину";
            _numberSigns = 5;

            _expectedText = "Мама \nмыла \nрамо-\nзаго-\nтови-\nтель-\nную \nмаши-\nну\n"; ;
        }

        private void CreateTestCase4()
        {
            _textToSplit =
                "Старый священник подошел ко мне с вопросом тяжелым: «Прикажете  начинать?» \"Слава богу, – сказала девушка, – насилу вы приехали сюда. Чуть было вы барышню не уморили ждать\" (По Пушкину).";
            _numberSigns = 13;

            _expectedText = "Старый свяще-\nнник подошел \nко мне с воп-\nросом тяжел-\nым: «Прикаже-\nте  начина-\nть?» \"Слава \nбогу, – сказ-\nала девушка, \n– насилу вы \nприехали сю-\nда. Чуть было\nвы барышню не\nуморили жда-\nть\" (По Пушк-\nину).\n";
        }

        private void btnNextTest_Click(object sender, EventArgs e)
        {
            ++_testCase;
            switch (_testCase)
            {
                case 2:
                {
                    CreateTestCase2();
                        break;
                }
                case 3:
                {
                    CreateTestCase3();
                    break;
                }
                case 4:
                {
                    CreateTestCase4();
                    break;
                }
                default:

                {
                    _testCase = 1;
                        CreateTestCase1();
                        break;
                }
            }

            richTextBox2.Clear();
            richTextBox1.Text = _textToSplit;
            numericUpDown1.Value = _numberSigns;
            richTextBox3.Text = _expectedText;
        }
    }
}
