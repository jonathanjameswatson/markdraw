import rangy from 'rangy';

import runPrism from './prism';

const cursor = {
  start: 0,
  end: 0,
  nextLine: 0,
};

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
  } catch {
    return;
  }
}

window.setUp = (editor) => {
  editor.addEventListener("click", () => setCursorPosition(editor));
  editor.addEventListener("keydown", () => setCursorPosition(editor));
  editor.addEventListener("focus", () => setCursorPosition(editor));
}

window.renderMarkdown = (editor, content) => {
  editor.innerHTML = content;
  runPrism();
}

window.getCursor = () => cursor;