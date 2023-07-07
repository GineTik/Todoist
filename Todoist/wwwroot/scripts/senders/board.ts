import { createEntityByClick, removeEntityByClick } from "../grudSender.functions.js";

export default function init(params: { createLink: string, removeLink: string }) {

    const $boardsContent = $(".boards__content");
    const $createInput = $(".create-input");

    createEntityByClick({
        link: params.createLink,
        getData: () => ({ name: $createInput.val() }),
        $container: $boardsContent,
        errorMessage: 'Creating a new board is failed',
        $inputsToClean: [$createInput]
    });

    removeEntityByClick({
        link: params.removeLink,
        getData: ($clickedBoard) =>({ id: $clickedBoard.attr("board-id") }),
        $container: $boardsContent,
        errorMessage: 'Take exception! Id incorect or you not is author'
    });
}