
const $errorField = $(".error-field");

function send(link: string, data: object, errorMessage: string): JQuery.jqXHR {
    return $.post(link, data)
        .fail(function (e) {
            $errorField.text(errorMessage);
            console.error(e);
        });
}

export function createEntityByClick(link: string, getData: Function, $container: JQuery<HTMLElement>, errorMessage: string) {
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

export function removeEntityByClick(link: string, getData: Function, $container: JQuery<HTMLElement>, errorMessage: string) {
    $container.on("click", ".remove-btn", function (e) {
        let $clickedBoard = $(e.target).parent();

        send(link, getData($clickedBoard), errorMessage)
            .done(() => {
                $clickedBoard.remove();
            });
    });
}