import { css, html, customElement } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';

const logo = "/App_Plugins/JallesBackofficeExtensions/assets/jalles-logo-yellow.svg";

@customElement('start-dashboard')
export class StartDashboardElement extends UmbLitElement {
  override render() {
    return html`
      <div class="start-dashboard">
        <img class="logo" src="${logo}" />
        <h2>Väkommen till Jalles TC Backoffice!</h2>
      </div>
    `;
  }

  static override readonly styles = css`
    .start-dashboard {
      display: flex;
      flex-direction: column;
      justify-content: center;
      max-width: 500px;
      text-align: center;
    }

    .start-dashboard .logo {
      height: 20rem;
    }
  `;
}

export default StartDashboardElement;

declare global {
  interface HTMLElementTagNameMap {
    'start-dashboard': StartDashboardElement;
  }
}
