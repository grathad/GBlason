﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using FormatManager.Serializer;
using GBlason.Common.Converter;
using GBlason.Properties;

namespace GBlason.ViewModel
{
    public class GbrFileViewModel : INotifyPropertyChanged
    {
        #region singleton

        private static GbrFileViewModel _singleton;

        public static GbrFileViewModel GetResources
        {
            get
            {
                return _singleton ?? (_singleton = new GbrFileViewModel());
            }
        }

        private GbrFileViewModel()
        {
            ScaledForMenuShapeResources = new ObservableCollection<ShapeViewModel>();
            SupportedLanguages = new ObservableCollection<CultureInfo>();
        }

        #endregion

        #region initialisations

        public void InitFromFile()
        {
            var manager = new GbrManager();
            var path = Path.Combine(Directory.GetCurrentDirectory(), Settings.Default.GbrLocalFile);
            var formatGbr = manager.LoadGbrFile(path);
            //chargement des shapes
            foreach (var newShapeVm in formatGbr.Shapes)
            {
                ScaledForMenuShapeResources.Add(newShapeVm.ConvertToViewModel());
            }
            SupportedLanguages.Add(CultureInfo.GetCultureInfo("en-GB"));
            SupportedLanguages.Add(CultureInfo.GetCultureInfo("fr-FR"));
            SupportedLanguages.Add(CultureInfo.GetCultureInfo("ja-JP"));
            //update culture
            if (!SupportedLanguages.Contains(GlobalApplicationViewModel.GetApplicationViewModel.PropertyPreferenceCultureInfo))
                GlobalApplicationViewModel.GetApplicationViewModel.PropertyPreferenceCultureInfo = CultureInfo.CurrentCulture;
            if (!SupportedLanguages.Contains(GlobalApplicationViewModel.GetApplicationViewModel.PropertyPreferenceCultureInfo))
                GlobalApplicationViewModel.GetApplicationViewModel.PropertyPreferenceCultureInfo = CultureInfo.GetCultureInfo("en-GB");

            //affect the thread cultureinfo
            Thread.CurrentThread.CurrentCulture =
                GlobalApplicationViewModel.GetApplicationViewModel.PropertyPreferenceCultureInfo;
            Thread.CurrentThread.CurrentUICulture =
                GlobalApplicationViewModel.GetApplicationViewModel.PropertyPreferenceCultureInfo;

            OnPropertyChanged("GetResources");
        }

        private void InitFromWebService()
        {}

        #endregion

        #region Resources

        public ObservableCollection<ShapeViewModel> ScaledForMenuShapeResources { get; set; }

        public ObservableCollection<CultureInfo> SupportedLanguages { get; set; }

        #endregion

        #region INotifyPropertyChanged Members

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
