import type { Editor } from '@tiptap/core';
import type { Node as ProseMirrorNode } from '@tiptap/pm/model';

export function clearFormatting(editor: Editor) {
  clearMarks(editor);
  removeEmptyParagraphs(editor);
}

/**
 * Removes all marks (bold, italic, underline, etc.) from the selection,
 * converts all block nodes in the selection to paragraphs,
 * and removes most classes and attributes from paragraphs.
 * This effectively resets formatting for the selected content.
 */
function clearMarks(editor: Editor) {
  const { from, to } = editor.state.selection;

  if (from === to) {
    return;
  }

  const { paragraph } = editor.state.schema.nodes;

  editor.chain()
    .focus()
    .setTextSelection({ from, to })
    .unsetAllMarks()
    .command(({ tr, state }) => {
      state.doc.nodesBetween(from, to, (node, pos) => {
        // Strip attributes from <ul>/<ol>/<blockquote> but keep their type & structure
        if (node.type.name === 'bulletList' || node.type.name === 'orderedList' || node.type.name === 'blockquote') {
          if (node.attrs && Object.keys(node.attrs).length) {
            tr.setNodeMarkup(pos, node.type, {}, node.marks);
          }

          return true;
        }

        // Strip attributes from any textblock (heading, paragraph inside list, etc.)
        if (node.isBlock && node.type.isTextblock) {
          tr.setNodeMarkup(pos, paragraph, {}, node.marks);
        }

        return true;
      });

      return true;
    })
    .run();
}

/**
 * Removes paragraphs with no text content or only &nbsp; in the current selection.
 */
function removeEmptyParagraphs(editor: Editor) {
  const { state } = editor;
  const { from, to } = state.selection;
  const positions: { pos: number; size: number }[] = [];

  state.doc.nodesBetween(from, to, (node: ProseMirrorNode, pos: number) => {
    if (isEmptyParagraph(node)) {
      positions.push({ pos, size: node.nodeSize });
    }
  });

  if (positions.length) {
    let tr = state.tr;
    positions.sort((a, b) => b.pos - a.pos).forEach(({ pos, size }) => {
      tr = tr.delete(pos, pos + size);
    });

    editor.view.dispatch(tr);
  }
}

/**
 * Returns true if the paragraph is empty or only contains text nodes with &nbsp; or whitespace.
 */
function isEmptyParagraph(node: ProseMirrorNode): boolean {
  if (node.type.name !== 'paragraph') {
    return false;
  }

  if (node.childCount === 0) {
    return true;
  }

  const onlyTextChildren = [...node.children].every(childNode => childNode.isText);
  const onlyNbspOrWhitespace = node.textContent.match(/[^\u00A0\s]/g) === null;

  return onlyTextChildren && onlyNbspOrWhitespace;
}
