using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace TypingSpeedTest
{
    public partial class Form1 : Form
    {
        private List<string> testTexts;
        private string currentTestText;
        private Stopwatch stopwatch;
        private Timer timer;
        private int timeLimit = 60; // seconds
        private Random random;

        public Form1()
        {
            InitializeComponent();
            InitializeTest();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // This method is intentionally left empty
            // It gets called when the form is loaded
        }

        private void InitializeTest()
        {
            testTexts = new List<string>
            {
                "A Fox one day spied a beautiful bunch of ripe grapes hanging from a vine trained along the branches of a tree. The grapes seemed ready to burst with juice, and the Fox's mouth watered as he gazed longingly at them.The bunch hung from a high branch, and the Fox had to jump for it. The first time he jumped he missed it by a long way. So he walked off a short distance and took a running leap at it, only to fall short once more. Again and again he tried, but in vain.Now he sat down and looked at the grapes in disgust.What a fool I am, he said. Here I am wearing myself out to get a bunch of sour grapes that are not worth gaping for.And off he walked very, very scornfully.There are many who pretend to despise and belittle that which is beyond their reach.",
                "A journey of a thousand miles begins with a single step.",
                "To be or not to be, that is the question.",
                "All that glitters is not gold.",
                "The only thing we have to fear is fear itself."
            };

            random = new Random();
            labelTestText.Text = "";
            textBoxInput.Enabled = false;
            labelResults.Text = "";
            stopwatch = new Stopwatch();

            timer = new Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += Timer_Tick;1

            buttonStart.Click += ButtonStart_Click;
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            currentTestText = testTexts[random.Next(testTexts.Count)];
            labelTestText.Text = currentTestText;

            textBoxInput.Clear();
            textBoxInput.Enabled = true;
            textBoxInput.Focus();

            stopwatch.Reset();
            stopwatch.Start();

            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int elapsedSeconds = (int)stopwatch.Elapsed.TotalSeconds;
            labelTimer.Text = (timeLimit - elapsedSeconds).ToString() + "s";

            if (elapsedSeconds >= timeLimit)
            {
                timer.Stop();
                stopwatch.Stop();
                textBoxInput.Enabled = false;
                CalculateResults();
            }
        }

        private void CalculateResults()
        {
            string userInput = textBoxInput.Text;
            int wordsTyped = userInput.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            double wpm = (wordsTyped / (double)timeLimit) * 60;

            int correctChars = 0;
            for (int i = 0; i < Math.Min(userInput.Length, currentTestText.Length); i++)
            {
                if (userInput[i] == currentTestText[i])
                {
                    correctChars++;
                }
            }
            double accuracy = (double)correctChars / currentTestText.Length * 100;

            labelResults.Text = String.Format("WPM: {0:F2}\nAccuracy: {1:F2}%", wpm, accuracy);
        }
    }
}
