using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Xaml.Interactivity;

namespace YoutubeDownloader.Behaviors;

public class MultiSelectionListBoxBehavior<T> : BehaviorBase<ListBox>
{
    public static readonly StyledProperty<IList?> SelectedItemsProperty =
        AvaloniaProperty.Register<MultiSelectionListBoxBehavior<T>, IList?>(nameof(SelectedItems), defaultBindingMode: BindingMode.TwoWay);

    static MultiSelectionListBoxBehavior()
    {
        SelectedItemsProperty.Changed.AddClassHandler<MultiSelectionListBoxBehavior<T>>(OnSelectedItemsChanged2);
    }

    private static void OnSelectedItemsChanged2(MultiSelectionListBoxBehavior<T> sender, AvaloniaPropertyChangedEventArgs args)
    {
        var behavior = sender;
        if (behavior._modelHandled)
            return;

        if (behavior.AssociatedObject is null)
            return;

        behavior._modelHandled = true;
        behavior.SelectItems();
        behavior._modelHandled = false;
    }

    //public static readonly DependencyProperty SelectedItemsProperty =
    //    DependencyProperty.Register(
    //        nameof(SelectedItems),
    //        typeof(IList),
    //        typeof(MultiSelectionListBoxBehavior<T>),
    //        new FrameworkPropertyMetadata(
    //            null,
    //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
    //            OnSelectedItemsChanged
    //        )
    //    );

    //private static void OnSelectedItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    //{
    //    var behavior = (MultiSelectionListBoxBehavior<T>) sender;
    //    if (behavior._modelHandled)
    //        return;

    //    if (behavior.AssociatedObject is null)
    //        return;

    //    behavior._modelHandled = true;
    //    behavior.SelectItems();
    //    behavior._modelHandled = false;
    //}

    private bool _viewHandled;
    private bool _modelHandled;

    public IList? SelectedItems
    {
        get => GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    // Propagate selected items from model to view
    private void SelectItems()
    {
        _viewHandled = true;

        AssociatedObject!.SelectedItems = new AvaloniaList<T>();
        if (SelectedItems is not null)
        {
            foreach (var item in SelectedItems)
                AssociatedObject.SelectedItems.Add(item);
        }

        _viewHandled = false;
    }

    // Propagate selected items from view to model
    private void OnListBoxSelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        if (_viewHandled) return;
        if (AssociatedObject!.Items is null) return;

        SelectedItems = AssociatedObject!.SelectedItems!.Cast<T>().ToArray();
    }

    // Re-select items when the set of items changes
    private void OnListBoxItemsChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        if (_viewHandled) return;
        if (AssociatedObject!.Items is null) return;
        SelectItems();
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject!.SelectionChanged += OnListBoxSelectionChanged;
        ((INotifyCollectionChanged)AssociatedObject!.Items!).CollectionChanged += OnListBoxItemsChanged;
    }

    /// <inheritdoc />
    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is not null)
        {
            AssociatedObject.SelectionChanged -= OnListBoxSelectionChanged;
            ((INotifyCollectionChanged)AssociatedObject!.Items!).CollectionChanged -= OnListBoxItemsChanged;
        }
    }
}