import { Editor } from '@tiptap/core';

/*
  Selects a consecutive range of block nodes by their natural position
  (first block = 1, second = 2, etc.). It counts only block nodes, remembers
  the first (startIndex) and last (endIndex), and creates a text selection
  inside their content (pos+1 .. pos+nodeSize-1) so we avoid a NodeSelection
  and operate on the text within. Stops once both edges are found.
*/
export function selectBlocks(editor: Editor, startIndex: number, endIndex: number) {
  if (endIndex < startIndex) {
    return;
  }

  let count = 0;
  let from = -1;
  let to = -1;

  editor.state.doc.descendants((node, pos) => {
    if (!node.isBlock) {
      return;
    }

    count++;

    if (count === startIndex) {
      from = pos + 1;
    }

    if (count === endIndex) {
      to = pos + node.nodeSize - 1;
    }

    if (count >= endIndex && from !== -1 && to !== -1) {
      return false;
    }
  });

  if (from >= 0 && to > from) {
    editor.chain().setTextSelection({ from, to }).run();
  }
}

/*
  Finds the first text node whose entire text equals fullText and selects it
  (or a substring if startOffset / endOffset given). Silent no-op if not found.
*/
export function selectText(editor: Editor, fullText: string, startOffset?: number, endOffset?: number) {
  let base = -1;
  let len = 0;

  editor.state.doc.descendants((node, pos) => {
    if (base === -1 && node.isText && node.text === fullText) {
      base = pos;
      len = node.text.length;

      return false;
    }
  });

  if (base === -1) {
    return;
  }

  const from = base + (startOffset ?? 0);
  const to = base + (endOffset ?? len);

  if (to > from) {
    editor.chain().setTextSelection({ from, to }).run();
  }
}
