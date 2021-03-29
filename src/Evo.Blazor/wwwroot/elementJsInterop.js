
export function addClass(element, className) {

}

export function removeClass(element, className) {

}

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

const resizeObservers = new Map();

export function create(key, dotNetObserver, domElement) {

    console.log(`reference: ${dotNetObserver !== null}, domElement: ${domElement !== null}`);

    const resizeObserver = new ResizeObserver((entries, observer) => {
        for (let entry of entries)
        {
            const measurements = getElementMeasurements(domElement);

            dotNetObserver.invokeMethodAsync('OnResizeObserved', measurements);
            break;
        }
    });

    var entry = {
        resizeObserver: resizeObserver,
        key: key
    };

    resizeObservers.set(key, entry);

    resizeObserver.observe(domElement);
}

