
export function getGlobalScope() {
    return (typeof window !== 'undefined') ? window : null
}

export function showPrompt(message) {
    return prompt(message, 'Type anything here');
}

export function removeAllSelections() {
    var selection = window.getSelection ? window.getSelection() : document.selection ? document.selection : null;
    if (!!selection) selection.empty ? selection.empty() : selection.removeAllRanges();
}
