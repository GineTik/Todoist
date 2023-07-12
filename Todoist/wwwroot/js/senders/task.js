import { createEntityByClick, editEntityByClick, removeEntityByClick, send } from "../grudSender.functions.js";
export default function init(params) {
    const $tasks_content = $(".tasks__content");
    const $inputs = {
        name: $("input[name='name']"),
        description: $("input[name='description']"),
        datetime: $("input[name='datatime']")
    };
    createEntityByClick({
        link: params.createLink,
        getData: () => ({
            name: $inputs.name.val(),
            description: $inputs.description.val(),
            dateBeforeExpiration: $inputs.datetime.val(),
            boardId: params.boardId
        }),
        $container: $tasks_content,
        errorMessage: "Creating a new task is failed",
        $inputsToClean: Object.keys($inputs).map(key => $inputs[key])
    });
    removeEntityByClick({
        link: params.removeLink,
        getData: ($clickedTask) => ({ taskId: $clickedTask.attr("task-id") }),
        errorMessage: "Remove error! Id incorect or you not is author"
    });
    editEntityByClick({
        link: params.editLink,
        getData: ($clickedTask) => ({
            taskId: $clickedTask.attr("task-id"),
            name: $inputs.name.val(),
            description: $inputs.description.val(),
            dateBeforeExpiration: $inputs.datetime.val(),
        }),
        targetButton: ".edit-btn",
        errorMessage: "Edit error! Id incorect or you not is author",
    });
    editEntityByClick({
        link: params.toggleClosedValueLink,
        getData: ($clickedTask) => ({ taskId: $clickedTask.attr("task-id") }),
        targetButton: ".toggle-closed-value",
        errorMessage: "Edit error! Id incorect or you not is author"
    });
    $tasks_content.sortable({
        stop: function (event, ui) {
            let allPositions = $(this).sortable("toArray", { attribute: "task-id" }).map(function (value, index) {
                return { taskId: value, newPosition: index };
            });
            send(params.editPositionLink, { newPositions: allPositions }, "Edit position error");
        }
    });
}
//# sourceMappingURL=task.js.map