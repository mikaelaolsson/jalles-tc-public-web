/**
 * Test setup helpers for Tiptap that mimic (as closely as we need) Umbraco's real
 * editor extensions. This lets our tests exercise the same node names / attributes
 * that production Umbraco code relies on (rte blocks, embedded media).
 */

import { mergeAttributes, Node } from '@tiptap/core';

// Adapted from https://github.com/umbraco/Umbraco-CMS/blob/release-17.0.0/src/Umbraco.Web.UI.Client/src/packages/tiptap/extensions/block/block.tiptap-extension.ts
// MIT License
const UMB_BLOCK_RTE_DATA_CONTENT_KEY = "data-content-key";

const umbRteBlock = Node.create({
  name: 'umbRteBlock',
  group: 'block',
  content: undefined,
  atom: true,
  marks: '',
  draggable: true,
  selectable: true,

  addAttributes() {
    return {
      [UMB_BLOCK_RTE_DATA_CONTENT_KEY]: {
        isRequired: true,
      },
    };
  },

  parseHTML() {
    return [{ tag: `umb-rte-block[${UMB_BLOCK_RTE_DATA_CONTENT_KEY}]` }];
  },

	renderHTML({ HTMLAttributes }) {
		return ['umb-rte-block', HTMLAttributes];
	},
});

const umbRteBlockInline = umbRteBlock.extend({
  name: 'umbRteBlockInline',
  group: 'inline',
  inline: true,

  parseHTML() {
    return [{ tag: `umb-rte-block-inline[${UMB_BLOCK_RTE_DATA_CONTENT_KEY}]` }];
  },

  renderHTML({ HTMLAttributes }: { HTMLAttributes: Record<string, unknown> }) {
    return ['umb-rte-block-inline', HTMLAttributes];
  },
});

// Adapted from https://github.com/umbraco/Umbraco-CMS/blob/release-17.0.0/src/Umbraco.Web.UI.Client/src/packages/tiptap/extensions/embedded-media/embedded-media.tiptap-extension.ts
// MIT License

// Denna ska ev bort då den inte verkar användas längre i Umbraco 17?
const iframeEmbed = Node.create({
  name: 'iframeEmbed',
  group: 'inline',
  inline: true,
  atom: true,
  selectable: false,
  draggable: false,
  marks: '',
  addAttributes() {
    return {
      src: { default: null, parseHTML: el => el.getAttribute('src') },
      width: { default: null, parseHTML: el => el.getAttribute('width') },
      height: { default: null, parseHTML: el => el.getAttribute('height') },
      title: { default: null, parseHTML: el => el.getAttribute('title') },
      frameborder: { default: null, parseHTML: el => el.getAttribute('frameborder') },
      allow: { default: null, parseHTML: el => el.getAttribute('allow') },
      referrerpolicy: { default: null, parseHTML: el => el.getAttribute('referrerpolicy') },
      allowfullscreen: { default: null, parseHTML: el => el.hasAttribute('allowfullscreen') ? '' : null },
    };
  },
  parseHTML() {
    return [{ tag: 'iframe' }];
  },
  renderHTML({ HTMLAttributes }) {
    return ['iframe', HTMLAttributes];
  },
});

const umbEmbeddedMedia = Node.create<UmbEmbeddedMediaOptions>({
	name: 'umbEmbeddedMedia',
	group() {
		return this.options.inline ? 'inline' : 'block';
	},
	inline() {
		return this.options.inline;
	},

	atom: true,
	marks: '',
	draggable: true,
	selectable: true,

	addAttributes() {
		return {
			'data-embed-constrain': { default: false },
			'data-embed-height': { default: 240 },
			'data-embed-url': { default: null },
			'data-embed-width': { default: 360 },
			markup: { default: null, parseHTML: (element) => element.innerHTML },
		};
	},

	parseHTML() {
		return [{ tag: '.umb-embed-holder', priority: 100 }];
	},

	renderHTML({ HTMLAttributes }) {
		const { markup, ...attrs } = HTMLAttributes;
		const embed = document.createRange().createContextualFragment(markup);
		return [this.options.inline ? 'span' : 'div', mergeAttributes({ class: 'umb-embed-holder' }, attrs), embed];
	},

	addCommands() {
		return {
			setEmbeddedMedia:
				(options) =>
				({ commands }) => {
					const attrs = {
						markup: options.markup,
						'data-embed-url': options.url,
						'data-embed-width': options.width,
						'data-embed-height': options.height,
						'data-embed-constrain': options.constrain,
					};
					return commands.insertContent({ type: this.name, attrs });
				},
		};
	},
});

interface UmbEmbeddedMediaOptions {
	inline: boolean;
}

export const umbTiptapTestExtensions = [
  umbRteBlock,
  umbRteBlockInline,
  iframeEmbed,
  umbEmbeddedMedia,
];
