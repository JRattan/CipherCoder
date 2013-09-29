using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace CipherCoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Cipher cipher = new Cipher();
     
        public MainWindow()
        {
            InitializeComponent();
            Reset();
        }

        public void Reset()
        {
            inputText.Text = "";
            outputText.Text = "";
            keyText.Text = "CIPHER";
            //CipherAssign();
        }

        /*
        public void CipherAssign()
        {
            cipher.Input = inputText.Text.ToString();
            cipher.Key = keyText.Text.ToString().Replace(" ", String.Empty);
        }
        */

        /*
        public void CipherUpdate()
        {
            if (String.IsNullOrWhiteSpace(outputText.Text))
                MessageBox.Show("There was nothing to encrypt/decrypt.", "Empty String");
        }
        */

        public void UseOutput()
        {
            inputText.Text = outputText.Text;
        }

        private void encryptBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (keyText.Text.ToString().Length == 0)
                    throw new Exception("Cipher key is empty!");
                keyText.Text = keyText.Text.ToString().ToUpper().Replace(" ", String.Empty);
                outputText.Text = Cipher.Encrypt(inputText.Text.ToString(), keyText.Text.ToString().Replace(" ", String.Empty));

                if (String.IsNullOrWhiteSpace(outputText.Text.ToString()))
                    MessageBox.Show("There was nothing to encrypt.", "Empty String");
            }
            catch (ArgumentException ex)
            {
                outputText.Text = ex.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void switchBtn_Click(object sender, RoutedEventArgs e)
        {
            UseOutput();
        }

        private void decryptBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (keyText.Text.ToString().Length == 0)
                    throw new Exception("Cipher key is empty!");
                keyText.Text = keyText.Text.ToString().ToUpper().Replace(" ", String.Empty);
                outputText.Text = Cipher.Decrypt(inputText.Text.ToString(), keyText.Text.ToString().Replace(" ", String.Empty));

                if (String.IsNullOrWhiteSpace(outputText.Text.ToString()))
                    MessageBox.Show("There was nothing to decrypt.", "Empty String");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.ShowDialog();
        }

        private void exportFile_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(outputText.Text.ToString()))
                MessageBox.Show("Cannot save. Ciphered output text is empty.", "No Text to Save");
            else
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "txt";
                saveDialog.AddExtension = true;
                saveDialog.Filter = "Text Document (*.txt)|*.txt|All files(*.*)|*.*";
                saveDialog.FileName = "CipherCode";
                saveDialog.OverwritePrompt = true;
                saveDialog.Title = "Cipher Coder";
                saveDialog.ValidateNames = true;

                if (saveDialog.ShowDialog().Value)
                {
                    using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                        writer.WriteLine(outputText.Text.ToString());

                    MessageBox.Show("Ciphered code saved!", "Success");
                }
            }
        }

        private void importFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Text Document (*.txt)|*.txt|All files(*.*)|*.*";
            openDialog.Title = "Cipher Coder";

            if (openDialog.ShowDialog().Value)
            {
                using (StreamReader reader = new StreamReader(openDialog.FileName))
                {
                    inputText.Text = "";
                    while (reader.Peek() >= 0)
                    {
                        inputText.Text += reader.ReadLine();
                        inputText.Text += "\n";
                    }
                }
            }
        }

        private void clearText_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void copyAll_Click(object sender, RoutedEventArgs e)
        {
            outputText.SelectAll();
            outputText.Copy();
        }

        //Temporary method since XAML one isn't working....
        private void textWrap_Click(object sender, RoutedEventArgs e)
        {
            if (textWrap.IsChecked == true)
            {
                inputText.TextWrapping = TextWrapping.Wrap;
                outputText.TextWrapping = TextWrapping.Wrap;
            }
            else
            {
                inputText.TextWrapping = TextWrapping.NoWrap;
                outputText.TextWrapping = TextWrapping.NoWrap;
            }
        }
    }
}