const $errorField = $(".error-field");
function send(link, data, errorMessage) {
    return $.post(link, data)
        .fail(function (e) {
        $errorField.text(errorMessage);
        console.error(e);
    });
}
export function createEntityByClick(params) {
    const $createBtn = $(".create-btn");
    const { link, getData, $container, errorMessage, $inputsToClean } = params;
    $createBtn.click(function () {
        send(link, getData(), errorMessage)
            .done((html) => {
            $container.append(html);
            $inputsToClean.forEach($input => {
                $input.val("");
            });
        });
    });
}
export function removeEntityByClick(params) {
    const { link, getData, $container, errorMessage } = params;
    $container.on("click", ".remove-btn", function (e) {
        let $clickedBoard = $(e.target).parent();
        send(link, getData($clickedBoard), errorMessage)
            .done(() => {
            $clickedBoard.remove();
        });
    });
}
export function editEntityByClick(params) {
    const { link, getData, $container, errorMessage } = params;
    $container.on("click", ".edit-btn", function (e) {
        let $clickedElement = $(e.target).parent();
        send(link, getData($clickedElement), errorMessage)
            .done((html) => {
            $(html).insertBefore($clickedElement);
            $clickedElement.remove();
        });
    });
}
//# sourceMappingURL=grudSender.functions.js.map