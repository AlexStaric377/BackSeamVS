using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Windows.Media;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public class ComplaintViewModel : DependencyObject
    {

        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        public static readonly DependencyProperty FilterTextProperty =
       DependencyProperty.Register("FilterText", typeof(string), typeof(ComplaintViewModel), new PropertyMetadata(""));

        // Свойтсво отвечает за  отображение коллекции и её фильтрацию
        public ICollectionView ItemsComplaint
        {
            get { return (ICollectionView)GetValue(ItemsComplaintProperty); }
            set { SetValue(ItemsComplaintProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsComplaintProperty =
            DependencyProperty.Register(" ItemsComplaint", typeof(ICollectionView), typeof(ComplaintViewModel), new PropertyMetadata(null));

        // Конструктор т.е. интерфейс 
        public ComplaintViewModel()
        {
            // GetDefaultView возвращает екземпляр

            //ItemsComplaint = CollectionViewSource.GetDefaultView(Complaint.GetComplaints());
 
        }

        public ICollectionView ItemsFeature
        {
            get { return (ICollectionView)GetValue(ItemsFeatureProperty); }
            set { SetValue(ItemsFeatureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsFeatureProperty =
            DependencyProperty.Register(" ItemsFeature", typeof(ICollectionView), typeof(ComplaintViewModel), new PropertyMetadata(null));



    }

    public class FeatureViewModelDependency : DependencyObject
    {

        // Свойтсво отвечает за  отображение коллекции и её фильтрацию
        public ICollectionView ItemsFeature
        {
            get { return (ICollectionView)GetValue(ItemsFeatureProperty); }
            set { SetValue(ItemsFeatureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsFeatureProperty =
            DependencyProperty.Register(" ItemsFeature", typeof(ICollectionView), typeof(FeatureViewModelDependency), new PropertyMetadata(null));

        // Конструктор т.е. интерфейс 
        public FeatureViewModelDependency()
        {
            // GetDefaultView возвращает екземпляр

            //ItemsFeature = CollectionViewSource.GetDefaultView(Feature.GetFeatures());

        }

    }

  

    
}
