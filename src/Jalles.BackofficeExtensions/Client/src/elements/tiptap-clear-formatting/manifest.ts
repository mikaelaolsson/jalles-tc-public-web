export default [
    {
        type: "tiptapExtension",
        kind: "button",
        alias: "tiptap-clear-formatting-extension",
        name: "TipTap Clear Formatting Extension",
        api: () => import("./tiptap-clear-formatting-api.ts"),
        meta: {
            icon: "icon-clear-formatting",
            label: "Clear Formatting",
            group: "#tiptap_extGroup_formatting"
        }
    } as UmbExtensionManifest,
    {
        type: "tiptapToolbarExtension",
        kind: "button",
        alias: "tiptap-clear-formatting-toolbar-extension",
        name: "TipTap Clear Formatting Toolbar Extension",
        forExtensions: ["tiptap-clear-formatting-extension"],
        js: () => import("./tiptap-clear-formatting-extension.ts"),
        meta: {
            alias: "clear-formatting",
            icon: "icon-wand",
            label: "Clear Formatting"
        }
    } as UmbExtensionManifest
]
