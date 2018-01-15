﻿Imports Windows.UI.Xaml.Media.Animation
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Param_ItemNamespace.Models
Imports Param_ItemNamespace.Services

Namespace Views
    Public NotInheritable Partial Class MasterDetailViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _selected As SampleOrder

        Public Property Selected As SampleOrder
            Get
                Return _selected
            End Get
            Set
                [Param_Setter](_selected, value)
            End Set
        End Property

        Public Property SampleItems As ObservableCollection(Of SampleOrder) = new ObservableCollection(Of SampleOrder)

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, AddressOf MasterDetailViewPage_Loaded
        End Sub

        Private Async Sub MasterDetailViewPage_Loaded(sender As Object, e As RoutedEventArgs)
            SampleItems.Clear()

            Dim data = Await SampleDataService.GetSampleModelDataAsync()

            For Each item As SampleOrder In data
                SampleItems.Add(item)
            Next

            If MasterDetailsViewControl.ViewState = MasterDetailsViewState.Both Then
                Selected = SampleItems.First()
            End If
        End Sub
    End Class
End Namespace
