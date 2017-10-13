# ActivationService & ActivationHandlers

## ActivationService

The ActivationService is in charge of handling the applications initialization and activation.

With the method `ActivateAsync()` it has one common entry point that is called from the app lifecycle events `OnLaunched`, `OnActivated` and `OnBackgroundActivated`. 
For more information on application lifecycle and its events see [Windows 10 universal Windows platform (UWP) app lifecycle](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/app-lifecycle).

## ActivationHandlers

For choosing the concrete type of activation the ActivationService relies on ActivationHandlers, that are registered in the method `GetActivationHandlers()`.

Each class in the application that can handle application activation should derive from the abstract class `ActivationHandler<T>` (T is the type of ActivationEventArguments the class can handle) and implement the method HandleInternalAsync().
The method `HandleInternalAsync()` is where the actual activation takes place.
The virtual method `CanHandleInternal()` checks if the incoming activation arguments are of the type the ActivationHandler can manage. It can be overwritten to establish further conditions based on the ActivationEventArguments.

### ActivationHandlers sample

We'll have look at the SuspendAndResumeService to see how activation works in detail:

```csharp
protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
{
    return args.PreviousExecutionState == ApplicationExecutionState.Terminated;
}

protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
{
    await RestoreStateAsync();
}

private async Task RestoreStateAsync()
{
    var saveState = await ApplicationData.Current.LocalFolder.ReadAsync<OnBackgroundEnteringEventArgs>(stateFilename);
    if (typeof(Page).IsAssignableFrom(saveState?.Target))
    {
        NavigationService.Navigate(saveState.Target, saveState.SuspensionState);
    }
}
```

The `CanHandleInternal()` method was overwritten here, to handle activation only in case the PreviousExecutionState is Terminated.
The `HandleInternalAsync()` method restores the previously stored application state and navigates to the stored page.

## What else happens on activation?

When executing `ActivatedAsync()`, the ActivationService retrieves the first ActivationHandler able to handle the current activation (evaluating `CanHandleInternal()` of all registered ActivationHandlers) and invokes it.

In case of interactive activation (for example on application launch or on activation from LiveTile) the ActivationService additionally executes the following steps:

* Initialize the App (`InitializeAsync()`) (f.ex register background tasks, set app theme)
* If there is no current content a frame is created and navigation events handlers are added
* If no ActivationHandler is found, a DefaultLaunchActivationHandler is instantiated, that is in charge of navigating to the default page.
* Activate the current window
* Execute StartUp Actions (`StartupAsync()`)

You can use `InitializeAsync()` and `StartupAsync()` to add code that should be executed on application initialization and startup.

## Sample: Add activation from File Association

Let's add activation from a file association:
We created a sample application, that allows to view markdown (.md) files. You can check the sample here: [Markdown Viewer](/samples/activation).

The sample application was created using Windows Template Studio with the following configuration:

* Project Type: Blank
* Framework: MVVM Basic
* Pages: MainPage and MarkdownPage

For viewing the markdown a MarkdownTextBlock from the [UWP Community Toolkit](https://github.com/Microsoft/UWPCommunityToolkit) was added. 

### Set up File Association Activation

First we have to add a file type association declaration in the application manifest, allowing the App to be shown as a default handler for markdown files.

![](resources/activation/DeclarationFileAssociation.PNG) 

Further we have to handle the file activation event by implementing OnFileActivated:

```csharp
protected override async void OnFileActivated(FileActivatedEventArgs args)
{
    await ActivationService.ActivateAsync(args);
}
```

### Add a FileAssociationService

Then we need a service that handles this new type of activation. We'll call it FileAssociationService, it derives from `ApplicationHandler<T>`. 
As it manages activation by File​Activated​Event​Args the signature would be: 

```csharp
internal class FileAssociationService : ActivationHandler<File​Activated​Event​Args>
{

}
```

Next, we'll implement HandleInternalAsync(), to evaluate the event args, and take action:

```csharp
protected override async Task HandleInternalAsync(File​Activated​Event​Args args)
{
    var file = args.Files.FirstOrDefault();

    NavigationService.Navigate(typeof(MarkdownPage), file);

    await Task.CompletedTask;
}
```

### Add the FileAssociationService to ActivationService

Last but not least, we'll have to add our new FileAssociationService to the ActivationHandlers registered in the ActivationService:

```csharp
private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<FileAssociationService>.Instance;
    yield break;
}
```
