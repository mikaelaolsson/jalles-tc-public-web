/**
 * @vitest-environment jsdom
 */
import { describe, it, expect, afterEach } from 'vitest';
import { Editor } from '@tiptap/core';
import { StarterKit } from '@tiptap/starter-kit';
import { clearFormatting } from './tiptap-clear-formatting';
import { selectBlocks, selectText } from '../../test-utils/tiptap-selection-helpers';
import { umbTiptapTestExtensions } from '../../test-utils/tiptap-umb-editor-setup';

describe('clearFormatting', () => {
  let editor: Editor;

  afterEach(() => {
    editor?.destroy();
  });

  it('clears formatting with full document selection', () => {
    editor = new Editor({
      extensions: [StarterKit, ...umbTiptapTestExtensions],
      content: `
        <h2><strong>Heading</strong></h2>
        <p><em>Bold and <u>italic</u></em></p>
        <p>&nbsp;</p>
        <ul><li><strong>List item</strong></li></ul>
        <p></p>
        <p>Normal</p>
        <p>   </p>
      `,
    });

    editor.commands.selectAll();
    clearFormatting(editor);

    expect(editor.getHTML()).toBe('<p>Heading</p><p>Bold and italic</p><ul><li><p>List item</p></li></ul><p>Normal</p>');
  });

  it('clears formatting across selected blocks', () => {
    editor = new Editor({
      extensions: [StarterKit, ...umbTiptapTestExtensions],
      content: `
        <h3><strong>A</strong> Head</h3>
        <p><em>Second <u>Line</u></em></p>
        <p></p>
        <p>Fourth <strong>Line</strong></p>
    `,
    });

    selectBlocks(editor, 1, 3);
    clearFormatting(editor);

    expect(editor.getHTML()).toBe('<p>A Head</p><p>Second Line</p><p>Fourth <strong>Line</strong></p>');
  });

  it('clears formatting for a text selection inside a paragraph', () => {
    editor = new Editor({
      extensions: [StarterKit, ...umbTiptapTestExtensions],
      content: `<p>Start <strong>BoldWord</strong> <em>End</em></p>`,
    });

    selectText(editor, 'BoldWord');
    clearFormatting(editor);

    expect(editor.getHTML()).toBe('<p>Start BoldWord <em>End</em></p>');
  });

  it('clears formatting only for selected block', () => {
    editor = new Editor({
      extensions: [StarterKit, ...umbTiptapTestExtensions],
      content: `
        <p>One <strong>bold</strong></p>
        <p>Two <em>italic</em></p>
        <p data-junk="yes" style="color: blue;">Three <strong>under</strong></p>
      `,
    });

    selectBlocks(editor, 3, 3);
    clearFormatting(editor);

    expect(editor.getHTML()).toBe(
      '<p>One <strong>bold</strong></p>' +
      '<p>Two <em>italic</em></p>' +
      '<p>Three under</p>'
    );
  });

  it('clears formatting only for selected partial word', () => {
    editor = new Editor({
      extensions: [StarterKit, ...umbTiptapTestExtensions],
      content: `<p><strong>WholeWord</strong> Tail</p>`,
    });

    // select "Whole"
    selectText(editor, 'WholeWord', 0, 5);
    clearFormatting(editor);

    expect(editor.getHTML()).toBe('<p>Whole<strong>Word</strong> Tail</p>');
  });

  it('strips attributes from ul/ol/blockquote while preserving structure', () => {
    editor = new Editor({
      extensions: [StarterKit, ...umbTiptapTestExtensions],
      content: `
        <ul class="main-list" data-x="1" style="padding:0">
          <li data-li="a"><p class="p-class"><strong>Item</strong> 1</p></li>
          <li><p>Item <em>2</em></p></li>
        </ul>
        <blockquote class="quote-x" data-q="1" style="color:red">
          <p><strong>Quoted</strong> <em>text</em></p>
        </blockquote>
        <ol class="ordered" data-y="2" style="margin:0">
          <li><p><u>First</u></p></li>
          <li data-li="b"><p>Second</p></li>
        </ol>
        <p>I need to add this useless paragraph here because the test setup otherwise adds an empty one for no reason at all and fails the test.</p>
      `,
    });

    editor.commands.selectAll();
    clearFormatting(editor);

    expect(editor.getHTML()).toBe(
      '<ul>' +
        '<li><p>Item 1</p></li>' +
        '<li><p>Item 2</p></li>' +
      '</ul>' +
      '<blockquote><p>Quoted text</p></blockquote>' +
      '<ol>' +
        '<li><p>First</p></li>' +
        '<li><p>Second</p></li>' +
      '</ol>' +
      '<p>I need to add this useless paragraph here because the test setup otherwise adds an empty one for no reason at all and fails the test.</p>'
    );
  });
});

