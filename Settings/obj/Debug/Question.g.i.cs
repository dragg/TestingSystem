﻿#pragma checksum "..\..\Question.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "426F76090D09004F24024F5529460824"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using System.Windows.Shell;


namespace Settings {
    
    
    /// <summary>
    /// WQuestion
    /// </summary>
    public partial class WQuestion : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbQuestion;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbNote;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbNote2;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btDeleteAnswer;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btChangeAnswer;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock2;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbAnswers;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox right;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbSubject;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tempAnswer;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button1;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbPathToFile;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\Question.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btSave;
        
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
            System.Uri resourceLocater = new System.Uri("/Settings;component/question.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Question.xaml"
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
            
            #line 4 "..\..\Question.xaml"
            ((Settings.WQuestion)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            
            #line 4 "..\..\Question.xaml"
            ((Settings.WQuestion)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbQuestion = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.tbNote = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.tbNote2 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.btDeleteAnswer = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\Question.xaml"
            this.btDeleteAnswer.Click += new System.Windows.RoutedEventHandler(this.DeleteAnswer);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btChangeAnswer = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\Question.xaml"
            this.btChangeAnswer.Click += new System.Windows.RoutedEventHandler(this.ChangeAnswer);
            
            #line default
            #line hidden
            return;
            case 7:
            this.textBlock2 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.lbAnswers = ((System.Windows.Controls.ListBox)(target));
            
            #line 59 "..\..\Question.xaml"
            this.lbAnswers.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.listAnswers_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.right = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 10:
            this.cmbSubject = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 11:
            this.tempAnswer = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.button1 = ((System.Windows.Controls.Button)(target));
            
            #line 89 "..\..\Question.xaml"
            this.button1.Click += new System.Windows.RoutedEventHandler(this.AddAnswer);
            
            #line default
            #line hidden
            return;
            case 13:
            this.tbPathToFile = ((System.Windows.Controls.TextBox)(target));
            return;
            case 14:
            this.btSave = ((System.Windows.Controls.Button)(target));
            
            #line 97 "..\..\Question.xaml"
            this.btSave.Click += new System.Windows.RoutedEventHandler(this.SaveAndClose);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

