import 'core-js/stable';
import 'regenerator-runtime/runtime.js';

import rangy from 'rangy';

import runPrism from './prism';
import binarySearch from './binarySearch';

const cursor = {
  start: 0,
  end: 0,
  nextLine: 0,
};

let dotnetReference = null;

const textElementNames = ['EM', 'A', 'STRONG', 'SPAN'];

const getI = (node, offset) => {
  let parentNode = node.nodeType == 3 ? node.parentNode : node;

  while (parentNode?.getAttribute('i') == null) {
    parentNode = parentNode.parentNode;
  }

  let totalOffset = 0;
  if (parentNode.nodeName != 'PRE') {
    const offsetRange = rangy.createRange();
    offsetRange.setStartAndEnd(parentNode, 0, node, offset);
    totalOffset = offsetRange.toString().length;
  }

  return [parentNode, parseInt(parentNode.getAttribute('i'), 10) + totalOffset];
}

const setCursorPosition = (contentDiv) => {
  const selection = rangy.getSelection();
  const type = selection.nativeSelection.type.toString();

  if (
    selection.rangeCount == 0
    || type == 'None'
    || !contentDiv.contains(selection.getRangeAt(0).commonAncestorContainer)
  ) {
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

    while (
      node.nextElementSibling == null
      || textElementNames.includes(node.nextElementSibling.tagName)
    ) {
      node = node.parentElement;
      if (node == contentDiv) {
        cursor.nextLine = cursor.end;
        return;
      }
    }

    const nextLineI = node.nextElementSibling?.getAttribute('i');

    cursor.nextLine = nextLineI == null ? cursor.end : parseInt(nextLineI);
  } catch (e) {
    return;
  }
}

const handleChange = async (editor, event) => {
  const { data, inputType } = event;
  if (data == null && inputType != 'insertParagraph') {
    const backwards = inputType == 'deleteContentBackward';
    await removeText(editor, backwards);
  } else {
    await insertText(editor, data || '\n ');
  }
}

const handlePaste = async (editor, event) => {
  event.preventDefault();

  const text = (event.clipboardData || window.clipboardData).getData('text');

  await insertText(editor, text);
}

const moveCursorTo = (editor, i) => {
  console.log(i);
  const elements = editor.querySelectorAll('[i]');

  const elementIndex = binarySearch(
    elements,
    null,
    (middleElement, needle, middleIndex, haystack) => {
      const middleI = parseInt(middleElement.getAttribute('i'), 10);
      if (middleI > i) {
        return 1;
      }
      if (middleIndex == elements.length - 1 || middleIndex == 0) {
        return 0;
      }
      const afterMiddleI = parseInt(elements[middleIndex + 1].getAttribute('i'), 10);
      if (afterMiddleI > i) {
        return 0;
      }
      return -1;
    }
  );

  const element = elements[elementIndex];
  const elementI = parseInt(element.getAttribute('i'), 10);
  const offset = i - elementI;

  const range = document.createRange();
  const selection = window.getSelection();

  range.setStart(element.firstChild, Math.min(element.textContent.length, offset));
  range.collapse(true);

  selection.removeAllRanges();
  selection.addRange(range);

  cursor.start = i;
  cursor.end = i;

  let nextIndex = elementIndex + 1;
  let nextLine = cursor.start;

  while (nextIndex < elements.length && nextLine == cursor.start) {
    const nextElement = elements[nextIndex];
    if (!textElementNames.includes(nextElement.nodeName)) {
      nextLine = parseInt(nextElement.getAttribute('i'), 10);
    }
    nextIndex += 1;
  }

  cursor.nextLine = nextLine;
}

const insertText = async (editor, text) => {
  const i = await dotnetReference.invokeMethodAsync('InsertText', text, cursor);
  moveCursorTo(editor, i);
}

const removeText = async (editor, backwards) => {
  const i = await dotnetReference.invokeMethodAsync('RemoveText', backwards, cursor);
  moveCursorTo(editor, i);
}

window.setReference = (reference) => {
  dotnetReference = reference;
}

window.setUp = (editor) => {
  editor.addEventListener('mouseup', () => setCursorPosition(editor));
  editor.addEventListener('keyup', () => setCursorPosition(editor));
  editor.addEventListener('focus', () => setCursorPosition(editor));
  editor.addEventListener('input', (event) => handleChange(editor, event));
  editor.addEventListener('paste', (event) => handlePaste(editor, event));
}

window.renderMarkdown = (editor, content) => {
  editor.innerHTML = content;
  runPrism();
}

window.getCursor = () => cursor;
