import { UmbTiptapToolbarElementApiBase } from '@umbraco-cms/backoffice/tiptap';
import type { Editor } from '@tiptap/core';
import { clearFormatting } from './tiptap-clear-formatting';

export default class UmbTiptapToolbarClearFormattingExtensionApi extends UmbTiptapToolbarElementApiBase {
  override execute(editor?: Editor) {
    if (!editor) {
      return;
    }

    clearFormatting(editor);
  }
}
