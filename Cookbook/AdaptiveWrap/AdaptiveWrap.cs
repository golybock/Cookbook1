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
        DependencyProperty.Register(nameof(MinItemWidthProperty), typeof(Double), typeof(AdaptiveWrapPanel),
            new FrameworkPropertyMetadata(new double()));
    
    public static DependencyProperty MaxItemWidthProperty =
        DependencyProperty.Register(nameof(MaxItemWidthProperty), typeof(Double), typeof(AdaptiveWrapPanel),
            new FrameworkPropertyMetadata(new double()));
    
    public Double MinItemWidth
    {
        get => (double)GetValue(MinItemWidthProperty);
        set => SetValue(MinItemWidthProperty, value);
    }

    public Double MaxItemWidth
    {
        get => (double) GetValue(MaxItemWidthProperty);
        set => SetValue(MaxItemWidthProperty, value);
    }
    
    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        var width = ActualWidth;
        
        double maxItemsCount = Math.Floor(width / MinItemWidth);
        
        foreach (FrameworkElement child in Children)
        {
            child.Width = ActualWidth / maxItemsCount;
        }
        
    }
}