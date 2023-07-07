
const $errorField = $(".error-field");

function send(link: string, data: object, errorMessage: string): JQuery.jqXHR {
    return $.post(link, data)
        .fail(function (e) {
            $errorField.text(errorMessage);
            console.error(e);
        });
}

export function createEntityByClick(params: { link: string, getData: Function, $container: JQuery<HTMLElement>, errorMessage: string, $inputsToClean: JQuery<HTMLElement>[] }) {
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

export function removeEntityByClick(params: { link: string, getData: Function, $container: JQuery<HTMLElement>, errorMessage: string }) {
    var { link, getData, $container, errorMessage } = params;

    $container.on("click", ".remove-btn", function (e) {
        let $clickedBoard = $(e.target).parent();

        send(link, getData($clickedBoard), errorMessage)
            .done(() => {
                $clickedBoard.remove();
            });
    });
}