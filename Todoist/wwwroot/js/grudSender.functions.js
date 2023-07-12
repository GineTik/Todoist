const $errorField = $(".error-field");
export function send(link, data, errorMessage) {
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
    const { link, getData, errorMessage } = params;
    $(document).on("click", ".remove-btn", function () {
        let $clickedElement = $(this).parent();
        send(link, getData($clickedElement), errorMessage)
            .done(() => {
            $clickedElement.remove();
        });
    });
}
export function editEntityByClick(params) {
    const { link, getData, targetButton, errorMessage } = params;
    $(document).on("click", targetButton, function () {
        let $clickedElement = $(this).parent();
        send(link, getData($clickedElement), errorMessage)
            .done((html) => {
            $(html).insertBefore($clickedElement);
            $clickedElement.remove();
        });
    });
}
//# sourceMappingURL=grudSender.functions.js.map