# Sample MVVM Project

This project is just to show how to create your initial WPF application.

### Libraries

#### Ticker.Core

The core functionality for the Ticker application. This is platform-independent and has no concept of a view.

#### TickerMvvmDI.Wpf

Sample WPF application which interacts with Ticker.Core


### Not Relevant 

###### Ticker.Shared

Small utility library with platform-independent helper classes. In a real example this would be moved out of the application into a shared code framework.

###### Ticker.WpfShared

Small utility library with WPF-specific helper classses, much of which is stripped from [Prism](https://github.com/PrismLibrary/Prism). Like Ticker.Shared, this would be moved into a shared code library separate from the project.
