﻿#pragma checksum "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6D972F5FDE9003D856334A58C59950FCA2D03CFA"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace ProdLogApp.Views {
    
    
    /// <summary>
    /// ProduccionAgregarGerente
    /// </summary>
    public partial class ProduccionAgregarGerente : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ProductoLabel;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ProductoTextBox;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox HoraInicioTextBox;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox HoraFinTextBox;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CantidadTextBox;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OperarioTextBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.6.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProdLogApp;V1.0.0.0;component/views/designs/menus/produccionagregargerente.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.6.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ProductoLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.ProductoTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            
            #line 54 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SeleccionarProducto_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.HoraInicioTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.HoraFinTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.CantidadTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            
            #line 88 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SeleccionarOperario_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 94 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Confirmar_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 96 "..\..\..\..\..\..\Views\Designs\Menus\ProduccionAgregarGerente.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cancelar_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.OperarioTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

