import rangy from 'rangy';
import runPrism from './prism';

const cursor = {
  start: 0,
  end: 0
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
  let offset;
  const caret = type == 'Caret';

  if (caret) {
    node = selection.anchorNode;
    offset = selection.anchorOffset;
  } else {
    if (!selection.isBackwards()) {
      node = selection.anchorNode;
      offset = selection.anchorOffset;
    } else {
      node = selection.focusNode;
      offset = selection.focusOffset;
    }
  }

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

  cursor.start = parseInt(parentNode.getAttribute("i"), 10) + totalOffset;

  console.log(cursor);
}

window.setUp = (editor) => {
  editor.addEventListener("click", () => setCursorPosition(editor))
}

window.renderMarkdown = (editor, content) => {
  editor.innerHTML = content;
  runPrism()
}