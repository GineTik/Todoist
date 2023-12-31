import { createEntityByClick, removeEntityByClick, editEntityByClick } from "../grudSender.functions.js";
export default function init(params) {
    const $boardsContent = $(".boards__content");
    const $input = $("input[name='name']");
    createEntityByClick({
        link: params.createLink,
        getData: () => ({ name: $input.val() }),
        $container: $boardsContent,
        errorMessage: "Creating a new board is failed",
        $inputsToClean: [$input]
    });
    removeEntityByClick({
        link: params.removeLink,
        getData: ($clickedBoard) => ({ boardId: $clickedBoard.attr("board-id") }),
        errorMessage: "Remove error! Id incorect or you not is author"
    });
    // edit name
    editEntityByClick({
        link: params.editNameLink,
        getData: ($clickedBoard) => ({ boardId: $clickedBoard.attr("board-id"), name: $input.val() }),
        targetButton: ".edit-btn",
        errorMessage: "Edit error! Id incorect or you not is author",
    });
}
//# sourceMappingURL=board.js.map