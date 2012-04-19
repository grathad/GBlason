﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GBlason.ViewModel;

namespace GBlason.Control.Aggregate
{
    /// <summary>
    /// Interaction logic for RecentDocumentMenu.xaml
    /// </summary>
    public partial class RecentDocumentMenu
    {
        public RecentDocumentMenu()
        {
            InitializeComponent();
        }

        private void FlagRecentItemExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var recentItem = e.Parameter as RecentFileViewModel;
            if(recentItem != null)
                recentItem.IsFixed = !recentItem.IsFixed;
        }
    }
}
