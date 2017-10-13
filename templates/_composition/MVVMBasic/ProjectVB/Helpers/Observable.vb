﻿Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Namespace Helpers
    Public Class Observable
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
 
        Friend Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
                return
            End If
 
            storage = value
            OnPropertyChanged(propertyName)
        End Sub
 
        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
