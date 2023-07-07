const $errorField = $(".error-field");
export function createEntityByClick(link, getData, $container, errorMessage) {
    const $createBtn = $(".create-btn");
    const $inputs = $(".create-input");
    $createBtn.click(function () {
        $.post(link, getData())
            .done(function (boardHtml) {
            $container.append(boardHtml);
            $inputs.val("");
        })
            .fail(function (e) {
            $errorField.text(errorMessage);
            console.log(e);
        });
    });
}
export function removeEntityByClick(link, getData, $container, errorMessage) {
    $container.on("click", ".remove-btn", function (e) {
        let $clickedBoard = $(e.target).parent();
        $.post(link, getData($clickedBoard))
            .done(function () {
            $clickedBoard.remove();
        })
            .fail(function (e) {
            console.error(e);
            $errorField.text("Take exception! Id incorect or you not is author");
        });
    });
}
//# sourceMappingURL=GRUDSender.js.map