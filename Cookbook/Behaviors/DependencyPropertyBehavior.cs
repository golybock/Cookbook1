using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using Expression = System.Linq.Expressions.Expression;

namespace Cookbook.Behaviors;

public class DependencyPropertyBehavior : Behavior<DependencyObject>
{
    public string Property { get; set; } = null!;
    public string UpdateEvent { get; set; } = null!;

    public static readonly DependencyProperty BindingProperty = DependencyProperty.RegisterAttached(
        "Binding",
        typeof(object),
        typeof(DependencyPropertyBehavior),
        new FrameworkPropertyMetadata { BindsTwoWayByDefault = true }
    );
        
    public object? Binding
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }
    
    private Delegate _handler;
    private EventInfo _eventInfo;
    private PropertyInfo _propertyInfo;

    protected override void OnAttached()
    {
        Type elementType = AssociatedObject.GetType();

        // Получаем свойство визуального элемента
        _propertyInfo = elementType.GetProperty(Property, BindingFlags.Instance | BindingFlags.Public);

        // Получаем событие, уведомляющее об изменении визуального элемента
        _eventInfo = elementType.GetEvent(UpdateEvent);

        // Создаем делегат для подписывания на событие
        _handler = CreateDelegateForEvent(_eventInfo, EventFired);

        // Подписываемся
        _eventInfo.AddEventHandler(AssociatedObject, _handler);
    }

    protected override void OnDetaching()
    {
        // Отписываемся
        _eventInfo.RemoveEventHandler(AssociatedObject, _handler);
    }
    
    private void EventFired()
    {
        Binding = _propertyInfo.GetValue(AssociatedObject, null);
    }
    
    private static Delegate CreateDelegateForEvent(EventInfo eventInfo, Action action)
    {
        ParameterExpression[] parameters = 
            eventInfo
                .EventHandlerType
                .GetMethod("Invoke")
                .GetParameters()
                .Select(parameter => Expression.Parameter(parameter.ParameterType))
                .ToArray();

        return Expression.Lambda(
                eventInfo.EventHandlerType,
                Expression.Call(Expression.Constant(action), "Invoke", Type.EmptyTypes),
                parameters
            )
            .Compile();
    }
    
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property.Name != "Binding") return;

        object oldValue = _propertyInfo.GetValue(AssociatedObject, null);
        if (oldValue.Equals(e.NewValue)) return;

        if (_propertyInfo.CanWrite)
            _propertyInfo.SetValue(AssociatedObject, e.NewValue, null);

        base.OnPropertyChanged(e);
    }
}