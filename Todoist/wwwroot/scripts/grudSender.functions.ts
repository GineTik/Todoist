
const $errorField = $(".error-field");

export function send(link: string, data: object, errorMessage: string): JQuery.jqXHR {
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

export function removeEntityByClick(params: { link: string, getData: Function, errorMessage: string }) {
    const { link, getData, errorMessage } = params;

    $(document).on("click", ".remove-btn", function () {
        let $clickedElement = $(this).parent();

        send(link, getData($clickedElement), errorMessage)
            .done(() => {
                $clickedElement.remove();
            });
    });
}

export function editEntityByClick(params: {link: string, getData: Function, targetButton: string, errorMessage: string }) {
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