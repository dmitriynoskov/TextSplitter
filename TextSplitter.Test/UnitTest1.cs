using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TextSplitter.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Split_Splitted_Expected_Test1()
        {
            //arrange
            string _textToSplit = "Александр Сергеевич Пушкин родился 26.05.1799 года (по старому стилю) в Москве в дворянской помещичьей семье.";

            int _numberSigns = 10;

            string _expectedText = "Александр \nСергеевич \nПушкин ро-\nдился\n26.05.1799\nгода (по \nстарому \nстилю) в \nМоскве в \nдворянской\nпомещичьей\nсемье.\n";

            //act
            Form1 form = new Form1();
            form.TextToSplit = _textToSplit;
            form.NumberSigns = _numberSigns;

            string split = form.Split();

            //assert
            Assert.AreEqual(_expectedText, split);
        }

        [TestMethod]
        public void Split_Splitted_Expected_Test2()
        {
            //arrange
            string _textToSplit = "В школьной программе число Пи округляют приблизительно до 3,14.";

            int _numberSigns = 16;

            string _expectedText = "В школьной прог-\nрамме число Пи \nокругляют прибл-\nизительно до\n3,14.\n";

            //act
            Form1 form = new Form1();
            form.TextToSplit = _textToSplit;
            form.NumberSigns = _numberSigns;
            string split = form.Split();

            //assert
            Assert.AreEqual(_expectedText, split);
        }

        [TestMethod]
        public void Split_Splitted_Expected_Test3()
        {
            //arrange
            string _textToSplit = "Мама мыла рамозаготовительную машину";

            int _numberSigns = 5;

            string _expectedText = "Мама \nмыла \nрамо-\nзаго-\nтови-\nтель-\nную \nмаши-\nну\n";

            //act
            Form1 form = new Form1();
            form.TextToSplit = _textToSplit;
            form.NumberSigns = _numberSigns;
            string split = form.Split();

            //assert
            Assert.AreEqual(_expectedText, split);
        }

        [TestMethod]
        public void Split_Splitted_Expected_Test4()
        {
            //arrange
            string _textToSplit = "Старый священник подошел ко мне с вопросом тяжелым: «Прикажете  начинать?» \"Слава богу, – сказала девушка, – насилу вы приехали сюда. Чуть было вы барышню не уморили ждать\" (По Пушкину).";

            int _numberSigns = 13;

            string _expectedText = "Старый свяще-\nнник подошел \nко мне с воп-\nросом тяжел-\nым: «Прикаже-\nте  начина-\nть?» \"Слава \nбогу, – сказ-\nала девушка, \n– насилу вы \nприехали сю-\nда. Чуть было\nвы барышню не\nуморили жда-\nть\" (По Пушк-\nину).\n";

            //act
            Form1 form = new Form1();
            form.TextToSplit = _textToSplit;
            form.NumberSigns = _numberSigns;
            string split = form.Split();

            //assert
            Assert.AreEqual(_expectedText, split);
        }
    }
}
