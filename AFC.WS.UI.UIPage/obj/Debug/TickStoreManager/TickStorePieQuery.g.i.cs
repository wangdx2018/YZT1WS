﻿#pragma checksum "..\..\..\TickStoreManager\TickStorePieQuery.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FC30B3D8E48571C15636B26808B906A09285AED6"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using AFC.BOM2.UIController;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using Visifire.Charts;


namespace AFC.WS.UI.UIPage.TickStoreManager {
    
    
    /// <summary>
    /// TickStorePieQuery
    /// </summary>
    public partial class TickStorePieQuery : AFC.BOM2.UIController.UserControlBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\TickStoreManager\TickStorePieQuery.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border DiagramBorder;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\TickStoreManager\TickStorePieQuery.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid rootLayout;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\TickStoreManager\TickStorePieQuery.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox StationName;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\TickStoreManager\TickStorePieQuery.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbTickType;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\TickStoreManager\TickStorePieQuery.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Visifire.Charts.Chart myChart;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AFC.WS.UI.UIPage;component/tickstoremanager/tickstorepiequery.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\TickStoreManager\TickStorePieQuery.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.DiagramBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.rootLayout = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.StationName = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.cmbTickType = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.myChart = ((Visifire.Charts.Chart)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

