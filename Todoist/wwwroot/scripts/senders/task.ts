import { createEntityByClick, editEntityByClick, removeEntityByClick } from "../grudSender.functions.js";

export default function init(params:
{
    createLink: string,
    removeLink: string,
    editLink: string,
    boardId: number
}) {

    const $tasks_content = $(".tasks__content");
    const $inputs = {
        name: $("input[name='name']"),
        description: $("input[name='description']"),
        datetime: $("input[name='datatime']")
    }

    createEntityByClick({
        link: params.createLink,
        getData: () => ({
            name: $inputs.name.val(),
            description: $inputs.description.val(),
            closingDate: $inputs.datetime.val(),
            boardId: params.boardId
        }),
        $container: $tasks_content,
        errorMessage: "Creating a new task is failed",
        $inputsToClean: Object.keys($inputs).map(key => $inputs[key])
    });

    removeEntityByClick({
        link: params.removeLink,
        getData: ($clickedTask) =>({ taskId: $clickedTask.attr("task-id") }),
        $container: $tasks_content,
        errorMessage: "Take exception! Id incorect or you not is author"
    });

    editEntityByClick({
        link: params.editLink,
        getData: ($clickedTask) => ({
            taskId: $clickedTask.attr("task-id"),
            name: $inputs.name.val(),
            description: $inputs.description.val(),
            closingDate: $inputs.datetime.val(),
        }),
        $container: $tasks_content,
        errorMessage: "Edit error! Id incorect or you not is author",
    });
}