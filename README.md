# Evo.Blazor.Splitters

Goal is to create a native splitter implementation for blazor

# Purpose 

1) Learn what is needed to create a decently complex custom control in Blazor
2) Create a splitter control that can be used to seperate a control area from a canvas

# Goals

- Avoid as much javascript as possible - The goal is to create a native solution if at all possible and not have to mess with a javascript implementation.  Split.js already does this work well, and the purpose of this project is to learn what it takes to 
- Inject as litle as possible into the DOM when implementing the control.  Gutter/Splitter bars will need to be added to seperate the panes, but one of the goals of this project is to learn how the DIV elements already present in the markup can be used.  There are two ways of approaching this. The first is to use javascript to edit what is already there.  The second is to use an HTML agility pack and parse the HTML and rebuild the outer divs.  Currently leaning toward the second as it eliminates javascript from the equation, which is another goal.

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
The next step is to create a div that appears after every pane div.  

## Use Cascading Values or Models to Propigate Settings

## Rendering

One of the goals of the project is to keep the dom overhead as light possible and not to impose elements onto to the developer if they are not needed.  

# References

[Column Resizable](https://github.com/alvaro-prieto/colResizable/blob/master/colResizable-1.6.js)

[Splitter.js Reference Implementation](https://github.com/nathancahill/split/blob/a206479d81a6b2a69ba5742f57fd378d997fd8e7/packages/splitjs/src/split.js#L42)

[Modernizer Calc Function](https://stackoverflow.com/questions/16625140/js-feature-detection-to-detect-the-usage-of-webkit-calc-over-calc/16625167#16625167)

[Radzen Blazor (MIT Licensed)](https://github.com/radzenhq/radzen-blazor/tree/master/Radzen.Blazor) - Specifically useful was the [tab control implementation]()

[Working Example using Splitter.js](http://jsfiddle.net/aguiguy/y2x7fdnu/)

[BlazorSplit](https://github.com/BlazorComponents/BlazorSplit) - An older implementation of a blazor split control that uses javascript.

[Investigating Drap and Drop with Blazor](https://chrissainty.com/investigating-drag-and-drop-with-blazor/)

[Browser Interop](https://github.com/RemiBou/BrowserInterop/tree/master/src/BrowserInterop)
