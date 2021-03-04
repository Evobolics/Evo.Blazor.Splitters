# Evo.Blazor.Splitters

Goal is to create a native splitter implementation for blazor

# Implementation

## Get Two Divs to Show Up

### Setup Child Content

## Build the Gutter / Splitter Bar

### Setup Services

### Made Services Injectable

### Setup Pane Registration

### Add Logic to Determine If Pane is Last

## Format the Controls Statically

Make the bar the right height, and color, give the panes a background color, use a calc expression to handle height.

## Make Formating Appear Dynamically
The next step is to create a div that appears after every 

## Use Cascading Values or Models to Propigate Settings

# References

[Column Resizable](https://github.com/alvaro-prieto/colResizable/blob/master/colResizable-1.6.js)

[Splitter.js Reference Implementation](https://github.com/nathancahill/split/blob/a206479d81a6b2a69ba5742f57fd378d997fd8e7/packages/splitjs/src/split.js#L42)

[Modernizer Calc Function](https://stackoverflow.com/questions/16625140/js-feature-detection-to-detect-the-usage-of-webkit-calc-over-calc/16625167#16625167)

[Radzen Blazor (MIT Licensed)](https://github.com/radzenhq/radzen-blazor/tree/master/Radzen.Blazor) - Specifically useful was the [tab control implementation]()

[Working Example using Splitter.js](http://jsfiddle.net/aguiguy/y2x7fdnu/)

[BlazorSplit](https://github.com/BlazorComponents/BlazorSplit) - An older implementation of a blazor split control that uses javascript.

[Investigating Drap and Drop with Blazor](https://chrissainty.com/investigating-drag-and-drop-with-blazor/)

[Browser Interop](https://github.com/RemiBou/BrowserInterop/tree/master/src/BrowserInterop)
