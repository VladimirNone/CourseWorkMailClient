﻿#pragma checksum "..\..\..\..\FolderItems\NavigateMenu.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C86867D4DC63176AA6C7E2929F7C497C7938B2B6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CourseWorkMailClient.FolderItems;
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


namespace CourseWorkMailClient.FolderItems {
    
    
    /// <summary>
    /// NavigateMenu
    /// </summary>
    public partial class NavigateMenu : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 12 "..\..\..\..\FolderItems\NavigateMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbNavMenu;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\FolderItems\NavigateMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miCreate;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\FolderItems\NavigateMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miRename;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\FolderItems\NavigateMenu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem miDelete;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.11.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CourseWorkMailClient;component/folderitems/navigatemenu.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\FolderItems\NavigateMenu.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lbNavMenu = ((System.Windows.Controls.ListBox)(target));
            
            #line 12 "..\..\..\..\FolderItems\NavigateMenu.xaml"
            this.lbNavMenu.ContextMenuOpening += new System.Windows.Controls.ContextMenuEventHandler(this.ContextMenu_ContextMenuOpening);
            
            #line default
            #line hidden
            return;
            case 2:
            this.miCreate = ((System.Windows.Controls.MenuItem)(target));
            
            #line 15 "..\..\..\..\FolderItems\NavigateMenu.xaml"
            this.miCreate.Click += new System.Windows.RoutedEventHandler(this.miCreate_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.miRename = ((System.Windows.Controls.MenuItem)(target));
            
            #line 16 "..\..\..\..\FolderItems\NavigateMenu.xaml"
            this.miRename.Click += new System.Windows.RoutedEventHandler(this.miRename_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.miDelete = ((System.Windows.Controls.MenuItem)(target));
            
            #line 17 "..\..\..\..\FolderItems\NavigateMenu.xaml"
            this.miDelete.Click += new System.Windows.RoutedEventHandler(this.miDelete_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 5:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.GotFocusEvent;
            
            #line 24 "..\..\..\..\FolderItems\NavigateMenu.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.OpenFolder);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

