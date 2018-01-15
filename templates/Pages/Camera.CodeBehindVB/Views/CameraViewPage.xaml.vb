﻿Imports Param_ItemNamespace.EventHandlers

Namespace Views
    Public NotInheritable Partial Class CameraViewPage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub CameraControl_PhotoTaken(sender As Object, e As CameraControlEventArgs)
            If Not String.IsNullOrEmpty(e.Photo) Then
                Photo.Source = New BitmapImage(New Uri(e.Photo))
            End If
        End Sub
    End Class
End Namespace
