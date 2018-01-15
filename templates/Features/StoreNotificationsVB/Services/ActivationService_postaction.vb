﻿'{**
' These code blocks include the StoreNotificationsFeatureService Instance in the method `GetActivationHandlers()`
' and initializes it in the method `InitializeAsync()` in the ActivationService of your project.
'**}

'{[{
Imports Param_RootNamespace.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Private Async Function InitializeAsync() As Task
            '{[{
            Await Singleton(Of StoreNotificationsFeatureService).Instance.InitializeAsync()
            '}]}
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
            '{[{
            yield Singleton(Of StoreNotificationsFeatureService).Instance
            '}]}
'{--{
            yield Exit Function'}--}
        End Function
    End Class
End Namespace
