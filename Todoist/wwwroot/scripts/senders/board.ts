import { createEntityByClick, removeEntityByClick } from "../grudSender.functions.js";

export default function init(params: { createLink: string, removeLink: string }) {

    const $boardsContent = $(".boards__content");
    const $createInput = $(".create-input");

    createEntityByClick(
        params.createLink,
        () => ({ name: $createInput.val() }),
        $boardsContent,
        'Creating a new board is failed',
    );

    removeEntityByClick(
        params.removeLink,
        ($clickedBoard) => ({ id: $clickedBoard.attr("board-id") }),
        $boardsContent,
        'Take exception! Id incorect or you not is author'
    );
}