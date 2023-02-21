﻿#pragma checksum "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "793FB7E1C384272B2F501178C428697D4085C4DF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Cookbook.Pages.LoginRegister;
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


namespace Cookbook.Pages.LoginRegister {
    
    
    /// <summary>
    /// LoginPage
    /// </summary>
    public partial class LoginPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border LoginBorder;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LoginTextBox;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border PasswordBorder;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordBox;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ErrorTextBlock;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LoginButton;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button GuestButton;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock RegistrationTextBlock;
        
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
            System.Uri resourceLocater = new System.Uri("/Cookbook;component/pages/loginregister/loginpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
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
            this.LoginBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.LoginTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 36 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
            this.LoginTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Input);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PasswordBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.PasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 50 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
            this.PasswordBox.PasswordChanged += new System.Windows.RoutedEventHandler(this.Input);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ErrorTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.LoginButton = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
            this.LoginButton.Click += new System.Windows.RoutedEventHandler(this.LoginButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.GuestButton = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
            this.GuestButton.Click += new System.Windows.RoutedEventHandler(this.GuestButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.RegistrationTextBlock = ((System.Windows.Controls.TextBlock)(target));
            
            #line 76 "..\..\..\..\..\Pages\LoginRegister\LoginPage.xaml"
            this.RegistrationTextBlock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.RegistrationTextBlock_OnMouseDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

