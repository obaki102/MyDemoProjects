window.Div = {
    scrollToView: function () {
        var currentElement = document.getElementById('scrollableDiv').lastElementChild;
        currentElement.focus()
        currentElement.scrollIntoView(false);
    }
} 