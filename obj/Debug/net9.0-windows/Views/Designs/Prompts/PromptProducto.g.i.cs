﻿#pragma checksum "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CBB09258BE5CB580EF701779CC986D87D87E55EE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ProdLogApp.Views;
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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Converters;
using Xceed.Wpf.Toolkit.Core;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Mag.Converters;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace ProdLogApp.Views {
    
    
    /// <summary>
    /// PromptProducto
    /// </summary>
    public partial class PromptProducto : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ColumnFilter;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.AutoSelectTextBox ProductoAutoComplete;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView Producciones;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
<<<<<<< Updated upstream
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.5.0")]
=======
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
>>>>>>> Stashed changes
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProdLogApp;V1.0.0.0;component/views/designs/prompts/promptproducto.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
<<<<<<< Updated upstream
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.5.0")]
=======
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
>>>>>>> Stashed changes
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ColumnFilter = ((System.Windows.Controls.ListBox)(target));
            
            #line 24 "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml"
            this.ColumnFilter.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ColumnFilter_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ProductoAutoComplete = ((Xceed.Wpf.Toolkit.AutoSelectTextBox)(target));
            
            #line 38 "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml"
            this.ProductoAutoComplete.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ProductoAutoComplete_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Producciones = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            
            #line 58 "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Confirmar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 59 "..\..\..\..\..\..\Views\Designs\Prompts\PromptProducto.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cancelar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

