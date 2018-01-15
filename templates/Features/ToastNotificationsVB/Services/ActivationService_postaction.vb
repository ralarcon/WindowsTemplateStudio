﻿'{**
' This code block include the ToastNotificationsFeatureService Instance in the method 
' `GetActivationHandlers()` in the ActivationService of your project.
'**}

'{[{
Imports Param_RootNamespace.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
            '{[{
            yield Singleton(Of ToastNotificationsFeatureService).Instance
            '}]}
'{--{

            yield Exit Function'}--}
        End Function
    End Class
End Namespace
