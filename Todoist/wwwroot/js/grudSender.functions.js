const $errorField = $(".error-field");
function send(link, data, errorMessage) {
    return $.post(link, data)
        .fail(function (e) {
        $errorField.text(errorMessage);
        console.error(e);
    });
}
export function createEntityByClick(link, getData, $container, errorMessage) {
    const $createBtn = $(".create-btn");
    const $inputs = $(".create-input");
    $createBtn.click(function () {
        send(link, getData(), errorMessage)
            .done((html) => {
            $container.append(html);
            $inputs.val("");
        });
    });
}
export function removeEntityByClick(link, getData, $container, errorMessage) {
    $container.on("click", ".remove-btn", function (e) {
        let $clickedBoard = $(e.target).parent();
        send(link, getData($clickedBoard), errorMessage)
            .done(() => {
            $clickedBoard.remove();
        });
    });
}
//# sourceMappingURL=grudSender.functions.js.map