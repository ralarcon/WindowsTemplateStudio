﻿'{**
' This code block adds the method `GetSampleModelDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
        '^^
        '{[{

        ' TODO WTS: Remove this once your MasterDetail pages are displaying real data
        Public Async Function GetSampleModelDataAsync() As Task(Of IEnumerable(Of SampleOrder))
            Await Task.CompletedTask

            Return AllOrders()
        End Function
        '}]}
    End Module
End Namespace
