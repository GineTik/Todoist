import { createEntityByClick, removeEntityByClick } from "../grudSender.functions.js";
export default function init(params) {
    const $tasks_content = $(".tasks__content");
    const $createInput = $(".create-input");
    createEntityByClick(params.createLink, () => ({ name: $createInput.val(), boardId: params.boardId }), $tasks_content, 'Creating a new task is failed');
    removeEntityByClick(params.removeLink, ($clickedTask) => ({ taskId: $clickedTask.attr("task-id") }), $tasks_content, 'Take exception! Id incorect or you not is author');
}
//# sourceMappingURL=task.js.map