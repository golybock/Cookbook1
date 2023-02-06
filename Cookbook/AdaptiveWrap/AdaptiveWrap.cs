using System;
using System.Windows;
using System.Windows.Controls;

namespace Cookbook.AdaptiveWrap;

public class AdaptiveWrapPanel : WrapPanel
{
    public AdaptiveWrapPanel()
    {
        SizeChanged += OnSizeChanged;
    }
    
    public static DependencyProperty MinItemWidthProperty =
        DependencyProperty.Register(nameof(MinItemWidthProperty), typeof(double), typeof(AdaptiveWrapPanel),
            new FrameworkPropertyMetadata(new double()));
    
    public static DependencyProperty MaxItemWidthProperty =
        DependencyProperty.Register(nameof(MaxItemWidthProperty), typeof(double), typeof(AdaptiveWrapPanel),
            new FrameworkPropertyMetadata(new double()));
    
    public static DependencyProperty LinesProperty =
        DependencyProperty.Register(nameof(LinesProperty), typeof(int), typeof(AdaptiveWrapPanel),
            new FrameworkPropertyMetadata(new int()));
    
    public double MinItemWidth
    {
        get => (double)GetValue(MinItemWidthProperty);
        set => SetValue(MinItemWidthProperty, value);
    }

    public int Lines
    {
        get => (int) GetValue(LinesProperty);
        set => SetValue(LinesProperty, value);
    }
    
    public double MaxItemWidth
    {
        get => (double) GetValue(MaxItemWidthProperty);
        set => SetValue(MaxItemWidthProperty, value);
    }
    
    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        double maxItemsInLine = Math.Floor(ActualWidth / MinItemWidth);

        if (maxItemsInLine > 0)
        {
            foreach (FrameworkElement child in Children)
            {
                child.Width = ActualWidth / maxItemsInLine;
            }
        }
        else
        {
            foreach (FrameworkElement child in Children)
            {
                child.Width = MinItemWidth + 25;
            } 
        }

        
    }
}