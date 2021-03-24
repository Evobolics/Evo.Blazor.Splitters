// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.



export function getElementMeasurements(element) {
    
    let boundRect = element.getBoundingClientRect();

    let retValue =
    {
        BoundingClientRect : boundRect,
        ClientHeight: element.clientHeight,
        ClientLeft: element.clientLeft,
        ClientTop: element.clientTop,
        ClientWidth: element.clientWidth,
        OffsetHeight: element.offsetHeight,
        OffsetWidth: element.offsetWidth,
        OffsetTop: element.offsetTop,
        OffsetLeft: element.offsetLeft,
        ScrollHeight: element.scrollHeight,
        OffsetWidth: element.scrollWidth,
        OffsetTop: element.scrollTop,
        OffsetLeft: element.ccrollLeft,
        
    };

    return retValue;

}

