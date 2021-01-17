import 'core-js/stable';
import 'regenerator-runtime/runtime.js';

import rangy from 'rangy';

import runPrism from './prism';

const cursor = {
  start: 0,
  end: 0,
  nextLine: 0,
};

let dotnetReference = null;

const getI = (node, offset) => {
  let parentNode = node.nodeType == 3 ? node.parentNode : node;

  while (parentNode?.getAttribute("i") == null) {
    parentNode = parentNode.parentNode;
  }

  let totalOffset = 0;
  if (parentNode.nodeName != "PRE") {
    const offsetRange = rangy.createRange();
    offsetRange.setStartAndEnd(parentNode, 0, node, offset);
    totalOffset = offsetRange.toString().length;
  }

  return [parentNode, parseInt(parentNode.getAttribute("i"), 10) + totalOffset];
}

const setCursorPosition = (contentDiv) => {
  const selection = rangy.getSelection();
  const type = selection.nativeSelection.type.toString();

  if (selection.rangeCount == 0) {
    return;
  }

  if (type == 'None') {
    return;
  }

  if (!contentDiv.contains(selection.getRangeAt(0).commonAncestorContainer)) {
    return;
  }

  let node;

  try {
    if (type == 'Caret') {
      const [resultNode, i] = getI(selection.anchorNode, selection.anchorOffset);
      cursor.start = i;
      cursor.end = i;
      node = resultNode;
    } else {
      if (!selection.isBackwards()) {
        [, cursor.start] = getI(selection.anchorNode, selection.anchorOffset);
        [node, cursor.end] = getI(selection.focusNode, selection.focusOffset);
      } else {
        [, cursor.start] = getI(selection.focusNode, selection.focusOffset);
        [node, cursor.end] = getI(selection.anchorNode, selection.anchorOffset);
      }
    }

    while (node.nextElementSibling == null) {
      node = node.parentElement;
      if (node == contentDiv) {
        cursor.nextLine = cursor.end;
        return;
      }
    }

    const nextLineI = node.nextElementSibling?.getAttribute("i");

    cursor.nextLine = nextLineI == null ? cursor.end : parseInt(nextLineI);
  } catch (e) {
    return;
  }
}

const handleChange = async (event) => {
  const { data } = event;
  if (data == null) {
    const backwards = event.inputType == "deleteContentBackward";
    await removeText(backwards);
  } else {
    await insertText(data);
  }
}

const handlePaste = async (event) => {
  event.preventDefault();

  const text = (event.clipboardData || window.clipboardData).getData('text');

  await insertText(text)
}

const moveCursorTo = (i) => {
  console.log(i);
}

const insertText = async (text) => {
  const i = await dotnetReference.invokeMethodAsync('InsertText', text, cursor);
  moveCursorTo(i);
}

const removeText = async (backwards) => {
  const i = await dotnetReference.invokeMethodAsync('RemoveText', backwards, cursor);
  moveCursorTo(i);
}

window.setReference = (reference) => {
  dotnetReference = reference;
}

window.setUp = (editor) => {
  editor.addEventListener("mouseup", () => setCursorPosition(editor));
  editor.addEventListener("keyup", () => setCursorPosition(editor));
  editor.addEventListener("focus", () => setCursorPosition(editor));
  editor.addEventListener("input", handleChange);
  editor.addEventListener("paste", handlePaste);
}

window.renderMarkdown = (editor, content) => {
  editor.innerHTML = content;
  runPrism();
}

window.getCursor = () => cursor;
