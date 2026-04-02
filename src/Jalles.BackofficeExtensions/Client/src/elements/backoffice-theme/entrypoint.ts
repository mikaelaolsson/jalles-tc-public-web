import {
  UmbEntryPointOnInit,
  UmbEntryPointOnUnload,
} from "@umbraco-cms/backoffice/extension-api";

export const onInit: UmbEntryPointOnInit = () => {
  injectBackofficeCss();

  console.info("[Backoffice Extensions] Initialization complete. 🎉");
};

export const onUnload: UmbEntryPointOnUnload = () => {
  console.info("[Backoffice Extensions] Unloaded successfully. 👋");
};

function injectBackofficeCss() {
  const css = document.createElement('link');
  css.rel = 'stylesheet';
  css.href = '/App_Plugins/JallesBackofficeExtensions/assets/backoffice-theme.css';
  document.head.appendChild(css);
}
