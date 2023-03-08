using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using NHotkey;

namespace KeySnail.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private readonly InputSimulator _sim = new InputSimulator();

        public MainWindow()
        {
            InitializeComponent();

            // NHotkey.Wpf.HotkeyManager.Current.AddOrReplace("PushToTalk", Key.S, ModifierKeys.None, PushToTalk);
            
        }
        
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void PushToTalk(object? sender, HotkeyEventArgs e)
        {
            // Debug.WriteLine(e.Name);

            // _sim.Keyboard.KeyPress(VirtualKeyCode.VK_F);

            // e.Handled = true;
        }

        // private void NewHotkey_OnKeyUp(object sender, KeyEventArgs e)
        // {
        //     Debug.WriteLine(e.Key);
        //     var block = (TextBlock) sender;
        //     block.Text = e.Key.ToString();
        // }
        //
        // private void NewHotkey2_OnKeyUp(object sender, KeyEventArgs e)
        // {
        //     Debug.WriteLine(e.Key);
        //     var block = (TextBlock) sender;
        //     block.Text = e.Key.ToString();
        // }
        //
        // private void FocusMouseDown(object sender, MouseButtonEventArgs e)
        // {
        //     ((TextBlock) sender).Focus();
        // }
        //
        // private void HotkeyEdit_Click(object sender, RoutedEventArgs e)
        // {
        //     var button = (Button) sender;
        //     // HotkeyThing.Focus();
        // }
        private void Delay_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}