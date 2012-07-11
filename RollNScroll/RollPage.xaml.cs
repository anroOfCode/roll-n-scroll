using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Fusao.RollNScroll
{
    public partial class RollPage : PhoneApplicationPage
    {
        // the roll page is so simple it makes sense to just store these as private
        // member variables instead of exposing them using a view model. 
        private string _rollText = "THIS IS SCROLL N ROLL";
        private string _foregroundColor = "white";
        private string _backgroundColor = "pink";
        private string _strobe = "false";

        // Used as a frame counter for strobing. 
        private int counter = 0;

        public RollPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // disable the screen timeout- wouldn't want the screen to dim while partying.
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!NavigationContext.QueryString.TryGetValue("scrollText", out _rollText))
            {
                _rollText = "THIS IS ROLL N' SCROLL";
            }

            if (!NavigationContext.QueryString.TryGetValue("forecolor", out _foregroundColor))
            {
                _foregroundColor = "magenta";
            }

            if (!NavigationContext.QueryString.TryGetValue("backcolor", out _backgroundColor))
            {
                _backgroundColor = "teal";
            }

            // It's simplier for this scenario to just keep the boolean as
            // a string. 
            if (!NavigationContext.QueryString.TryGetValue("strobe", out _strobe))
            {
                _strobe = "False";
            }

            AccentColorNameToBrush accentColorNameConverter = new AccentColorNameToBrush();

            // A single UI element is limited to 2048 pixels, many strings of text
            // exceed this. We break each text string down and add them to a StackPanel
            // to overcome this limit. The letters off screen will not be drawn, the StackPanel
            // lays things out nicely, and we overcome that annoying 2048px limit. Everyone wins.
            for (int i = 0; i < _rollText.Length; i++)
            {
                TextBlock letterTextBlock = new TextBlock();
                letterTextBlock.Text = _rollText[i].ToString();
                letterTextBlock.FontFamily = new FontFamily("Arial");
                letterTextBlock.FontSize = 600;
                letterTextBlock.Foreground = (SolidColorBrush)accentColorNameConverter.Convert(_foregroundColor, typeof(SolidColorBrush), null, null);
                txtScrollView.Children.Add(letterTextBlock);
            }

            LayoutRoot.Background = (SolidColorBrush)accentColorNameConverter.Convert(_backgroundColor, typeof(SolidColorBrush), null, null);

            // Since we call this action recursively we have to define it and 
            // set it to null before attaching a lambda function to it.
            Action a = null;

            a = new Action(() =>
                 {
                     // gets the size of the ScrollView, if it was bounded by no window, i.e. the size
                     // of the text we stuck in it. 
                     txtScrollView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                     // The 20px value below is hardcoded to give a little bit of a delay between
                     // when the text scrolls off the screen and appears again, it's totally arbitrary.

                     // If the text has scrolled completely off the screen, we
                     // move it back where it belongs. 
                     if (scrollViewer.Margin.Left <= -txtScrollView.DesiredSize.Width - LayoutRoot.ActualWidth)
                     {
                         scrollViewer.Margin = new Thickness(LayoutRoot.ActualWidth + 20, 0, 0, 0);
                     }

                     // We move 20px at a time. 
                     scrollViewer.Margin = new Thickness(scrollViewer.Margin.Left - 20, 0, 0, 0);

                     // Every third movement we swap the colors of the foreground
                     // and background if in strobe mode. 
                     if (_strobe == "True" && counter % 3 == 0)
                     {
                         // Grab a copy of the Background brush
                         Brush b = LayoutRoot.Background;

                         // Set the background to the color of the foreground of one of the ScrollView's
                         // children (which is a letter)
                         LayoutRoot.Background = ((TextBlock)txtScrollView.Children[0]).Foreground;

                         // Update all the ScrollView's children to be the color that was the ScrollView. 
                         for (int i = 0; i < txtScrollView.Children.Count; i++)
                         {
                             ((TextBlock)txtScrollView.Children[i]).Foreground = b;
                         }
                     }

                     counter++;

                     // Schedule the invokation of this Action as soon as
                     // possible. 
                     Dispatcher.BeginInvoke(a);
                 });

            // Start the recursive call of our main movement function.
            Dispatcher.BeginInvoke(a);
        }
    }
}