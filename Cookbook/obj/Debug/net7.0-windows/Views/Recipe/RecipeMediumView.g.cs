﻿#pragma checksum "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "AC82959096C865615BE6854F92B3A271D2488110"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Cookbook.Converters;
using Cookbook.Views.Recipe;
using ModernWpf;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using ModernWpf.DesignTime;
using ModernWpf.MahApps;
using ModernWpf.MahApps.Controls;
using ModernWpf.Markup;
using ModernWpf.Media.Animation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Cookbook.Views.Recipe {
    
    
    /// <summary>
    /// RecipeMediumView
    /// </summary>
    public partial class RecipeMediumView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem DeleteMenuItem;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem EditMenuItem;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Id;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border LikeButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Cookbook;component/views/recipe/recipemediumview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml"
            ((Cookbook.Views.Recipe.RecipeMediumView)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.RecipeMediumView_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DeleteMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 34 "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml"
            this.DeleteMenuItem.Click += new System.Windows.RoutedEventHandler(this.DeleteMenuItem_OnClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.EditMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 41 "..\..\..\..\..\Views\Recipe\RecipeMediumView.xaml"
            this.EditMenuItem.Click += new System.Windows.RoutedEventHandler(this.EditMenuItem_OnClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Id = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.LikeButton = ((System.Windows.Controls.Border)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

