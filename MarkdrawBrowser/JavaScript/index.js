import rangy from 'rangy';
import runPrism from './prism';

const cursor = {
  start: 0,
  end: 0
}

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

  return parseInt(parentNode.getAttribute("i"), 10) + totalOffset;
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

  try {
    if (type == 'Caret') {
      const i = getI(selection.anchorNode, selection.anchorOffset)
      cursor.start = i;
      cursor.end = i;
    } else {
      if (!selection.isBackwards()) {
        cursor.start = getI(selection.anchorNode, selection.anchorOffset);
        cursor.end = getI(selection.focusNode, selection.focusOffset);
      } else {
        cursor.start = getI(selection.focusNode, selection.focusOffset);
        cursor.end = getI(selection.anchorNode, selection.anchorOffset);
      }
    }
  } catch {
    return;
  }

  console.log(cursor);
}

window.setUp = (editor) => {
  editor.addEventListener("click", () => setCursorPosition(editor))
}

window.renderMarkdown = (editor, content) => {
  editor.innerHTML = content;
  runPrism()
}